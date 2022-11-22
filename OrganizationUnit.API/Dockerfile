#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OrganizationUnit.API/OrganizationUnit.API.csproj", "OrganizationUnit.API/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["OrganizationUnit.Infrastructure/OrganizationUnit.Infrastructure.csproj", "OrganizationUnit.Infrastructure/"]
COPY ["IntegrationEventLogEF/IntegrationEventLogEF.csproj", "IntegrationEventLogEF/"]
COPY ["OrganizationUnit.Domain/OrganizationUnit.Domain.csproj", "OrganizationUnit.Domain/"]
RUN dotnet restore "OrganizationUnit.API/OrganizationUnit.API.csproj"
COPY . .
WORKDIR "/src/OrganizationUnit.API"
RUN dotnet build "OrganizationUnit.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrganizationUnit.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrganizationUnit.API.dll"]
