#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Location.API/Location.API.csproj", "Location.API/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
RUN dotnet restore "Location.API/Location.API.csproj"
COPY . .
WORKDIR "/src/Location.API"
RUN dotnet build "Location.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Location.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Location.API.dll"]