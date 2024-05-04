FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY ./CutelCaptureThePhone.Web ./CutelCaptureThePhone.Web
COPY ./CutelCaptureThePhone.Data.Postgres ./CutelCaptureThePhone.Data.Postgres
COPY ./CutelCaptureThePhone.Core ./CutelCaptureThePhone.Core

WORKDIR /App/CutelCaptureThePhone.Web

RUN dotnet restore

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/CutelCaptureThePhone.Web/out .
ENTRYPOINT ["dotnet", "CutelCaptureThePhone.Web.dll"]