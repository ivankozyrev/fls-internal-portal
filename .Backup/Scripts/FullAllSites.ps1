function GetBackupFileName([string]$basePath) {
	$date = Get-Date;
	$result = $basePath + '.' + $date.Year + $date.Month + $date.Day + $date.Hour + $date.Minute + $date.Second;
	return [string]$result;	
}

Add-PSSnapin Microsoft.SharePoint.PowerShell
$sites = Get-SPSite
foreach($site in $sites) {
	$date = Get-Date
	$backupPath = GetBackupFileName('c:\Temp\spbackup\' + $site.Id);
	echo $backupPath
	Backup-SPSite -identity $site.Id -path $backupPath
}