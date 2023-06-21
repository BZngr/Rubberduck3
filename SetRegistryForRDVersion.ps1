#Requires -RunAsAdministrator

[cmdletbinding(SupportsShouldProcess = $True)]
param (
    [Parameter(Mandatory = $True)]
    [ValidateSet("CurrentVersion", "rd2", "rd3", "2", "3")] 
    [string]$requestedOp
)

################################ Script starts at line 562 ################################


$HKLMClsidPath = "Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\"
$HKCRClsidPath = "Registry::HKEY_CLASSES_ROOT\CLSID\"

$extensionClsid = "69E0F697-43F0-3B33-B105-9B8188A6F040"
$dockableToolWindowClsid = "69E0F699-43F0-3B33-B105-9B8188A6F040"

$script:rd3Version = "TBD"
$script:rd2Version = "TBD"
$script:rdActiveVersion = "TBD"

$script:rdHKCRKeys = @()

#There are currently(4/23) 2 registry keys to be added for RD3: 
#Rubberduck.Extension and Rubberduck.DockableToolWindow
$numberOfRD3Keys = 2

$rdExtension = @{
    HKLMPath = "${HKLMClsidPath}{${extensionClsid}}"
    ShortHKLMPath = "HKLM:\SOFTWARE\Classes\CLSID\{<Extension>}"
    HKCRPath = "${HKCRClsidPath}{${extensionClsid}}"
    ShortHKCRPath = "HKCR:\CLSID\{<Extension>}"
    InProcServer32Path = "Registry::HKEY_CLASSES_ROOT\CLSID\{${extensionClsid}}\InProcServer32"
}

$rdDockableToolWindow = @{
    HKLMPath = "${HKLMClsidPath}{${dockableToolWindowClsid}}"
    ShortHKLMPath = "HKLM:\SOFTWARE\Classes\CLSID\{<DockableToolWindow>}"
    HKCRPath = "${HKCRClsidPath}{${dockableToolWindowClsid}}"
    ShortHKCRPath = "HKCR:\CLSID\{<DockableToolWindow>}"
    InProcServer32Path = "Registry::HKEY_CLASSES_ROOT\CLSID\{${dockableToolWindowClsid}}\InProcServer32"
}

$rdRegistryKeyPropertyNames = @("Assembly", "Class", "CodeBase", "RuntimeVersion")

$rd3HKLMDataPersistancePath = (Get-ChildItem env:USERPROFILE).Value + `
    "\AppData\Roaming\Rubberduck\RD3HKLMValues.json"


function Test-CanProcessToTargetVersion($targetIsRD2){

    Write-Verbose "Evaluating Registry content..."

    $result = @{CanProcess = $False}

    if ((Test-IsConfiguredForRD2) -and $targetIsRD2){
        $result.Message = "Registry is configured for Rubberduck Version ${script:rdActiveVersion} - Nothing to do"
        $result
        return
    }
    
    if ((Test-IsConfiguredForRD3) -and (-not $targetIsRD2)){
        $result.Message = "Registry is configured for Rubberduck Version ${script:rdActiveVersion} - Nothing to do"
        $result
        return
    }
    
    if (-not $targetIsRD2){

        $valuesFromFileFailureMsg = "Prior RD3 values unavailable. Please build the Rubberduck3 solution to setup the registry"
        if (-not (Test-Path (Get-CachedRD3KeyValuesFilepath))){
            $result.Message = $valuesFromFileFailureMsg
            $result
            return
        }

        $regKeyModelsFromFile = New-RegistryKeyModelsFromFile

        if ($null -eq $regKeyModelsFromFile){
            $result.Message = $valuesFromFileFailureMsg
            $result
            return
        }

        if ($regKeyModelsFromFile.Count -ne $numberOfRD3Keys){
            $result.Message = $valuesFromFileFailureMsg
            $result
            return
        }

    } else {
        foreach ($prototypeKey in Get-RD2PrototypeKeys){
            if (-not (Test-Path $prototypeKey)){
                $result.Message = "RD2 Registry Key(s) not found - script expects to discover existing registry entries for RD2"
                $result
                return
            }
        }   
    }

    $result = @{
        "CanProcess" = $True
        "ModelsFromFile" = $regKeyModelsFromFile
    }
    $result
}

function Restore-RubberduckV2($rdHKLMClsidKeys){
    Write-Verbose "Restoring registry values to support Rubberduck Version 2"

    $registryKeyModels = Get-RegistryKeyModels $rdHKLMClsidKeys

    if (-not (Test-FileIsLocked (Get-CachedRD3KeyValuesFilepath)) ){
        Export-RD3RegistryValues $registryKeyModels
    }
        
    Remove-RD3RegistryKeys $rdHKLMClsidKeys

    Set-RD2InProcServer32Values

    "Registry keys set to support Rubberduck Version ${script:rd2Version}"
}

function Restore-RubberduckV3($regKeyModels){

    Write-Verbose "Restoring registry values to support Rubberduck Version ${script:rd3Version}"

    Set-Rd3Version $regKeyModels
    
    Add-RD3RegistryKeys $regKeyModels

    Set-RD3InProcServer32Values $regKeyModels

    "Registry keys set to support Rubberduck Version ${script:rd3Version}"
}

function Set-Rd3Version($regKeyModels){
    if ($regKeyModels.Count -lt 1){
        return
    }

    $assemblyValue = $regKeyModels[0].Properties.Assembly
    $idxOf3 = $assemblyValue.IndexOf("3")

    $script:rd3Version = $assemblyValue.Substring(`
        $idxOf3, $assemblyValue.IndexOf(",", $idxOf3) - $idxOf3)
}

