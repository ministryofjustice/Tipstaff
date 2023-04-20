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

# Copy the WebApp.zip file
COPY WebApp.zip /inetpub/

# Extract the contents of WebApp.zip to a temporary directory
RUN powershell -Command " \
    Expand-Archive -Path C:\inetpub\WebApp.zip -DestinationPath C:\temp_extracted; \
    xcopy C:\temp_extracted\Content\D_C\a\1\s\Tipstaff\obj\Release\Package\PackageTmp\* C:\inetpub\wwwroot /E /I \
    "

# Remove the temporary extracted directory
RUN powershell -Command "Remove-Item -Recurse -Force C:\temp_extracted"

# Expose the IIS port
EXPOSE 80

# Start the W3SVC service
ENTRYPOINT ["powershell", "-Command", "Start-Service W3SVC; Invoke-WebRequest http://localhost -UseBasicParsing; iisreset /restart; while ($true) { Start-Sleep -Seconds 3600 }"]