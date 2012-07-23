function WaitForJobToFinish([string]$Identity)
{   
    $job = Get-SPTimerJob | ?{ $_.Name -like "*solution-deployment*$Identity*" }
    $maxwait = 30
    $currentwait = 0

    if (!$job)
    {
        Write-Host -f Red '[ERROR] Timer job not found'
    }
    else
    {
        $jobName = $job.Name
        Write-Host -NoNewLine "[WAIT] Waiting to finish job $jobName"        
        while (($currentwait -lt $maxwait))
        {
            Write-Host -f Green -NoNewLine .
            $currentwait = $currentwait + 1
            Start-Sleep -Seconds 2
            if (!(Get-SPTimerJob $jobName)){
                break;
            }
        }
        Write-Host  -f Green "...Done!"
    }
}

function RetractSolution([string]$Identity)
{
    Write-Host "[RETRACT] Uninstalling $Identity"    
    Write-Host -NoNewLine "[RETRACT] Does $Identity contain any web application-specific resources to deploy?"
    $solution = Get-SPSolution | where { $_.Name -match $Identity }
    if($solution.ContainsWebApplicationResource)
    {
        Write-Host  -f Yellow "...Yes!"        
        Write-Host -NoNewLine "[RETRACT] Uninstalling $Identity from all web applications"            
        Uninstall-SPSolution -identity $Identity  -allwebapplications -Confirm:$false
        Write-Host -f Green "...Done!"
    }
    else
    {
        Write-Host  -f Yellow  "...No!"        
        Uninstall-SPSolution -identity $Identity -Confirm:$false    
        Write-Host -f Green "...Done!"
    }

    WaitForJobToFinish

    Write-Host -NoNewLine  '[UNINSTALL] Removing solution:' $SolutionName
    Remove-SPSolution -Identity $Identity -Confirm:$false
    Write-Host -f Green "...Done!"
}

function DeploySolution([string]$Path, [string]$Identity)
{
    Write-Host -NoNewLine "[DEPLOY] Adding solution:" $Identity
    Add-SPSolution $Path
    Write-Host -f Green "...Done!"

    Write-Host -NoNewLine "[DEPLOY] Does $Identity contain any web application-specific resources to deploy?"
    $solution = Get-SPSolution | where { $_.Name -match $Identity }

    if($solution.ContainsWebApplicationResource)
    {
        Write-Host -f Yellow "...Yes!"        
        Write-Host -NoNewLine "[DEPLOY] Installing $Identity for all web applications"    
        Install-SPSolution -Identity $Identity -AllWebApplications -GACDeployment

    }
    else
    {
        Write-Host -f Yellow "...No!"        
        Write-Host -NoNewLine "[DEPLOY] Globally deploying $Identity"    
        Install-SPSolution -Identity $Identity -GACDeployment
    }
    Write-Host -f Green "...Done!"

    WaitForJobToFinish
}

$snapin = Get-PSSnapin | Where-Object { $_.Name -eq "Microsoft.SharePoint.Powershell" }
if ($snapin -eq $null) {
    Write-Host "[INIT] Loading SharePoint Powershell Snapin"
    Add-PSSnapin "Microsoft.SharePoint.Powershell"
}

Write-Host "[INIT] Locating WSP files to be deployed"
$wsps = Get-ChildItem . *.wsp | where-object { !($_.psiscontainer) }

foreach ($wsp in $wsps)
{
    $identity = $wsp.Name
    $path = $wsp.FullName
    Write-Host "[INFO] ----------------------------------------"
    Write-Host "[INFO] Installing $Identity"
    Write-Host -NoNewLine "[INFO] Determining if $Identity is already installed"

    $isInstalled = Get-SPSolution | where { $_.Name -eq $identity }
    if ($isInstalled)
    {
        Write-Host -ForegroundColor Yellow "...Yes!"
        (RetractSolution $identity)
        (DeploySolution $path $identity)
    }
    else
    {
        Write-Host -ForegroundColor Yellow "...No!"
        (DeploySolution $path $identity)
    }

    Write-Host -NoNewline "[INFO] Installation and deployment of $Identity"
    Write-Host -ForegroundColor Green "...Done!"
}