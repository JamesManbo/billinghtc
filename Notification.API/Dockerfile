#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Notification.API/Notification.API.csproj", "Notification.API/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
COPY ["MongoClusterRepository/MongoClusterRepository.csproj", "MongoClusterRepository/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
RUN dotnet restore "Notification.API/Notification.API.csproj"
COPY . .
WORKDIR "/src/Notification.API"
RUN dotnet build "Notification.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notification.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.API.dll"]