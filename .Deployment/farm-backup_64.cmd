c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe Set-ExecutionPolicy Unrestricted
echo %CD%
cd .\scripts
c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe .\farm-backup.ps1 -db %1 > pslog.txt 2>&1
cd..