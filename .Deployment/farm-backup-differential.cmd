c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe Set-ExecutionPolicy Unrestricted
echo %CD%
cd .\scripts
powershell .\farm-backup.ps1 -diff true -db %1% > ..\logs\farm-backup-differential-log.txt 2>&1
cd..