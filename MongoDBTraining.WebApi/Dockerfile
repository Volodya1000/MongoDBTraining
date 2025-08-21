# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MongoDBTraining.WebApi/MongoDBTraining.WebApi.csproj", "MongoDBTraining.WebApi/"]
COPY ["MongoDBTraining.Application/MongoDBTraining.Application.csproj", "MongoDBTraining.Application/"]
COPY ["MongoDBTraining.Domain/MongoDBTraining.Domain.csproj", "MongoDBTraining.Domain/"]
COPY ["MongoDBTraining.Persistence/MongoDBTraining.Persistence.csproj", "MongoDBTraining.Persistence/"]
RUN dotnet restore "./MongoDBTraining.WebApi/MongoDBTraining.WebApi.csproj"
COPY . .
WORKDIR "/src/MongoDBTraining.WebApi"
RUN dotnet build "./MongoDBTraining.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MongoDBTraining.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MongoDBTraining.WebApi.dll"]