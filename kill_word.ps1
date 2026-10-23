Get-Process WINWORD -ErrorAction SilentlyContinue | Stop-Process -Force
Write-Host "WINWORD processes terminated."
