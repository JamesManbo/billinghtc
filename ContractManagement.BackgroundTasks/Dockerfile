#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ContractManagement.BackgroundTasks/ContractManagement.BackgroundTasks.csproj", "ContractManagement.BackgroundTasks/"]
COPY ["ContractManagement.Domain/ContractManagement.Domain.csproj", "ContractManagement.Domain/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["ContractManagement.Utility/ContractManagement.Utility.csproj", "ContractManagement.Utility/"]
COPY ["ContractManagement.Infrastructure/ContractManagement.Infrastructure.csproj", "ContractManagement.Infrastructure/"]
COPY ["IntegrationEventLogEF/IntegrationEventLogEF.csproj", "IntegrationEventLogEF/"]
RUN dotnet restore "ContractManagement.BackgroundTasks/ContractManagement.BackgroundTasks.csproj"
COPY . .
WORKDIR "/src/ContractManagement.BackgroundTasks"
RUN dotnet build "ContractManagement.BackgroundTasks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContractManagement.BackgroundTasks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContractManagement.BackgroundTasks.dll"]
