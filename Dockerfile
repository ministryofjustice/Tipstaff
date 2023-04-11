# Use the official .NET Framework 4.6.1 ASP.NET image as the base image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.6.1-windowsservercore-ltsc2019

# Install IIS features and management tools
RUN powershell -Command \
    Add-WindowsFeature Web-Server; \
    Add-WindowsFeature Web-Mgmt-Tools

# Copy the application files
COPY your_application_folder/ /inetpub/wwwroot/

# Expose the IIS port
EXPOSE 80