. ".\helpers.ps1"

function RemoveWebApplications()
{
    WriteFuncInfo "Removing all web applications..."
    
    $applications = Get-SPWebApplication
    if($applications -eq $null)
    {
        WriteFuncInfo "No web applications found."
        return
    }
    WriteFuncInfo "Removing web applications $($applications.Count):"
    
    foreach($application in $applications)
    {
        WriteFuncInfo "$($application.Name) with database and iis site"
        Remove-SPWebApplication $application -DeleteIISSite -RemoveContentDatabases -Confirm:$false  
    }   
}

function RemoveCentralAdministration()
{
    WriteFuncInfo "Removing central administrations:"
    $applications = Get-SPWebApplication -IncludeCentralAdministration | where-object {$_.IsAdministrationWebApplication -eq $true }
    if($applications -eq $null)
    {
        WriteFuncInfo "No central administrations found."
        return
    }
    foreach($application in $applications)
    {
        WriteFuncInfo "$($application.Url)"
        
        # for central administration web application Remove-SPWebApplication cmdlet requests confiramtion
        # even if parameter '-Confirm:$false' is used
        
        # Remove-SPWebApplication $application -DeleteIISSite -RemoveContentDatabases -Confirm:$false 
        WriteFuncInfo "`t removing iis site"
        $application.UnprovisionGlobally($true);

        WriteFuncInfo "`t removing databases"
		foreach ($contentDb in $application.ContentDatabases)
		{
			$contentDb.Unprovision();
		}

		$application.Delete();
     }    
}

function RemoveServiceApplications()
{
    WriteFuncInfo "Removing service applications..."
    
    $applications = Get-SPServiceApplication
    if($applications -eq $null)
    {
        WriteFuncInfo "No service applications found."
        return
    }
    WriteFuncInfo "Removing service applications $($applications.Count):"
    
    foreach($application in $applications)
    {
        WriteFuncInfo "$($application.Name) with data"
        UnPublish-SPServiceApplication $application.Id -ErrorAction SilentlyContinue -Confirm:$false
        Remove-SPServiceApplication $application.Id -RemoveData -ErrorAction SilentlyContinue -Confirm:$false

        if((Get-SPServiceApplication  $application.Id) -eq $null)
        {
            WriteFuncInfo "Removed"
        }
        else
        {
            WriteFuncInfo "Error"
        }
    } 
}

function StopServicesInstances()
{
    WriteFuncInfo "stoping services instances..."
    
    $services = Get-SPServiceInstance
    if($services -eq $null)
    {
        WriteFuncInfo "No services instances found."
        return
    }
    WriteFuncInfo "Stoping services instances $($services.Count):"
    
    foreach($service in $services)
    {
        WriteFuncInfoLine $($service.TypeName)
        WriteFuncInfo (Stop-SPServiceInstance $service.Id -Confirm:$false).Status 
    } 
}

function RemoveFarmSolutions()
{
    $solutions = Get-SPSolution
    
    if($solutions -eq $null)
    {
        WriteFuncInfo "No solutions found."
        return
    }
        
    foreach($solution in $solutions)
    {
        WriteFuncInfo "[SOLUTION] "$solution.SolutionId"|`t"$solution.Name"|"
        WriteFuncInfo "FEATURES:"
        $features = $site.WebApplication.Farm.FeatureDefinitions | where-object {$_.SolutionId -eq $solution.SolutionId} | Sort-Object Scope
        if($features -eq $null) 
        {
            WriteFuncInfo "No features"
        }
        else
        {
            foreach($feature in $features)
            {
                WriteFuncInfoLine $feature.Scope "`t"
                WriteFuncInfo $feature.DisplayName
            }
        }
        
        WriteFuncInfo "Uninstalling solution..."
        Uninstall-SPSolution $solution -Confirm:$false

        WriteFuncInfoLine "Waiting."
        #Wait for solution to be uninstalled
        WaitForJobToFinish($solution.Name)

        #Remove solution from the farm
        WriteFuncInfo "Removing solution..."
        Remove-SPSolution $solution -Confirm:$false
        WriteFuncInfo "Removed solution."
        WriteFuncInfo "`n----------------------------------------------------------------------`n"
    }
}

function RemoveConfigurationDataBase()
{
    $databases = Get-SPDatabase | where-object {$_.Type -ne "Configuration Database"}
    $configDatabase = Get-SPDatabase | where-object {$_.Type -eq "Configuration Database"}
    if(($databases -eq $null) -and ($configDatabase -eq $null))
    {
        WriteFuncInfo "No databases found."
        return
    }
    WriteFuncInfo "Deleting databases $($databases.Count):"
    
    foreach($database in $databases)
    {
        WriteFuncInfo "$($database.Name)"
        $database
    } 
    
    WriteFuncInfo "Disconnecting configuration database"
    Disconnect-SPConfigurationDatabase -Confirm:$false 
    
    WriteFuncInfo "Removing configuration database"
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $configDatabase.DatabaseConnectionString
    
    Write-Host "Loaded: "([System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")).FullName
    $SQlSvr = New-Object Microsoft.SqlServer.Management.Smo.Server $($conn.DataSource)
    WriteFuncInfo "Killing all processes for $($configDatabase.Name)"
    $SQlSvr.KillAllprocesses("$($configDatabase.Name)")
    WriteFuncInfo "Drop $($configDatabase.Name)"
    $SQlSvr.Databases[$($configDatabase.Name)].drop() 
}


## START

WriteInfo RegisterPSSnapin
RegisterPSSnapin

WriteInfo RemoveWebApplications
RemoveWebApplications

WriteInfo RemoveFarmSolutions
RemoveFarmSolutions

WriteInfo RemoveCentralAdministration
RemoveCentralAdministration

WriteInfo RemoveServiceApplications
RemoveServiceApplications

WriteInfo StopServicesInstances
StopServicesInstances

WriteInfo RemoveConfigurationDataBase
RemoveConfigurationDataBase 