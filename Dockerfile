# Use the nightly .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build-env
WORKDIR /app

# Update NO_PROXY so that api.nuget.org bypasses the proxy
ENV NO_PROXY=api.nuget.org,localhost,intel.com,192.168.0.0/16,172.16.0.0/12,127.0.0.0/8,10.0.0.0/8

# Copy the csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose port 80
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "SpaceAPI.dll"]
