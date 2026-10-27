# syntax=docker/dockerfile:1

# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first so `dotnet restore` is cached until dependencies change.
COPY MediCare.slnx ./
COPY MediCare.Api/MediCare.Api.csproj MediCare.Api/
COPY MediCare.Application/MediCare.Application.csproj MediCare.Application/
COPY MediCare.Contract/MediCare.Contract.csproj MediCare.Contract/
COPY MediCare.Domain/MediCare.Domain.csproj MediCare.Domain/
COPY MediCare.Persistence/MediCare.Persistence.csproj MediCare.Persistence/
RUN dotnet restore MediCare.Api/MediCare.Api.csproj

COPY . .
RUN dotnet publish MediCare.Api/MediCare.Api.csproj \
        -c Release \
        -o /app/publish \
        --no-restore \
        /p:UseAppHost=false

# ---------- runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Npgsql negotiates GSS encryption by default; without libgssapi it logs a
# "Cannot load library libgssapi_krb5.so.2" line on every new connection.
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

COPY --from=build --chown=$APP_UID:$APP_UID /app/publish ./

# aspnet images ship a non-root "app" user (uid 1654).
USER $APP_UID

ENTRYPOINT ["dotnet", "MediCare.Api.dll"]
