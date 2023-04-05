# Install the AWS CLI
# $ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue'; 
# Invoke-WebRequest -UseBasicParsing https://awscli.amazonaws.com/AWSCLIV2.msi -OutFile AWSCLIV2.msi; 
# Start-Process msiexec -Wait -ArgumentList '/I AWSCLIV2.msi /quiet';
# $env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine")
# Remove-Item -Force AWSCLIV2.msi

$command = "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12"
Invoke-Expression $command
Invoke-WebRequest -Uri "https://awscli.amazonaws.com/AWSCLIV2.msi" -Outfile C:\AWSCLIV2.msi
$arguments = "/i `"C:\AWSCLIV2.msi`" /quiet"
Start-Process msiexec.exe -ArgumentList $arguments -Wait
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine")