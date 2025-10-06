#!/bin/bash
set -e

echo "⏳ Esperando a que SQL Server esté listo..."

sleep 15

echo "🚀 Iniciando aplicación..."
dotnet sbBancoTurnos.dll