# Pull the Windows Server IIS base image
FROM mcr.microsoft.com/windows/servercore/iis

WORKDIR /inetpub/wwwroot

# Remove the default IIS start page
RUN powershell -NoProfile -Command "Remove-Item -Recurse C:\inetpub\wwwroot\*"

# Install IIS features and management tools
RUN powershell -Command \
    Add-WindowsFeature Web-Server; \
    Add-WindowsFeature Web-Mgmt-Tools;

# Copy the WebApp.zip and extract its contents
COPY WebApp.zip /temp/WebApp.zip
RUN powershell -Command \
    Expand-Archive -Path C:\temp\WebApp.zip -DestinationPath C:\temp\extracted; \
    Remove-Item -Path C:\temp\WebApp.zip -Force

# Copy the extracted contents to the IIS web root
COPY /temp/extracted/Content\D_C\a\1\s\Tipstaff\obj\Release\Package\PackageTmp\* /inetpub/wwwroot/Tipstaff

# Expose the IIS port
EXPOSE 80

# Set the entrypoint to use the ServiceMonitor for IIS
ENTRYPOINT ["powershell", "C:\\ServiceMonitor.exe", "w3svc"]
