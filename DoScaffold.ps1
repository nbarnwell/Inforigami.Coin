
$ErrorActionPreference = 'Stop'

function Set-Encoding {
    [CmdletBinding(SupportsShouldProcess=$true)]
    param(
        [Parameter(Mandatory=$true)]
        [string] $Path
    )
    $files = @(
        'create.cshtml','create.cshtml.cs',
        'delete.cshtml','delete.cshtml.cs',
        'details.cshtml','details.cshtml.cs',
        'edit.cshtml','edit.cshtml.cs',
        'index.cshtml','index.cshtml.cs'
    );

    gci -Path $Path -r -inc $files |
        %{
            (Get-Content $_) |
                Out-File $_ -Force -Encoding utf8
        }
}

function Get-PluralName {
    param([string] $SingularName)

    if ($SingularName -eq 'Person') {
        'People'
    } elseif ($SingularName -match 'is$') {
        $SingularName -replace 'is$','es'
    } elseif ($SingularName -match 'ion$') {
        "$($SingularName)s"
    } elseif ($SingularName -match 'on$') {
        $SingularName -replace 'on$','a'
    } elseif ($SingularName -match 'us$') {
        $SingularName -replace 'us$','uses'
    } elseif ($SingularName -match 'o$') {
        $SingularName -replace 'o$','oes'
    } elseif ($SingularName -match '[^aeiou]y$') {
        $SingularName -replace 'y$','ies'
    } elseif ($SingularName -match '(s|ss|sh|ch|x|z)$') {
        "$($SingularName)es"
    } else {
        "$($SingularName)s"
    }
}

function Invoke-CodeGeneration {
    [CmdletBinding(SupportsShouldProcess=$true)]
    param(
        [Parameter(Mandatory=$true)]
        [string] $EntityName,
        [string] $AreaName,
        [string] $projectName = 'Coin.Web'
    )
    $outputFolder = "Pages\$(Get-PluralName $EntityName)"

    if (!!$AreaName) {
        $outputFolder = "Areas\$AreaName\$outputFolder"
    }
    
    $fullyQualifiedOutputFolder = Join-Path (Join-Path '.' $projectName) $outputFolder
    
    Write-Host "Generating code for $EntityName to $fullyQualifiedOutputFolder"

    if ($PSCmdlet.ShouldProcess("dotnet aspnet-codegenerator razorpage -p $projectName -m $EntityName -dc CoinContext -udl -outDir $outputFolder --referenceScriptLibraries --force --no-build")) {
        dotnet aspnet-codegenerator razorpage -p Coin.Web -m $EntityName -dc CoinContext -udl -outDir $outputFolder --referenceScriptLibraries --force --no-build
    }

    Set-Encoding $fullyQualifiedOutputFolder
}

Invoke-CodeGeneration 'Account' 'Accounting'
Invoke-CodeGeneration 'AccountStatement' 'Accounting'
Invoke-CodeGeneration 'AccountTransactionCategory' 'Accounting'
Invoke-CodeGeneration 'AccountTransaction' 'Accounting'
Invoke-CodeGeneration 'AccountTransactionStatus' 'Accounting'
Invoke-CodeGeneration 'AccountTransactionType' 'Accounting'
Invoke-CodeGeneration 'BankAccount' 'Accounting'
Invoke-CodeGeneration 'Bank' 'Accounting'
Invoke-CodeGeneration 'BankSpecificTransactionType' 'Accounting'
Invoke-CodeGeneration 'Budget' 'Accounting'
Invoke-CodeGeneration 'BudgetItem' 'Accounting'
Invoke-CodeGeneration 'Currency' 'Accounting'

Invoke-CodeGeneration 'VehicleMileageLog' 'Vehicles'
Invoke-CodeGeneration 'VehiclePart' 'Vehicles'
Invoke-CodeGeneration 'Vehicle' 'Vehicles'
Invoke-CodeGeneration 'VehicleType' 'Vehicles'

Invoke-CodeGeneration 'Household'
Invoke-CodeGeneration 'Person'