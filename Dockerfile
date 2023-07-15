FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore

# copy everything else and build app
COPY CleanArchitecture.Api/. ./CleanArchitecture.Api/
WORKDIR /app/CleanArchitecture.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/CleanArchitecture.Api/out ./

EXPOSE 80
ENTRYPOINT ["dotnet", "CleanArchitecture.Api.dll"]
