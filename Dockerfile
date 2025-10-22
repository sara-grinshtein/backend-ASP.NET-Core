# ---------------------------
# STAGE 1: Build & Publish
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build
WORKDIR /src

# Copy solution and project files first (for better restore caching)
# If you keep the .sln only for local dev, it's fine; we'll restore by .csproj.
COPY PrijectYedidim.sln ./
COPY Common/Common.csproj Common/
COPY Repository/Repository.csproj Repository/
COPY Service/Service.csproj Service/
COPY Mock/Mock.csproj Mock/
COPY PrijectYedidim/PrijectYedidim.csproj PrijectYedidim/

# 🔧 Restore ONLY the web project to avoid missing test project
RUN dotnet restore PrijectYedidim/PrijectYedidim.csproj

# Copy the rest of the source
COPY . .

# Publish the web project
RUN dotnet publish PrijectYedidim/PrijectYedidim.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---------------------------
# STAGE 2: Runtime
# ---------------------------
FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

ENV PORT=8080
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "PrijectYedidim.dll"]
