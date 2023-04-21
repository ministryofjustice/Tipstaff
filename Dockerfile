# Pull the Windows Server IIS base image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

# Add the ARG values from the build pipeline
ARG RDS_PASSWORD
ARG RDS_USERNAME

# Set the required environment variables
ENV CurServer="DEVELOPMENT"
ENV DB_NAME="tipstaffdbdev"
ENV RDS_HOSTNAME="tipstaffdbdev.cx4fhff2nzo3.eu-west-2.rds.amazonaws.com"
ENV RDS_PORT="5432"
ENV RDS_PASSWORD=$RDS_PASSWORD
ENV RDS_USERNAME=$RDS_USERNAME
ENV supportEmail="dts-legacy-apps-support-team@hmcts.net"
ENV supportTeam="DTS Legacy Apps Support Team"
ENV ida:ClientId="09730739-d16b-47e6-a8c6-007ad48bed2d"

# Copy the WebApp.zip file
COPY WebApp.zip /inetpub/

# Extract the contents of WebApp.zip to a temporary directory and clean up the files that are no longer needed
RUN powershell -Command " \
    Expand-Archive -Path C:\inetpub\WebApp.zip -DestinationPath C:\temp_extracted; \
    xcopy C:\temp_extracted\Content\D_C\a\1\s\Tipstaff\obj\Release\Package\PackageTmp\* C:\inetpub\wwwroot /E /I; \
    Remove-Item -Path C:\inetpub\wwwroot\WebApp.zip -Force; \
    Remove-Item -Recurse -Force C:\temp_extracted \
    "

# Download ServiceMonitor
RUN powershell -Command " \
    Invoke-WebRequest -Uri 'https://dotnetbinaries.blob.core.windows.net/servicemonitor/2.0.1.6/ServiceMonitor.exe' -OutFile 'C:\ServiceMonitor.exe' \
    "

# Expose the IIS port
EXPOSE 80

# Start ServiceMonitor to manage the W3SVC service
ENTRYPOINT ["C:\\ServiceMonitor.exe", "W3SVC"]
