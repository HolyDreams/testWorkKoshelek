#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/testWorkKoshelek.GetterClient/testWorkKoshelek.GetterClient.csproj", "src/testWorkKoshelek.GetterClient/"]
COPY ["src/Core/Core.Domain/Core.Domain.csproj", "src/Core/Core.Domain/"]
RUN dotnet restore "./src/testWorkKoshelek.GetterClient/testWorkKoshelek.GetterClient.csproj"
COPY . .
WORKDIR "/src/src/testWorkKoshelek.GetterClient"
RUN dotnet build "./testWorkKoshelek.GetterClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./testWorkKoshelek.GetterClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "testWorkKoshelek.GetterClient.dll"]