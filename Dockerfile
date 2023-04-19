# Pull the Windows Server IIS base image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019

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