FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS base
WORKDIR /CheckoutAPI
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
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