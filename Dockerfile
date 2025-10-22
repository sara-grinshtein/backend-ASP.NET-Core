# ---------------------------
# STAGE 1: Build & Publish
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build
WORKDIR /src

# Copy solution first to enable better restore caching
# (We copy each .csproj into its folder to leverage Docker layer cache)
COPY PrijectYedidim.sln ./
COPY Common/Common.csproj Common/
COPY Repository/Repository.csproj Repository/
COPY Service/Service.csproj Service/
COPY Mock/Mock.csproj Mock/
COPY PrijectYedidim/PrijectYedidim.csproj PrijectYedidim/

# Restore NuGet packages based on the copied project files
RUN dotnet restore

# Now copy the entire source tree
COPY . .

# Publish only the Web project in Release; UseAppHost=false makes the image slimmer
RUN dotnet publish PrijectYedidim/PrijectYedidim.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---------------------------
# STAGE 2: Runtime
# ---------------------------
FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy AS runtime
WORKDIR /app

# Bring the published output from the build stage
COPY --from=build /app/publish ./

# Render sets PORT dynamically at runtime; we default to 8080 for local runs
ENV PORT=8080
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose is informative for local/docker usage (Render handles routing for you)
EXPOSE 8080

# Start the web app (DLL name matches the web project)
ENTRYPOINT ["dotnet", "PrijectYedidim.dll"]
