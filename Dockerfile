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
COPY Tipstaff.zip /inetpub;
RUN powershell -Command \
    Expand-Archive -Path C:\inetpub\Tipstaff.zip -DestinationPath C:\inetpub; \
    Remove-Item -Path C:\inetpub\Tipstaff.zip -Force

RUN powershell -Command "Get-ChildItem -Path C:\inetpub -Recurse"

# Copy the extracted contents to the IIS web root
# COPY .\a\1\s\WebApp\Extracted\Content\D_C\a\1\s\Tipstaff\obj\Release\Package\PackageTmp\* /inetpub/wwwroot/Tipstaff

# Expose the IIS port
EXPOSE 80

# Set the entrypoint to use the ServiceMonitor for IIS
ENTRYPOINT ["powershell", "C:\\ServiceMonitor.exe", "w3svc"]
