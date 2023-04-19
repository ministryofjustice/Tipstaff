# Pull the Windows Server IIS base image
FROM mcr.microsoft.com/windows/servercore/iis

# Install IIS features and management tools
RUN powershell -Command \
    Add-WindowsFeature Web-Server; \
    Add-WindowsFeature Web-Mgmt-Tools; \
    Add-WindowsFeature NET-Framework-45-ASPNET; \
    Add-WindowsFeature Web-Asp-Net45

# Download and install .NET 4.6.1
ADD https://download.microsoft.com/download/E/4/1/E4173890-A24A-4936-9FC9-AF930FE3FA40/NDP461-KB3102436-x86-x64-AllOS-ENU.exe /NDP461-KB3102436-x86-x64-AllOS-ENU.exe
RUN /NDP461-KB3102436-x86-x64-AllOS-ENU.exe /q /norestart /log C:\net461.log

# Copy the WebApp.zip file
COPY WebApp.zip /inetpub/

# Extract the contents of WebApp.zip to a temporary directory
RUN powershell -Command " \
    Expand-Archive -Path C:\inetpub\WebApp.zip -DestinationPath C:\temp_extracted; \
    xcopy C:\temp_extracted\Content\D_C\a\1\s\Tipstaff\obj\Release\Package\PackageTmp\* C:\inetpub\wwwroot /E /I \
    "

# Remove the temporary extracted directory
RUN powershell -Command "Remove-Item -Recurse -Force C:\temp_extracted"

# Show the contents of wwwroot
RUN powershell -Command "Get-ChildItem -Path C:\inetpub\wwwroot"

# Expose the IIS port
EXPOSE 80