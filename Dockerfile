FROM  --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src
COPY ["CleanArchitecture.Api/CleanArchitecture.Api.csproj", "CleanArchitecture.Api/"]
COPY ["CleanArchitecture.Application/CleanArchitecture.Application.csproj", "CleanArchitecture.Application/"]
COPY ["CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "CleanArchitecture.Domain/"]
COPY ["CleanArchitecture.Shared/CleanArchitecture.Shared.csproj", "CleanArchitecture.Shared/"]
COPY ["CleanArchitecture.Proto/CleanArchitecture.Proto.csproj", "CleanArchitecture.Proto/"]
COPY ["CleanArchitecture.gRPC/CleanArchitecture.gRPC.csproj", "CleanArchitecture.gRPC/"]
COPY ["CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj", "CleanArchitecture.Infrastructure/"]
RUN dotnet restore "CleanArchitecture.Api/CleanArchitecture.Api.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src/CleanArchitecture.Api"
RUN dotnet build "CleanArchitecture.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build -a $TARGETARCH

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
RUN dotnet publish "CleanArchitecture.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -a $TARGETARCH

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=5s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost/healthz || exit 1
EXPOSE 80
ENTRYPOINT ["dotnet", "CleanArchitecture.Api.dll"]
