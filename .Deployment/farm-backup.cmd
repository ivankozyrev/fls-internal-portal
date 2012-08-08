c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe Set-ExecutionPolicy Unrestricted
echo %CD%
cd .\scripts
powershell .\farm-backup.ps1 -db %1 > farm-backup-log.txt 2>&1
cd..