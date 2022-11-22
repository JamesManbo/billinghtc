#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CMS.APIGateway/CMS.APIGateway.csproj", "CMS.APIGateway/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
RUN dotnet restore "CMS.APIGateway/CMS.APIGateway.csproj"
COPY . .
WORKDIR "/src/CMS.APIGateway"
RUN dotnet build "CMS.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CMS.APIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CMS.APIGateway.dll"]