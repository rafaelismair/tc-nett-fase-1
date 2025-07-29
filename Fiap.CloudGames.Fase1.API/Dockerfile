# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 4080
EXPOSE 4081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fiap.CloudGames.Fase1.API/Fiap.CloudGames.Fase1.API.csproj", "Fiap.CloudGames.Fase1.API/"]
COPY ["Fiap.CloudGames.Fase1.Application/Fiap.CloudGames.Fase1.Application.csproj", "Fiap.CloudGames.Fase1.Application/"]
COPY ["Fiap.CloudGames.Fase1.Infrastructure/Fiap.CloudGames.Fase1.Infrastructure.csproj", "Fiap.CloudGames.Fase1.Infrastructure/"]
COPY ["Fiap.CloudGames.Fase1.Domain/Fiap.CloudGames.Fase1.Domain.csproj", "Fiap.CloudGames.Fase1.Domain/"]
RUN dotnet restore "./Fiap.CloudGames.Fase1.API/Fiap.CloudGames.Fase1.API.csproj"
COPY . .
WORKDIR "/src/Fiap.CloudGames.Fase1.API"
RUN dotnet build "./Fiap.CloudGames.Fase1.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fiap.CloudGames.Fase1.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fiap.CloudGames.Fase1.API.dll"]