# Pull the Windows Server IIS base image
FROM mcr.microsoft.com/windows/servercore/iis

# Install IIS features and management tools
RUN powershell -Command \
    Add-WindowsFeature Web-Server; \
    Add-WindowsFeature Web-Mgmt-Tools

# Copy the WebApp.zip file
COPY WebApp.zip /inetpub/

# Extract the contents of WebApp.zip to a temporary directory
RUN powershell -Command \
    Expand-Archive -Path C:\inetpub\WebApp.zip -DestinationPath C:\temp_extracted

# Output the contents of the extracted directory to the pipeline console
RUN powershell -Command "Get-ChildItem -Path C:\temp_extracted -Recurse"

# Select a particular nested file within the extracted directory
# (Replace 'path\to\nested\file.ext' with the actual relative path to the file you want to copy)
COPY C:\temp_extracted\Content\D_C\a\1\s\Tipstaff\obj\Release\Package\PackageTmp\* C:\inetpub\wwwroot

# Output the contents of C:\inetpub\wwwroot
RUN powershell -Command "Get-ChildItem -Path C:\inetpub\wwwroot -Recurse"

# Remove the temporary extracted directory
RUN powershell -Command "Remove-Item -Recurse -Force C:\temp_extracted"

# Expose the IIS port
EXPOSE 80
