#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ContractManagement.API/ContractManagement.API.csproj", "ContractManagement.API/"]
COPY ["ContractManagement.Caching/ContractManagement.Caching.csproj", "ContractManagement.Caching/"]
COPY ["WebHost.Customization/WebHost.Customization.csproj", "WebHost.Customization/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["ContractManagement.Domain/ContractManagement.Domain.csproj", "ContractManagement.Domain/"]
COPY ["IntegrationEventLogEF/IntegrationEventLogEF.csproj", "IntegrationEventLogEF/"]
COPY ["ContractManagement.Infrastructure/ContractManagement.Infrastructure.csproj", "ContractManagement.Infrastructure/"]
COPY ["ContractManagement.Utility/ContractManagement.Utility.csproj", "ContractManagement.Utility/"]
RUN dotnet restore "ContractManagement.API/ContractManagement.API.csproj"
COPY . .
WORKDIR "/src/ContractManagement.API"
RUN dotnet build "ContractManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContractManagement.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContractManagement.API.dll"]