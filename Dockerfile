# Use the official .NET Framework 4.6.1 ASP.NET image as the base image
FROM mcr.microsoft.com/windows/servercore/iis

# Install IIS features and management tools
RUN powershell -Command \
    Add-WindowsFeature Web-Server; \
    Add-WindowsFeature Web-Mgmt-Tools

# Copy the application files to the Docker image
COPY WebApp/ /inetpub/wwwroot

# Expose the IIS port
EXPOSE 80