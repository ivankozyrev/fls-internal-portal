param([string]$out,[string]$diff)

. ".\helpers.ps1"


WriteInfo "RegisterPSSnapin"
RegisterPSSnapin

$backupPath = "c:\my_backup_delete\"
$threadNumber = 10
$backupMethod = "full"

if(![string]::IsNullOrEmpty($out))
{
    Write-Host "[ARGS] Input Argument 'out'=$out"
    $backupPath = $out
}

if(![string]::IsNullOrEmpty($diff))
{
    Write-Host "[ARGS] Input Argument 'type'=$type (differential)"
    $backupMethod = "differential"
}

Write-Host "Creating backup ($backupMethod) Backup folder: '$backupPath'"

Backup-SPFarm -Directory $backupPath -BackupMethod $backupMethod -BackupThreads $threadNumber

Write-Host "END"