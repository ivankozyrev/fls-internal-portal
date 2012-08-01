. ".\helpers.ps1"

Function StartSPServices
{
    $ServicesToSetAutomatic = ("SPAdminV4","SPWriterV4")

    Write-Host -ForegroundColor Blue "- Starting SP services..."
    ForEach ($SvcName in $ServicesToSetAutomatic)
    {
    If (Get-Service -Name $SvcName -ErrorAction SilentlyContinue)
    {
    Set-Service -Name $SvcName -StartupType Automatic
    Start-Service -Name $SvcName
    Write-Host -ForegroundColor Blue " - Service $SvcName is now set to Automatic start"
    }
    }
    Write-Host -ForegroundColor White "- Finished re-enabling SP demo services."
}


RegisterPSSnapin

$secpasswd = ConvertTo-SecureString "Pass4311" -AsPlainText -Force
$adminCredentials = New-Object System.Management.Automation.PSCredential ("DOMAIN\ikozyrev", $secpasswd)
$dbConfigName = "SharePoint_Config"
$dbAdminContentName = "SharePoint_AdminContent"
$secPassphrase = ConvertTo-SecureString "ABa$$w0rd" -AsPlainText -Force
$dbServer = "ikozyrev\sharepoint"

#// todo add SharePoint_Shell_Access to database http://www.sharepointassist.com/2010/01/29/the-local-farm-is-not-accessible-cmdlets-with-featuredependencyid-are-not-registered/
New-SPConfigurationDatabase -DatabaseName $dbConfigName `
        -DatabaseServer $dbServer `
        -AdministrationContentDatabaseName $dbAdminContentName `
        –Passphrase $secPassphrase `
        –FarmCredentials $adminCredentials

Install-SPHelpCollection -All

Initialize-SPResourceSecurity

Install-SPService

#// todo: it better to desire only needed features
Install-SPFeature -AllExistingFeatures

New-SPCentralAdministration -Port 2048

Install-SPApplicationContent

$webApplicationName = "SharePoin 80t"
$applicationPoolName = "SharePoint"
$applicationPoolAccount = "NETWORKSERVICE"
$webApplicationPort = 80
$webApplicationUrl = ""

## New-SPWebApplication -Name "SharePoint" -ApplicationPool "SharePoint 80" -ApplicationPoolAccount "NETWORKSERVICE" -Port 80 
New-SPWebApplication -Name $webApplicationName -ApplicationPool $applicationPoolName -ApplicationPoolAccount $applicationPoolAccount -Port $webApplicationPort

StartSPServices

##New-SPWebApplication -Name "Contoso Internet Site" -Port 80 -HostHeader sharepoint.contoso.com -URL "https://www.contoso.com" -ApplicationPool "ContosoAppPool" -ApplicationPoolAccount (Get-SPManagedAccount "DOMAIN\jdoe")