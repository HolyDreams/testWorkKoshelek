#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/testWorkKoshelek.API/testWorkKoshelek.API.csproj", "src/testWorkKoshelek.API/"]
COPY ["src/Base/Infrastructure/Base.Infrastructure.AutoMapper/Base.Infrastructure.AutoMapper.csproj", "src/Base/Infrastructure/Base.Infrastructure.AutoMapper/"]
COPY ["src/Base/Base.Interfaces/Base.Interfaces.csproj", "src/Base/Base.Interfaces/"]
COPY ["src/Core/Core.Domain/Core.Domain.csproj", "src/Core/Core.Domain/"]
COPY ["src/Core/Core.Interfaces/Core.Interfaces.csproj", "src/Core/Core.Interfaces/"]
COPY ["src/Base/Base.Models/Base.Models.csproj", "src/Base/Base.Models/"]
COPY ["src/Infrastructure/Infrastructure.DataAccess/Infrastructure.DataAccess.csproj", "src/Infrastructure/Infrastructure.DataAccess/"]
COPY ["src/Base/Infrastructure/Base.Infrastructure.DataAccess/Base.Infrastructure.DataAccess.csproj", "src/Base/Infrastructure/Base.Infrastructure.DataAccess/"]
COPY ["src/Infrastructure/Infrastructure.WebSockets/Infrastructure.WebSockets.csproj", "src/Infrastructure/Infrastructure.WebSockets/"]
COPY ["src/Base/Infrastructure/Base.Infrastructure.WebSockets/Base.Infrastructure.WebSockets.csproj", "src/Base/Infrastructure/Base.Infrastructure.WebSockets/"]
COPY ["src/Logic/Logic.Interfaces/Logic.Interfaces.csproj", "src/Logic/Logic.Interfaces/"]
COPY ["src/Logic/Logic/Logic.csproj", "src/Logic/Logic/"]
RUN dotnet restore "./src/testWorkKoshelek.API/testWorkKoshelek.API.csproj"
COPY . .
WORKDIR "/src/src/testWorkKoshelek.API"
RUN dotnet build "./testWorkKoshelek.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./testWorkKoshelek.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "testWorkKoshelek.API.dll"]