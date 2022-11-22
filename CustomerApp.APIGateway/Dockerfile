#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["CustomerApp.APIGateway/CustomerApp.APIGateway.csproj", "CustomerApp.APIGateway/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
RUN dotnet restore "CustomerApp.APIGateway/CustomerApp.APIGateway.csproj"
COPY . .
WORKDIR "/src/CustomerApp.APIGateway"
RUN dotnet build "CustomerApp.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerApp.APIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerApp.APIGateway.dll"]