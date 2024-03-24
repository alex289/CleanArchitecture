FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /app

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore -a $TARGETARCH

# copy everything else and build app
COPY CleanArchitecture.Api/. ./CleanArchitecture.Api/
WORKDIR /app/CleanArchitecture.Api
RUN dotnet publish -c Release -o out -a $TARGETARCH

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
ARG TARGETARCH
WORKDIR /app
COPY --from=build /app/CleanArchitecture.Api/out ./

HEALTHCHECK --interval=30s --timeout=5s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost/healthz || exit 1

EXPOSE 80
ENTRYPOINT ["dotnet", "CleanArchitecture.Api.dll"]
