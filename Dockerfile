# Pull the Windows Server IIS base image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2022

# Create a directory for your application
WORKDIR C:/app

# Copy your WebApp.zip file into the container
COPY WebApp.zip .

# Unzip the WebApp.zip file
RUN powershell -Command " \
    Expand-Archive -Path C:\app\WebApp.zip -DestinationPath C:\temp_extracted; \
    xcopy C:\temp_extracted\Content\D_C\a\Tipstaff\Tipstaff\Tipstaff\obj\Release\Package\PackageTmp\* C:\inetpub\wwwroot /E /I; \
    Remove-Item -Path C:\app\WebApp.zip -Force; \
    Remove-Item -Recurse -Force C:\temp_extracted \
    "

# Expose the IIS port
EXPOSE 80

# Start ServiceMonitor to manage the W3SVC service
ENTRYPOINT ["C:\\ServiceMonitor.exe", "W3SVC"]
