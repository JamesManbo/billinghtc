#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DebtManagement.API/DebtManagement.API.csproj", "DebtManagement.API/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["DebtManagement.Infrastructure/DebtManagement.Infrastructure.csproj", "DebtManagement.Infrastructure/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
COPY ["IntegrationEventLogEF/IntegrationEventLogEF.csproj", "IntegrationEventLogEF/"]
COPY ["DebtManagement.Domain/DebtManagement.Domain.csproj", "DebtManagement.Domain/"]
RUN dotnet restore "DebtManagement.API/DebtManagement.API.csproj"
COPY . .
WORKDIR "/src/DebtManagement.API"
RUN dotnet build "DebtManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DebtManagement.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DebtManagement.API.dll"]