function Add-RD3RegistryKeys($regKeyModels){

    foreach ($regModel in $regKeyModels){        
        if (Test-ContainsExtensionClsid $regModel.KeyPath){
            $shortPath = $rdExtension.ShortHKLMPath
        } else {
            $shortPath =  $rdDockableToolWindow.ShortHKLMPath
        }

        Write-Verbose ("Restore RD3 values to Key ${shortPath}\InProcServer32")

        Add-RegistryKeys (Get-Rd3HKLMPaths $regModel.KeyPath)

        $rdRegistryKeyPropertyNames | 
            ForEach-Object { Set-RegistryKeyProperties $regModel $_ }    
    }
}   

function Get-ActiveRDVersion($rdHKCRKeys){
    
    $result = "TBD"
    
    if ($rdHKCRKeys.Count -eq 2){ 
        if ($rdHKCRKeys[0] -like "2.*"){
            $script:rd2Version = $rdHKCRKeys[0]
            $script:rd3Version = $rdHKCRKeys[1]
        } else {
            $script:rd3Version = $rdHKCRKeys[0]
            $script:rd2Version = $rdHKCRKeys[1]
        }
        $result = $script:rd3Version
    }
    #Handles scenario where only Rd2 OR Rd3 is registered on the machine
    elseif (-not $rdHKCRKeys.Count -eq 0){
        if ($rdHKCRKeys -like "2.*"){
            $script:rd2Version = $rdHKCRKeys
            $result = $script:rd2Version
        } elseif ($rdHKCRKeys -like "3.*"){
            $script:rd3Version = $rdHKCRKeys
            $result = $script:rd3Version
        }
    }
    $result
}

function Get-CachedRD3KeyValuesFilepath($filepath = $null){

    $result = $rd3HKLMDataPersistancePath
    if (-not $null -eq $filepath){
        $result = $filePath
    }

    $result
}

function Get-HKLMKeysOfInterest($keyNames){

    if ($keyNames.Count -eq 0){
        @()
        return
    }

    $result = $keyNames | Where-Object { Test-IsRDClsidOfInterest $_ } | 
        ForEach-Object {"${HKLMClsidPath}${_}"}

    if ($null -eq $result){
        $result = @()
    }

    $result
}

