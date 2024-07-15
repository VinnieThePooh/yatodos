# to run:
# docker build -t todos.api -f ./Todos.Api/Dockerfile - in solution dir
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Todos.Api/Todos.Api.csproj", "Todos.Api/"]
COPY ["Todos.Models/Todos.Models.csproj", "Todos.Models/"]
COPY ["Todos.DataAccess/Todos.DataAccess.csproj", "Todos.DataAccess/"]
RUN dotnet restore "Todos.Api/Todos.Api.csproj"

COPY ["Todos.Api/*", "Todos.Api/"]
COPY ["Todos.Models/*", "Todos.Models/"]
COPY ["Todos.DataAccess/*", "Todos.DataAccess/"]

WORKDIR "/src/Todos.Api"
RUN dotnet build "Todos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Todos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

ENV ASPNETCORE_ENVIRONMENT="Production" \
    ConnectionStrings__DefaultConnection="" \
    ASPNETCORE_URLS="http://localhost:5146"

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todos.Api.dll"]
