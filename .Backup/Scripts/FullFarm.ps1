Add-PSSnapin Microsoft.SharePoint.PowerShell
$backupPath = c:\Temp\spbackup
Backup-SPFarm -Directory $backupPath -BackupMethod full -BackupThreads 10