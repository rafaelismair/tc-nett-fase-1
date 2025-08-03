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

# Final image
FROM base AS final

# Mudar para root para instalar pacotes
USER root

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=8e036d4bacc7671bbc6088daa6144af9FFFFNRAL \
NEW_RELIC_APP_NAME="fiapcloudgamesfase1api"


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
WORKDIR /app
COPY --from=publish /app/publish .

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 
ENV CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A}
ENV CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=8e036d4bacc7671bbc6088daa6144af9FFFFNRAL \
NEW_RELIC_APP_NAME="fiapcloudgamesfase1api" 

# Voltar para o usuário original, se necessário
USER $APP_UID

ENTRYPOINT ["dotnet", "Fiap.CloudGames.Fase1.API.dll"]