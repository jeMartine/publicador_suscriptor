# ===== Etapa 1: Build del frontend Svelte =====
FROM node:20 AS frontend-build
WORKDIR /app
COPY app_publisher/package*.json ./
RUN npm install
COPY app_publisher/ .
RUN npm run build

# ===== Etapa 2: Build del backend .NET =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar proyecto y restaurar dependencias
COPY BancoTurnosApp/BancoTurnosApp.csproj ./BancoTurnosApp/
RUN dotnet restore ./BancoTurnosApp/BancoTurnosApp.csproj

# Copiar todo el código
COPY BancoTurnosApp/ ./BancoTurnosApp/
WORKDIR /src/BancoTurnosApp

# Publicar el proyecto
RUN dotnet publish -c Release -o /app/out

# ===== Etapa 3: Imagen final =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiamos backend y frontend
COPY --from=build /app/out .
COPY --from=frontend-build /app/dist ./wwwroot

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BancoTurnosApp.dll"]
