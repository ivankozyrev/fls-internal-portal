param([string]$port,[string]$db,[string]$cred)

. ".\helpers.ps1"

# default parameters START
$CENTRAL_ADMINISTRATION_PORT = 2048
$DATABASE_SERVER = "ikozyrev\sharepoint"
$CREDENTIALS_PATH = "..\..\deploymentAccount.creds"
# default parametrs END

if(![string]::IsNullOrEmpty($port))
{
    Write-Host "[ARGS] Input Argument 'port'=$port"
    $CENTRAL_ADMINISTRATION_PORT = $port
}

if(![string]::IsNullOrEmpty($db))
{
    Write-Host "[ARGS] Input Argument 'db'=$db"
    $DATABASE_SERVER = $db
}

if(![string]::IsNullOrEmpty($cred))
{
    Write-Host "[ARGS] Input Argument 'cred'=$cred"
    $CREDENTIALS_PATH = $cred
}

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

function GetCreadentials()
{
    if(Test-Path $CREDENTIALS_PATH)
    {
       WriteFuncInfo "Credentials will be taken from $CREDENTIALS_PATH."
       $fileLines = Get-Content $CREDENTIALS_PATH
       return CreatUserCredentials $fileLines[0] $fileLines[1]
    }
    else
    {
        WriteFuncInfo "You can specify yours credentials in '$CREDENTIALS_PATH'.First line is username (e.g. 'Domain\UserName'), second is yours password."
        return Get-Credential
    }
}

function CreatUserCredentials([string]$user,[string]$password)
{
    $secpasswd = ConvertTo-SecureString $password -AsPlainText -Force
    return New-Object System.Management.Automation.PSCredential ($user, $secpasswd)
}

function AddUserToMnagedAccount([System.Management.Automation.PSCredential]$credentials)
{
    $account = Get-SPManagedAccount $credentials.UserName
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

function CreatConfigurationDatabase([System.Management.Automation.PSCredential]$adminCredentials,[string]$dbServer)
{
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

$credentials = GetCreadentials

WriteInfo "CreatConfigurationDatabase"
CreatConfigurationDatabase $credentials $DATABASE_SERVER

WriteInfo "AddUserToMnagedAccount"
AddUserToMnagedAccount $credentials

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
New-SPCentralAdministration -Port $CENTRAL_ADMINISTRATION_PORT

WriteInfo "Install-SPApplicationContent"
Install-SPApplicationContent

WriteInfo "CreateWebApplication"
CreateWebApplication $credentials.UserName

WriteInfo "StartSPServices"
StartSPServices