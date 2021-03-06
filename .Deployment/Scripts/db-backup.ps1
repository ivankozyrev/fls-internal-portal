. ".\helpers.ps1"
[void][System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.ConnectionInfo');            
[void][System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.Management.Sdk.Sfc');            
[void][System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO');            
# Requiered for SQL Server 2008 (SMO 10.0).            
[void][System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMOExtended'); 


function Backup([Microsoft.SqlServer.Management.Smo.Server]$server,[string]$databaseName,[string]$outDirectory)
{
    $backup = New-Object ("Microsoft.SqlServer.Management.Smo.Backup");            
    $backup.Action = "Database";            
    $backup.Database = $databaseName;
    $backupFilePath = $outDirectory + $databaseName + "_full"+ ".bak"         
    $backup.Devices.AddDevice($backupFilePath, "File");            
    $backup.BackupSetDescription = "Full backup of " + $databaseName + " " + $timestamp;            
    $backup.Incremental = 0;            
    Write-Host "Creating backup for Database $databaseName in $backupFilePath"           
    $backup.SqlBackup($server);     
    # For db with recovery mode <> simple: Log backup.            
    If ($server.Databases[$databaseName].RecoveryModel -ne 3)            
    {            
        $logBackup = New-Object ("Microsoft.SqlServer.Management.Smo.Backup");            
        $logBackup.Action = "Log";            
        $logBackup.Database = $databaseName; 
        $backupFilePath = $outDirectory + $databaseName + "_log" + ".trn"               
        $logBackup.Devices.AddDevice($backupFilePath, "File");            
        $logBackup.BackupSetDescription = "Log backup of " + $databaseName + " " + $timestamp;            
        #Specify that the log must be truncated after the backup is complete.            
        $logBackup.LogTruncation = "Truncate";
        # Starting log backup process            
        Write-Host "Creating backup for Log $databaseName in $backupFilePath" 
        $logBackup.SqlBackup($server);            
    };            
}

WriteInfo RegisterPSSnapin
RegisterPSSnapin

$databases = Get-SPDatabase
           
$started = (Get-Date -format yyyy-MM-dd-HH-mm-ss);
Write-Output ("Started at: $started");
          
$Dest = "c:\my_backup_delete\$started\"
if($(Get-Item $Dest) -eq $null)  
{
    New-Item $Dest -type directory 
}  

foreach($database in $databases)
{
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $database.DatabaseConnectionString
    $SQlSvr = New-Object Microsoft.SqlServer.Management.Smo.Server $($conn.DataSource)
    Write-Host "Proccessing $($database.Name) on $($SQlSvr.Name)"
    Backup $SQlSvr $database.Name $Dest
}

Write-Output ("Finished at: " + (Get-Date -format  yyyy-MM-dd-HH:mm:ss));