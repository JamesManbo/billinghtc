#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SystemUserIdentity.API/SystemUserIdentity.API.csproj", "SystemUserIdentity.API/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
COPY ["OrganizationUnit.Infrastructure/OrganizationUnit.Infrastructure.csproj", "OrganizationUnit.Infrastructure/"]
COPY ["IntegrationEventLogEF/IntegrationEventLogEF.csproj", "IntegrationEventLogEF/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["OrganizationUnit.Domain/OrganizationUnit.Domain.csproj", "OrganizationUnit.Domain/"]
RUN dotnet restore "SystemUserIdentity.API/SystemUserIdentity.API.csproj"
COPY . .
WORKDIR "/src/SystemUserIdentity.API"
RUN dotnet build "SystemUserIdentity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SystemUserIdentity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SystemUserIdentity.API.dll"]