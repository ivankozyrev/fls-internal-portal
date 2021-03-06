. ".\helpers.ps1"
function StopSPServices
{
    $Services = ("SPTimerV4")

    Write-Host -ForegroundColor Blue "- Stoping SP services..."
    ForEach ($SvcName in $Services)
    {
        If (Get-Service -Name $SvcName -ErrorAction SilentlyContinue)
        {
            Stop-Service -Name $SvcName
            Write-Host -ForegroundColor Blue " - Service $SvcName is now stoped"
        }
    }
    Write-Host -ForegroundColor White "- Finished stoping SP services."
}


WriteInfo RegisterPSSnapin
RegisterPSSnapin

StopSPServices