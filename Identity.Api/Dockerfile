FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY "DotNetCoreBackendHomework.sln" "DotNetCoreBackendHomework.sln"

COPY "WebApi/WebApi.csproj" "WebApi/WebApi.csproj"
COPY "Identity.Api/Identity.Api.csproj" "Identity.Api/Identity.Api.csproj"
COPY "Identity.Api/Identity.Api.csproj" "WebApi.Tests/WebApi.Tests.csproj"

RUN dotnet restore "DotNetCoreBackendHomework.sln"

COPY . .
WORKDIR /src/Identity.Api
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]