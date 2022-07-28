#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["eart_keycloak_microservice/eart_keycloak_microservice.csproj", "eart_keycloak_microservice/"]
RUN dotnet restore "eart_keycloak_microservice/eart_keycloak_microservice.csproj"
COPY . .
WORKDIR "/src/eart_keycloak_microservice"
RUN dotnet build "eart_keycloak_microservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eart_keycloak_microservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eart_keycloak_microservice.dll"]