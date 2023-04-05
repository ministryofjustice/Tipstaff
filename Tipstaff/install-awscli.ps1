# Install the AWS CLI
$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue'; 
Invoke-WebRequest -UseBasicParsing https://awscli.amazonaws.com/AWSCLIV2.msi -OutFile AWSCLIV2.msi; 
Start-Process msiexec -Wait -ArgumentList '/I AWSCLIV2.msi /quiet'; 
Remove-Item -Force AWSCLIV2.msi