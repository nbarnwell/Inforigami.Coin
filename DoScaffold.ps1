cd C:\Users\Neil\Documents\GitHub\Inforigami.Coin\Coin.Web

function Set-Encoding {
    $files = @(
        'create.cshtml','create.cshtml.cs',
        'delete.cshtml','delete.cshtml.cs',
        'details.cshtml','details.cshtml.cs',
        'edit.cshtml','edit.cshtml.cs',
        'index.cshtml','index.cshtml.cs'
    );

    gci -r -inc $files |
        %{
            (Get-Content $_) |
                Out-File $_ -Force -Encoding utf8
        }
}

function Get-PluralName {
    param([string] $SingularName)

    if ($SingularName -eq 'Person') {
        'People'
    } elsif ($SingularName -match 'is$') {
        $SingularName -replace 'is$','es'
    } elsif ($SingularName -match 'on$') {
        $SingularName -replace 'on$','a'
    } elsif ($SingularName -match 'us$') {
        $SingularName -replace 'us$','uses'
    } elsif ($SingularName -match 'o$') {
        $SingularName -replace 'o$','oes'
    } elsif ($SingularName -match '[^aeiou]y$') {
        $SingularName -replace 'y$','ies'
    } elsif ($SingularName -match 's|ss|sh|ch|x|z$') {
        "$($SingularName)es"
    } else {
        "$($SingularName)s"
    }
}

function Invoke-CodeGeneration {
    [CmdletBinding(SupportShouldProcess=$true)]
    param(
        [Parameter(Mandatory=$true)]
        [string] $EntityName,
        [string] $AreaName)
    $outputFolder = "pages\$(Get-PluralName $EntityName)"

    if (!!$AreaName) {
        $outputFolder = "Areas\$AreaName\$outputFolder"
    }

    if ($PSCmdlet.ShouldProcess("dotnet aspnet-codegenerator razorpage -p Coin.Web -m $EntityName -dc CoinContext -udl -outDir $outputFolder --referenceScriptLibraries --force --no-build")) {
        dotnet aspnet-codegenerator razorpage -p Coin.Web -m $EntityName -dc CoinContext -udl -outDir $outputFolder --referenceScriptLibraries --force --no-build
    }
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
Invoke-CodeGeneration 'Currency' 'Accounting'

Invoke-CodeGeneration 'VehicleMileageLog' 'Vehicles'
Invoke-CodeGeneration 'VehiclePart' 'Vehicles'
Invoke-CodeGeneration 'Vehicle' 'Vehicles'
Invoke-CodeGeneration 'VehicleType' 'Vehicles'

Invoke-CodeGeneration 'Household'
Invoke-CodeGeneration 'Person'