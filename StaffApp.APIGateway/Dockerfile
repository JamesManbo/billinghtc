#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["StaffApp.APIGateway/StaffApp.APIGateway.csproj", "StaffApp.APIGateway/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
RUN dotnet restore "StaffApp.APIGateway/StaffApp.APIGateway.csproj"
COPY . .
WORKDIR "/src/StaffApp.APIGateway"
RUN dotnet build "StaffApp.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StaffApp.APIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StaffApp.APIGateway.dll"]