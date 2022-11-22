#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DebtManagement.BackgroundTasks/DebtManagement.BackgroundTasks.csproj", "DebtManagement.BackgroundTasks/"]
COPY ["DebtManagement.Infrastructure/DebtManagement.Infrastructure.csproj", "DebtManagement.Infrastructure/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
COPY ["IntegrationEventLogEF/IntegrationEventLogEF.csproj", "IntegrationEventLogEF/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["DebtManagement.Domain/DebtManagement.Domain.csproj", "DebtManagement.Domain/"]
RUN dotnet restore "DebtManagement.BackgroundTasks/DebtManagement.BackgroundTasks.csproj"
COPY . .
WORKDIR "/src/DebtManagement.BackgroundTasks"
RUN dotnet build "DebtManagement.BackgroundTasks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DebtManagement.BackgroundTasks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DebtManagement.BackgroundTasks.dll"]
