c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe Set-ExecutionPolicy Unrestricted
echo %CD%
c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe .\Scripts\farm-create.ps1 -db %1 > ..\logs\farm-create-log.txt 2>&1
cd..