FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime-env
WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Auction.Api.dll"]