function Set-RD2InProcServer32Values(){
    [cmdletbinding(SupportsShouldProcess = $True)]
    param ()

    foreach ($key in Get-RD2PrototypeKeys){

        $InProcServer32Key = Get-InProcServer32PathFromKey $key

        if (Test-ContainsExtensionClsid $key){
            Write-Verbose ("Restore RD2 values to Key " + $rdExtension.ShortHKCRPath + "\InProcServer32")
        } else {
            Write-Verbose ("Restore RD2 values to Key " + $rdDockableToolWindow.ShortHKCRPath + "\InProcServer32")
        }

        if ($PSCmdlet.ShouldProcess($InProcServer32Key)){} 

        $model = New-RegistryKeyModelFromRegistryKey $key

        $model.KeyPath = $InProcServer32Key
        
        $rdRegistryKeyPropertyNames | ForEach-Object { 
            Set-RegistryKeyProperties $model $_        
        } 
    }
}

function Set-RD3InProcServer32Values(){
    [cmdletbinding(SupportsShouldProcess = $True)]
    param (
        [Parameter (Mandatory = $true)]
        [Object[]] $regKeyModels
    )

    foreach ($model in $regKeyModels){

        if (Test-ContainsExtensionClsid $model.KeyPath){
            $shortPath = $rdExtension.ShortHKCRPath
            $keyPath = $rdExtension.HKCRPath
        } else {
            $shortPath = $rdDockableToolWindow.ShortHKCRPath
            $keyPath = $rdDockableToolWindow.HKCRPath
        }

        $InProcServer32Key = $keyPath + "\InProcServer32"
        Write-Verbose ("Restore RD3 values to Key ${shortPath}\InProcServer32")

        if ($PSCmdlet.ShouldProcess($InProcServer32Key)){} 

        $clone = (New-RdRegistryKeyModel $model.KeyPath $model.Properties)
        $clone.KeyPath = $InProcServer32Key
        
        $rdRegistryKeyPropertyNames | ForEach-Object { 
            Set-RegistryKeyProperties $clone $_        
        } 
    }
}

function Get-RD2PrototypeKeys(){
    @(
        $rdExtension.InProcServer32Path + "\${script:rd2Version}"
        $rdDockableToolWindow.InProcServer32Path + "\${script:rd2Version}"
    )
}


