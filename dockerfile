FROM microsoft/dotnet:2.2-sdk AS base
WORKDIR /Checkout

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Checkout.sln ./
COPY Checkout/CheckoutAPI.csproj Checkout/
COPY . .
WORKDIR /src/Checkout
RUN dotnet build -c Release -o /Checkout

FROM build AS publish
RUN dotnet publish -c Release

FROM base AS final
WORKDIR /Checkout
COPY --from=publish /Checkout .
ENTRYPOINT ["dotnet", "CheckoutAPI.dll"]