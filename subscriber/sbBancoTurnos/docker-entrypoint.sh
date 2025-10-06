#!/bin/bash
set -e

echo "⏳ Esperando a que SQL Server esté listo..."

sleep 1

echo "🚀 Iniciando aplicación..."
dotnet sbBancoTurnos.dll