FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo o código-fonte
COPY . .

# Restaurar dependências
RUN dotnet restore

# Compilar e publicar
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar a aplicação publicada
COPY --from=build /app/publish .

# Definir variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_ROLL_FORWARD=LatestMajor
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 80

ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]