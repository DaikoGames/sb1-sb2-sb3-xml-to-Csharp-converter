cd
cd ./Install
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
./Install-Chocolately.ps1
./dotnet-install.ps1 -Runtime dotnet -Version 9.0.0
./InstallAvalonia.ps1
./Install-Node-and-NPM.ps1