# Set the base image
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build

# Set the working directory
WORKDIR /app

# Copy your application files
COPY . ./

# Restore NuGet packages
RUN nuget restore

# Build the application
RUN msbuild /p:Configuration=Release

# Set up the runtime image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8 AS runtime

# Set the working directory
WORKDIR /inetpub/wwwroot

# Copy the built application from the build image
COPY --from=build /app/. /inetpub/wwwroot/