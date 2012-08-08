Add-PSSnapin Microsoft.SharePoint.PowerShell
$backupPath = "c:\my_backup_delete\"
Backup-SPFarm -Directory $backupPath -BackupMethod full -BackupThreads 10