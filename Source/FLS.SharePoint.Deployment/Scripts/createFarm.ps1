. ".\helpers.ps1"

function StartSPServices
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
    Write-Host -ForegroundColor White "- Finished re-enabling SP services."
}

function AddUserToMnagedAccount([string]$user,[string]$password)
{
    $secpasswd = ConvertTo-SecureString $password -AsPlainText -Force
    $credentials = New-Object System.Management.Automation.PSCredential ($user, $secpasswd)
    $account = Get-SPManagedAccount $credentials
    if($account -eq $null)
    {
        $account = New-SPManagedAccount –Credential $credentials
        WriteFuncInfo "Added $account to sp managed accounts."
    }
    else
    {
        WriteFuncInfo "Account $account already added."
    }
    
}

function CreatConfigurationDatabase([string]$user,[string]$password,[string]$dbServer)
{
    $secpasswd = ConvertTo-SecureString $password -AsPlainText -Force
    $adminCredentials = New-Object System.Management.Automation.PSCredential ($user, $secpasswd)
    $dbConfigName = "SharePoint_Config"
    $dbAdminContentName = "SharePoint_AdminContent"
    $secPassphrase = ConvertTo-SecureString "SP Pa$$w0rd" -AsPlainText -Force
    
    #// todo add SharePoint_Shell_Access to database http://www.sharepointassist.com/2010/01/29/the-local-farm-is-not-accessible-cmdlets-with-featuredependencyid-are-not-registered/
    New-SPConfigurationDatabase -DatabaseName $dbConfigName `
        -DatabaseServer $dbServer `
        -AdministrationContentDatabaseName $dbAdminContentName `
        –Passphrase $secPassphrase `
        –FarmCredentials $adminCredentials 
}

function CreateWebApplication([string]$applicationPoolAccount)
{
    $webApplicationName = "SharePoin 80"
    $applicationPoolName = "SharePoint 80"
    $webApplicationPort = 80
    $webApplicationUrl = ""

    ## New-SPWebApplication -Name "SharePoint" -ApplicationPool "SharePoint 80" -ApplicationPoolAccount "NETWORKSERVICE" -Port 80 
    $application = New-SPWebApplication -Name $webApplicationName -ApplicationPool $applicationPoolName -ApplicationPoolAccount $applicationPoolAccount -Port $webApplicationPort
    WriteFuncInfo "Created $($application.Url), status: $($application.Status)"
}


WriteInfo "RegisterPSSnapin"
RegisterPSSnapin

WriteInfo "CreatConfigurationDatabase"
CreatConfigurationDatabase "DOMAIN\IKozyrev" "Pass4311" "ikozyrev\sharepoint"

WriteInfo "AddUserToMnagedAccount"
AddUserToMnagedAccount "DOMAIN\IKozyrev" "Pass4311"

WriteInfo "Install-SPHelpCollection"
Install-SPHelpCollection -All

WriteInfo "Initialize-SPResourceSecurity"
Initialize-SPResourceSecurity

WriteInfo "Install-SPService"
Install-SPService

#// todo: it better to desire only needed features
WriteInfo "Install-SPFeature"
Install-SPFeature -AllExistingFeatures

WriteInfo "New-SPCentralAdministration"
New-SPCentralAdministration -Port 2048

WriteInfo "Install-SPApplicationContent"
Install-SPApplicationContent

WriteInfo "CreateWebApplication"
CreateWebApplication "DOMAIN\IKozyrev"

WriteInfo "StartSPServices"
StartSPServices