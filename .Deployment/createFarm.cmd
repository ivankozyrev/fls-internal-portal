c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe Set-ExecutionPolicy Unrestricted
echo %CD%
cd .\scripts
c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe .\createFarm.ps1
cd..