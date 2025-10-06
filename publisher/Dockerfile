# ===== Etapa 1: Build del frontend Svelte =====
FROM node:20 AS frontend-build
WORKDIR /app
COPY app_publisher/package*.json ./
ENV NODE_ENV=development
RUN npm install
COPY app_publisher/ .
RUN npm run build

# ===== Etapa 2: Build del backend .NET =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY BancoTurnosApp/BancoTurnosApp.csproj ./BancoTurnosApp/
RUN dotnet restore ./BancoTurnosApp/BancoTurnosApp.csproj

COPY BancoTurnosApp/ ./BancoTurnosApp/
WORKDIR /src/BancoTurnosApp

RUN dotnet publish -c Release -o /app/out

# ===== Etapa 3: Imagen final =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out .
COPY --from=frontend-build /app/build ./wwwroot


# Script de inicio con reintentos
COPY --from=build /src/BancoTurnosApp/docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh || true

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["/docker-entrypoint.sh"]