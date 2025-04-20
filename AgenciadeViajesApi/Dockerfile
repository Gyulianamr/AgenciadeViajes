# Etapa 1: Compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copiar todos los archivos
COPY . ./

# Restaurar paquetes y publicar en modo Release
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa 2: Ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

# Exponer el puerto
EXPOSE 80

# Ejecutar la API
ENTRYPOINT ["dotnet", "AgenciadeViajesApi.dll"]
