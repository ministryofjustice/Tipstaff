# Pull the Windows Server IIS base image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

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

# Start the W3SVC service
ENTRYPOINT ["powershell", "-Command", "$DB_NAME = [Environment]::GetEnvironmentVariable('DB_NAME', 'Process'); $RDS_HOSTNAME = [Environment]::GetEnvironmentVariable('RDS_HOSTNAME', 'Process'); $RDS_PASSWORD = [Environment]::GetEnvironmentVariable('RDS_PASSWORD', 'Process'); $RDS_PORT = [Environment]::GetEnvironmentVariable('RDS_PORT', 'Process'); $RDS_USERNAME = [Environment]::GetEnvironmentVariable('RDS_USERNAME', 'Process'); [Environment]::SetEnvironmentVariable('DB_NAME', $DB_NAME, 'Machine'); [Environment]::SetEnvironmentVariable('RDS_HOSTNAME', $RDS_HOSTNAME, 'Machine'); [Environment]::SetEnvironmentVariable('RDS_PASSWORD', $RDS_PASSWORD, 'Machine'); [Environment]::SetEnvironmentVariable('RDS_PORT', $RDS_PORT, 'Machine'); [Environment]::SetEnvironmentVariable('RDS_USERNAME', $RDS_USERNAME, 'Machine'); Start-Service W3SVC; Invoke-WebRequest http://localhost -UseBasicParsing; iisreset /restart; while ($true) { Start-Sleep -Seconds 3600 }"]
