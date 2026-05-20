FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos os arquivos do projeto para o container
COPY . .

# CORREÇÃO: Aponta a restauração direto para o .csproj da camada de apresentação
RUN dotnet restore src/MedicalScheduling.Presentation/MedicalScheduling.Presentation.csproj

# Executa o publish (o publish já builda automaticamente)
RUN dotnet publish src/MedicalScheduling.Presentation/MedicalScheduling.Presentation.csproj \
  -c Release \
  -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia os arquivos publicados da imagem de build
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "MedicalScheduling.Presentation.dll"]