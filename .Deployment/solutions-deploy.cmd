c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe Set-ExecutionPolicy Unrestricted
echo %CD%
c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe .\Scripts\solutions-deploy.ps1 > ..\logs\farm-backup-differential-log.txt 2>&1