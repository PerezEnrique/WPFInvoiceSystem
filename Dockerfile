FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App
COPY . ./

WORKDIR /App/WPFInvoiceSystem.API
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/WPFInvoiceSystem.API/out .
ENTRYPOINT ["dotnet", "WPFInvoiceSystem.API.dll"]