# Base image with .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only csproj and restore
COPY StockApi.csproj ./
RUN dotnet restore "StockApi.csproj"

# Copy everything elseF
COPY . ./

# Build
RUN dotnet build "StockApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
RUN dotnet publish "StockApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "StockApi.dll"]