function Get-RegistryKeyModels($rdHKLMKeys){
    
    $registryKeyModels = @()
    foreach ($k in $rdHKLMKeys){
        $registryKeyModels += `
            (New-RegistryKeyModelFromRegistryKey `
                ($k + "\InProcServer32\" + $script:rd3Version))
    }

    $registryKeyModels
}

function Remove-RD3RegistryKeys($regKeys){
    Write-Verbose "Removing RD3 HKey_LOCAL_MACHINE registry keys..."
    Remove-RegistryKeys $regKeys
}

function Export-RD3RegistryValues($registryKeyModels, $exportPath = $null){

    Write-Verbose "Saving current RD3 HKey_LOCAL_MACHINE registry values..."
    
    $filepath = Get-CachedRD3KeyValuesFilepath $exportPath

    ($registryKeyModels | ConvertTo-Json) | Out-File  $filepath
}

function Get-Rd3HKLMPaths($targetKey, $scriptVersion = $null){
    
    $versionToken = $script:rd3Version
    if ($null -ne $scriptVersion){
        $versionToken = $scriptVersion
    }

    $baseTargetKey = $rdExtension.HKLMPath
    if (Test-ContainsDockableToolWindowClsid $targetKey){
        $baseTargetKey = $rdDockableToolWindow.HKLMPath
    }

    @(
        $baseTargetKey
        ($baseTargetKey + "\InProcServer32")
        ($baseTargetKey + "\InProcServer32\" + $versionToken)
    )
}

function New-RdRegistryKeyModel($registryKey = $null, $properties = @{}){
    @{
        "KeyPath" = $registryKey
        "IsRDRegistryKeyModel" = $True
        'Properties' = $properties
    }
}

function New-RegistryKeyModelFromRegistryKey($registryKey){

    $properties = @{}
    if (Test-Path $registryKey){
        $rdRegistryKeyPropertyNames | ForEach-Object {
            $properties.Add($_, (Get-RegistryPropertyValue $registryKey $_))}
    }

    New-RdRegistryKeyModel $registryKey $properties
}

function New-RegistryKeyModelsFromFile($filePath = $null){
    
    $results = $null
    $modelsFilepath = Get-CachedRD3KeyValuesFilepath $filepath

    $content = Get-Content $modelsFilepath
    if ($null -eq $content -or ($content.Trim() -eq "")){
        $results
        return
    }
    
    try {
        $jsonObjs = $content | ConvertFrom-Json
    }
    catch {
        $results
        return
    }

    $results = @()
    foreach ($obj in $jsonObjs){
        $model = (Convert-JsonToRegistryKeyModel $obj)
        if ($null -eq $model){
            $results = $null
            $results
            return
        } else {
            $results += $model
        }
    }
    $results
}

function Convert-JsonToRegistryKeyModel($jsonObject){
    #https://stackoverflow.com/questions/3740128/pscustomobject-to-hashtable
    $props = @{}
    $jsonObject.Properties.psobject.properties | ForEach-Object { $props[$_.Name] = $_.Value }  
    
    $model = New-RdRegistryKeyModel $jsonObject.KeyPath $props

    if (Test-IsValidRegistryKeyModel $model){
        $model 
        return
    }
    $null
}

function Get-InProcServer32PathFromKey($key){
    $target = "InProcServer32"
    $key.Substring(0, $key.IndexOf($target) + $target.Length)
}

function Get-Rd3VersionFromRd3RegistryKey($key){
    $target = "InProcServer32\3."
    $key.Substring($key.IndexOf($target) + ("InProcServer32\").Length)
}


function Test-IsValidRegistryKeyModel($model){
 
    if ($null -eq $model){
        $False
        return
    } elseif  (-not ((Test-HasAllKeys $model) -and (Test-AllKeysNotNull $model))){
        $False
        return
    } elseif (-not ((Test-HasAllProperties $model) -and (Test-AllPropertiesNotNull $model))){
        $False
        return
    }

    $True
 }
 
function Test-ContainsExtensionClsid($token){
    ($token -like "*${extensionClsid}*")
}

function Test-ContainsDockableToolWindowClsid($token){
    ($token -like "*${dockableToolWindowClsid}*")
}

function Test-IsRDClsidOfInterest($clsid){
    (Test-ContainsExtensionClsid $clsid) -or `
        (Test-ContainsDockableToolWindowClsid $clsid)
}

function Test-HasAllKeys($model){
    (@("KeyPath", "Properties") | 
        Where-Object {-not $model.ContainsKey($_)}).Count -eq 0       
}

function Test-AllKeysNotNull($model){
    (@("KeyPath", "Properties") | 
        Where-Object {$null -eq $model[$_]}).Count -eq 0
}

function Test-HasAllProperties($model){
    ($rdRegistryKeyPropertyNames | 
        Where-Object {-not $model.Properties.ContainsKey($_)}).Count -eq 0       
}

function Test-AllPropertiesNotNull($model){
    ($rdRegistryKeyPropertyNames | 
        Where-Object {$null -eq $model.Properties[$_]}).Count -eq 0       
}

function Test-IsConfiguredForRD2(){
    if ($script:rdActiveVersion -eq "TBD"){
        $script:rdActiveVersion = Get-ActiveRDVersion $script:rdHKCRKeys
    }
    $script:rdActiveVersion.StartsWith("2.")
}

function Test-IsConfiguredForRD3(){
    if ($script:rdActiveVersion -eq "TBD"){
        $script:rdActiveVersion = Get-ActiveRDVersion $script:rdHKCRKeys
    }
    $script:rdActiveVersion.StartsWith("3.")
}

function Test-FileIsLocked($file){
    #https://stackoverflow.com/questions/24992681/powershell-check-if-a-file-is-locked
    try { 
        [IO.File]::OpenWrite($file).close()
        $false 
    }
    catch {
        $true
    }    
}

function Get-SubKeys($hiveName, $subKeyPath){
    $hiveKeys = @()
    try 
    {
        $reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey(`
            [Microsoft.Win32.RegistryHive]($hiveName), $env:COMPUTERNAME)

        $subKey = $reg.OpenSubKey($subKeyPath)

        $subKeyNames = $subKey | 
            Select-Object Name, @{
                label='SubKeys'
                expression={$_.GetSubKeyNames()}
            }
            
        $hiveKeys = $subKeyNames.SubKeys
    }
    catch {
        $hiveKeys = @()
    }
    finally {
        $subKey.Dispose()
    }

    $hiveKeys
}

function Remove-RegistryKeys(){
    [cmdletbinding(SupportsShouldProcess = $True)]
    param (
        [Parameter(Mandatory = $True)]
        [string[]]$keyPaths
    )

    foreach ($key in $keyPaths | Where-Object {Test-Path $_}){ 
        if ($PSCmdlet.ShouldProcess($key, "Remove-Item")){
            Write-Verbose ("Deleting Registry Key: " + $key)
            Remove-Item -Path $key -Recurse
        }
    }      
}
 
function Add-RegistryKeys(){
    [cmdletbinding(SupportsShouldProcess = $True)]
    param (
        [Parameter (Mandatory = $true)]
        [Object] $pathsToCreate
    )

    foreach ($kp in $pathsToCreate){
        if ($PSCmdlet.ShouldProcess($kp, "New-Item")){
            New-Item -Path $kp
            Write-Verbose ("Added Registry Key: " + $kp)
        }
    }
}

function Set-RegistryKeyProperties(){
    [cmdletbinding(SupportsShouldProcess = $True)]
    param (
        [Parameter (Mandatory = $true)]
        [Object] $registryKeyModel,
        [Parameter (Mandatory = $true)]
        [string] $valueName
    )

    $valueData = $registryKeyModel.Properties[$valueName]

    $shouldProcessMsg = "'" + $valueName + "' = '" + $valueData + "'"
    
    if ($PSCmdlet.ShouldProcess($shouldProcessMsg, "Set-ItemProperty")){
        Set-ItemProperty -Path $registryKeyModel.KeyPath -Name $valueName -Value $valueData
        Write-Verbose ("Set Property: '" + $shouldProcessMsg + "'")
    }
}

function Get-RegistryPropertyValue($registryKey, $propertyName){
    (Get-ItemPropertyValue -Path $registryKey -Name $propertyName)
}
################################ Start of Script ################################

$script:rdHKCRKeys = Get-SubKeys "ClassesRoot" "CLSID\{${extensionClsid}}\InProcServer32"

$script:rdActiveVersion = Get-ActiveRDVersion $script:rdHKCRKeys

if ($requestedOp -eq "CurrentVersion"){
    
    if ($script:rdActiveVersion -eq "TBD"){
        "Unknown: Rubberduck2 and/or Rubberduck3 may not be installed"
    } else {
        "Current Registry configuration: Rubberduck Version ${script:rdActiveVersion}"
    }
    exit
}

$targetIsRD2 = ($requestedOp -like "*2")

$validationResults = Test-CanProcessToTargetVersion $targetIsRD2

if (-not $validationResults.CanProcess){
    $validationResults.Message
    exit
}

if ($targetIsRD2){
    $rd3HKLMKeys = Get-HKLMKeysOfInterest( Get-SubKeys "LocalMachine" "SOFTWARE\Classes\CLSID")
    Restore-RubberduckV2 $rd3HKLMKeys

} else {
    if ($script:rd3Version -eq "TBD"){
        $keyPaths = $validationResults.ModelsFromFile.KeyPath
        $script:rd3Version = Get-Rd3VersionFromRd3RegistryKey($keyPaths[0])
    }
    Restore-RubberduckV3 $validationResults.ModelsFromFile
}

<#
.SYNOPSIS
Modifies the registry to enable either the Rubberduck 3 or the Rubberduck 2 VBIDE Add-In.
.DESCRIPTION
Modifies the registry to enable either the Rubberduck 3 or the Rubberduck 2 VBIDE Add-In.

Why: 
RD3 re-uses RD2 CLSIDs.  Consequently, the two Add-Ins cannot co-exist in memory.  
When the RD3 solution is built, the build process introduces new keys to the LocalMachine (HKLM) hive.  
Re-building RD2 does not configure the registry to load RD2 once the HKLM keys have been introduced.

*** The script must be invoked from a PowerShell session with Administrator privileges ***

To run the script to see what it does WITHOUT making changes, enter the following:
(The commands below assume a Powershell session is opened in the folder containing the SetRegistryForRDVersion.ps1 script)

Setup for RD2:    PS> .\SetRegistryForRDVersion.ps1 2 -Verbose -WhatIf
Setup for RD3:    PS> .\SetRegistryForRDVersion.ps1 3 -Verbose -WhatIf

****************************************************************************************
Note: Absence of the -WhatIf switch parameter from the above expressions will enable the 
script to make changes to the registry.
*****************************************************************************************

To see the currently active version: 
PS> .\SetRegistryForRDVersion.ps1 CurrentVersion

Alternatively, re-building the RD3 solution will also modify the Registry, enabling the RD3 Add-In  

There is more content available using the -examples, -detailed, or -full switches as indicated in REMARKS
.NOTES
Setting up for Rubberduck Version 2:

1. Deletes two HKLM registry keys (including their children) introduced by building RD3: 
    HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{69E0F697-43F0-3B33-B105-9B8188A6F040} (Rubberduck.Extension)
    HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{69E0F699-43F0-3B33-B105-9B8188A6F040} (Rubberuck.DockableToolWindow)
    Note: before deleting, the script saves values contained in these keys to a file (RD3HKLMValues.json) located in 
        the same folder as the Rubberduck.config file.

2. Modifies the values of:
    HKEY_CLASSES_ROOT\CLSID\{69E0F697-43F0-3B33-B105-9B8188A6F040}\InProcServer32 and
    HKEY_CLASSES_ROOT\CLSID\{69E0F699-43F0-3B33-B105-9B8188A6F040}\InProcServer32

    to match the values contained in 

    HKEY_CLASSES_ROOT\CLSID\{69E0F697-43F0-3B33-B105-9B8188A6F040}\InProcServer32\2.X.Y.Z and 
    HKEY_CLASSES_ROOT\CLSID\{69E0F699-43F0-3B33-B105-9B8188A6F040}\InProcServer32\2.X.Y.Z respectively

Setting up for Rubberduck Version 3:

1. Re-creates the RD3 HKLM registry keys and their children based on data contained in 
    RD3HKLMValues.json.  The RD3HKLMValues.json file is generated/refreshed when the script is run 
    to update from RD3 to RD2.

2. Modifies the following registry keys with RD3 values

    HKEY_CLASSES_ROOT\CLSID\{69E0F697-43F0-3B33-B105-9B8188A6F040}\InProcServer32 and
    HKEY_CLASSES_ROOT\CLSID\{69E0F699-43F0-3B33-B105-9B8188A6F040}\InProcServer32


3. If the RD3HKLMValues.json file is missing or cannot be used for any reason, the user is prompted to 
rebuild the Rubbberduck3 solution.  Rebuilding the solution results in the same registry changes
as described in steps 1 and 2 above. 

Note: Closing and re-opening the Registry Editor application will show that adding the two HKLM keys 
in #1 above, resulted in the creation of:
    HKEY_CLASSES_ROOT\CLSID\{69E0F697-43F0-3B33-B105-9B8188A6F040}\InProcServer32\3.X.Y.Z and 
    HKEY_CLASSES_ROOT\CLSID\{69E0F699-43F0-3B33-B105-9B8188A6F040}\InProcServer32\3.X.Y.Z

.EXAMPLE
...\SetRegistryForRDVersion.ps1 CurrentVersion
Script output identifies the Rubberduck version currently setup in the registry

.EXAMPLE
...\SetRegistryForRDVersion.ps1 2 -WhatIf
See operations that 'would have' taken place to setup for RD2

.EXAMPLE
...\SetRegistryForRDVersion.ps1 2 -Verbose -WhatIf
See operations that 'would have' taken place to setup for RD2 with the most descriptive content

.EXAMPLE
...\SetRegistryForRDVersion.ps1 rd2 -Verbose
Allow the script to make changes to the registry to setup RD2, providing a narrative of the process

.EXAMPLE
.\SetRegistryForRDVersion.ps1 3 -Verbose -WhatIf
See operations that 'would have' taken place to setup for RD3 with the most descriptive content

.EXAMPLE
...\SetRegistryForRDVersion.ps1 rd3 -Verbose -WhatIf
See operations that 'would have' taken place to setup for RD3 with the most descriptive content

.EXAMPLE
...\SetRegistryForRDVersion.ps1 rd3
Allow the script to make changes to the registry to setup RD3 with limited descriptive content
#>

