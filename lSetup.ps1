# Install Chocolately
Start-Process powershell -ArgumentList "-Command", "Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))" -Verb RunAs -Wait
# Install mkcert
$psi = New-Object System.Diagnostics.ProcessStartInfo
$psi.FileName = "choco"
$psi.Arguments = "install mkcert -y"
$psi.Verb = "runas"
$psi.UseShellExecute = $true
[System.Diagnostics.Process]::Start($psi).WaitForExit()
# Run scripts to create certificates
cd Auth/Install
./lGenCert.ps1
cd ../..