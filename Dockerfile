# Usar a imagem base do .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Definir o diretório de trabalho
WORKDIR /app

# Copiar csproj e restaurar dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar o restante do código
COPY . ./

# Construir a aplicação
RUN dotnet publish -c Release -o out

# Usar a imagem base do .NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build /app/out .

# Definir o comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "ApiMegev.dll"]

