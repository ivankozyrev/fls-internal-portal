function RegisterPSSnapin()
{
    $snapin = Get-PSSnapin | Where-Object { $_.Name -eq "Microsoft.SharePoint.Powershell" }
    if ($snapin -eq $null) {
        Write-Host "[INIT] Loading SharePoint Powershell Snapin"
        Add-PSSnapin "Microsoft.SharePoint.Powershell"
    }
}

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

function WriteFuncInfo([string]$message)
{
    Write-Host "`t`t: $message" 
}

function WriteInfo([string]$message)
{
    Write-Host "`t: $message" 
}

function WriteFuncInfoLine([string]$message)
{
    Write-Host -NoNewLine "`t  $message" 
}