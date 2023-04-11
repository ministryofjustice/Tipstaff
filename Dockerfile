# Set the base image
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build

# Install IIS, .NET 4.6.1, and other required features
RUN powershell -Command \
    Add-WindowsFeature Web-Server; \
    Add-WindowsFeature NET-Framework-45-ASPNET; \
    Add-WindowsFeature Web-Asp-Net45; \
    Add-WindowsFeature Web-Mgmt-Tools; \
    Invoke-WebRequest -Uri "https://download.microsoft.com/download/1/E/5/1E5F1C0A-0D5B-426A-A603-1798B951DDAE/NDP461-KB3102436-x86-x64-AllOS-ENU.exe" -OutFile dotnet461-installer.exe; \
    Start-Process dotnet461-installer.exe -ArgumentList "/q /norestart" -Wait; \
    Remove-Item dotnet461-installer.exe

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

# Expose the IIS port
EXPOSE 80
