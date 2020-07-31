#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/bbv_MicroserviceDemo.Order.API/bbv_MicroserviceDemo.Order.API.csproj", "src/bbv_MicroserviceDemo.Order.API/"]
COPY ["src/bbv_MicroserviceDemo.Repositories/bbv_MicroserviceDemo.Repositories.csproj", "src/bbv_MicroserviceDemo.Repositories/"]
COPY ["src/bbv_MicroserviceDemo.Order.API.DataAccess/bbv_MicroserviceDemo.Order.API.DataAccess.csproj", "src/bbv_MicroserviceDemo.Order.API.DataAccess/"]
COPY ["src/bbv_MicroserviceDemo.Domains/bbv_MicroserviceDemo.Domains.csproj", "src/bbv_MicroserviceDemo.Domains/"]
COPY ["src/bbv_MicroserviceDemo.Order.API.Events/bbv_MicroserviceDemo.Order.API.Events.csproj", "src/bbv_MicroserviceDemo.Order.API.Events/"]
COPY ["src/bbv_MicroserviceDemo.Common/bbv_MicroserviceDemo.Common.csproj", "src/bbv_MicroserviceDemo.Common/"]
RUN dotnet restore "src/bbv_MicroserviceDemo.Order.API/bbv_MicroserviceDemo.Order.API.csproj"
COPY . .
WORKDIR "/src/src/bbv_MicroserviceDemo.Order.API"
RUN dotnet build "bbv_MicroserviceDemo.Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "bbv_MicroserviceDemo.Order.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bbv_MicroserviceDemo.Order.API.dll"]