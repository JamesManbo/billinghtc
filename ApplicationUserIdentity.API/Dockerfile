#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ApplicationUserIdentity.API/ApplicationUserIdentity.API.csproj", "ApplicationUserIdentity.API/"]
COPY ["WebHost.Customization/WebHost.Customization.csproj", "WebHost.Customization/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["IntegrationEventLogEF/IntegrationEventLogEF.csproj", "IntegrationEventLogEF/"]
RUN dotnet restore "ApplicationUserIdentity.API/ApplicationUserIdentity.API.csproj"
COPY . .
WORKDIR "/src/ApplicationUserIdentity.API"
RUN dotnet build "ApplicationUserIdentity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApplicationUserIdentity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApplicationUserIdentity.API.dll"]