FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/MedicalScheduling.Domain/MedicalScheduling.Domain.csproj", "MedicalScheduling.Domain/"]
COPY ["src/MedicalScheduling.Application/MedicalScheduling.Application.csproj", "MedicalScheduling.Application/"]
COPY ["src/MedicalScheduling.Infrastructure/MedicalScheduling.Infrastructure.csproj", "MedicalScheduling.Infrastructure/"]
COPY ["src/MedicalScheduling.Presentation/WebAPI/MedicalScheduling.Presentation.WebAPI.csproj", "MedicalScheduling.Presentation/WebAPI/"]
RUN dotnet restore "MedicalScheduling.Presentation/WebAPI/MedicalScheduling.Presentation.WebAPI.csproj"
COPY src/ .
WORKDIR /src/MedicalScheduling.Presentation/WebAPI
RUN dotnet publish "MedicalScheduling.Presentation.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MedicalScheduling.Presentation.WebAPI.dll"]
