# ===== Etapa 1: Build del frontend Svelte =====
FROM node:20 AS frontend-build
WORKDIR /app
COPY app_subscriber/package*.json ./
ENV NODE_ENV=development
RUN npm install
COPY app_subscriber/ .
RUN npm run build

# ===== Etapa 2: Build del backend .NET =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY sbBancoTurnos/sbBancoTurnos.csproj ./sbBancoTurnos/
RUN dotnet restore ./sbBancoTurnos/sbBancoTurnos.csproj

COPY sbBancoTurnos/ ./sbBancoTurnos/
WORKDIR /src/sbBancoTurnos

RUN dotnet publish -c Release -o /app/out

# ===== Etapa 3: Imagen final =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out .
COPY --from=frontend-build /app/dist ./wwwroot


# Script de inicio con reintentos
COPY --from=build /src/sbBancoTurnos/docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh || true

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["/docker-entrypoint.sh"]

