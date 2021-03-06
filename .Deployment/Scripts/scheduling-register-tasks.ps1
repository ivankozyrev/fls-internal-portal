
$user = "System"
$pwd = "Pass4311"
$command = "c:\windows\sysnative\WindowsPowerShell\v1.0\powershell.exe"
$commandArguments = "C:\@Work\SharePoint\master\.Deployment\Scripts\farm-backup.ps1 -out c:\my_backup_delete\"

$Hostname = $Env:computername



$service = new-object -com("Schedule.Service")
Write-Host -NoNewLine "Connecting to service 'Schedule.Service' on $Hostname"
$service.Connect($Hostname)

if($service.Connected -eq $true)
{
    write-Host -f Green "`t Connected"
}
else
{
    write-Host -f Red "`t Not connected"
    return
}
$rootFolder = $service.GetFolder("\")
Write-Host "Creating new task..."
$taskDefinition = $service.NewTask(0)

Write-Host "`t Add RegistrationInfo:"
$taskDefinition.RegistrationInfo.Description = "[FLS] full SP backup"
$taskDefinition.RegistrationInfo.Author = $user

Write-Host "`t Add Settings:"
$taskDefinition.Settings.Enabled = $true
$taskDefinition.Settings.StartWhenAvailable = $true
$taskDefinition.Settings.Hidden = $false

Write-Host "`t Add Triggers:"
$trigger = $taskDefinition.Triggers.Create(2)
$trigger.StartBoundary = "2012-01-01T22:00:00"
$trigger.DaysInterval = 1
$trigger.Id = "DailyTriggerId"
$trigger.Enabled = $true

Write-Host "`t Add Actions:"
$action = $taskDefinition.Actions.Create(0)
$action.Path = $command
$action.Arguments = $commandArguments
$rootFolder.RegisterTaskDefinition('[FLS] full SP backup', $taskDefinition, 2, "System" , $null , 5)