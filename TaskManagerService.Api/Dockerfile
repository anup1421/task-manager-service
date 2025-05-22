FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BackendService.Api/BackendService.Api.csproj", "BackendService.Api/"]
COPY ["BackendService.Core/BackendService.Core.csproj", "BackendService.Core/"]
COPY ["BackendService.Data/BackendService.Data.csproj", "BackendService.Data/"]
COPY ["BackendService.Infrastructure/BackendService.Infrastructure.csproj", "BackendService.Infrastructure/"]
RUN dotnet restore "BackendService.Api/BackendService.Api.csproj"
COPY . .
WORKDIR "/src/BackendService.Api"
RUN dotnet build "BackendService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendService.Api.dll"]
