FROM microsoft/dotnet:2.2-sdk AS base
WORKDIR /CheckoutAPI

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Checkout.sln ./
COPY CheckoutAPI/CheckoutAPI.csproj CheckoutAPI/
COPY . .
WORKDIR /src/CheckoutAPI
RUN dotnet build -c Release -o /CheckoutAPI

FROM build AS publish
RUN dotnet publish -c Release

FROM base AS final
WORKDIR /CheckoutAPI
COPY --from=publish /CheckoutAPI .
ENTRYPOINT ["dotnet", "CheckoutAPI.dll"]