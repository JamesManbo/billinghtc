#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["FeedbackAndReports/Feedback.API/Feedback.API.csproj", "FeedbackAndReports/Feedback.API/"]
COPY ["Global.Models/Global.Models.csproj", "Global.Models/"]
COPY ["Global.Configs/Global.Configs.csproj", "Global.Configs/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["MongoClusterRepository/MongoClusterRepository.csproj", "MongoClusterRepository/"]
COPY ["GenericService/GenericRepository.csproj", "GenericService/"]
RUN dotnet restore "FeedbackAndReports/Feedback.API/Feedback.API.csproj"
COPY . .
WORKDIR "/src/FeedbackAndReports/Feedback.API"
RUN dotnet build "Feedback.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Feedback.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN sed -i "s|DEFAULT@SECLEVEL=2|DEFAULT@SECLEVEL=1|g" /etc/ssl/openssl.cnf

ENTRYPOINT ["dotnet", "Feedback.API.dll"]