# Use the official .NET Framework 4.6.1 ASP.NET image as the base image
FROM mcr.microsoft.com/windows/servercore/iis

# Install IIS features and management tools
RUN powershell -Command \
    Add-WindowsFeature Web-Server; \
    Add-WindowsFeature Web-Mgmt-Tools

# Copy the WebApp.zip file and extract its contents
COPY WebApp.zip /inetpub/
RUN powershell -Command \
    Expand-Archive -Path C:\inetpub\WebApp.zip -DestinationPath C:\inetpub\wwwroot; \
    Remove-Item -Path C:\inetpub\WebApp.zip -Force

# Expose the IIS port
EXPOSE 80