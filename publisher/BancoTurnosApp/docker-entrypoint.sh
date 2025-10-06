#!/bin/bash
set -e

echo "⏳ Esperando a que SQL Server esté listo..."

# Esperar 15 segundos para dar tiempo a SQL Server
sleep 15

echo "🚀 Iniciando aplicación..."
dotnet BancoTurnosApp.dll