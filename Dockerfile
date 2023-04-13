# Pull the Windows Server IIS base image
FROM mcr.microsoft.com/windows/servercore/iis

WORKDIR /inetpub/wwwroot

# Remove the default IIS start page
RUN powershell -NoProfile -Command "Remove-Item -Recurse C:\inetpub\wwwroot\*"

# Install IIS features and management tools
RUN powershell -Command \
    Add-WindowsFeature Web-Server; \
    Add-WindowsFeature Web-Mgmt-Tools \
    Add-WindowsFeature Web-Asp-Net45

# Copy the WebApp.zip file and extract its contents
COPY WebApp.zip /inetpub/wwwroot
RUN powershell -Command \
    Expand-Archive -Path C:\inetpub\wwwroot\WebApp.zip -DestinationPath C:\inetpub\wwwroot; \
    Remove-Item -Path C:\inetpub\wwwroot\WebApp.zip -Force

# Expose the IIS port
EXPOSE 80

# Set the entrypoint to use the ServiceMonitor for IIS
ENTRYPOINT ["powershell", "C:\\ServiceMonitor.exe", "w3svc"]