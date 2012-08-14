c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe Set-ExecutionPolicy Unrestricted
echo %CD%
cd .\scripts
c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe .\farm-cleanup.ps1 > ..\logs\farm-cleanup-log.txt 2>&1

cd..