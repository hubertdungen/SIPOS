param(
    [string]$Version = "Beta-1.5.2",
    [string]$Runtime = "win-x64"
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$publishProfile = "$Runtime-portable"
$publishDir = Join-Path $repoRoot "bin\Release\net10.0-windows\$Runtime\publish\portable"
$artifactDir = Join-Path $repoRoot "artifacts"
$zipPath = Join-Path $artifactDir "SIPOS-$Version-$Runtime-portable.zip"
$checksumPath = "$zipPath.sha256"

Push-Location $repoRoot
try {
    dotnet publish "SIPOS.csproj" "/p:PublishProfile=$publishProfile"
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish failed with exit code $LASTEXITCODE."
    }

    New-Item -ItemType Directory -Path $artifactDir -Force | Out-Null
    if (Test-Path $zipPath) {
        Remove-Item $zipPath -Force
    }

    Compress-Archive -Path (Join-Path $publishDir "*") -DestinationPath $zipPath -Force
    $hash = Get-FileHash -Algorithm SHA256 -Path $zipPath
    "$($hash.Hash)  $(Split-Path -Leaf $zipPath)" | Set-Content -Path $checksumPath -Encoding ASCII

    Write-Host "Created $zipPath"
    Write-Host "Created $checksumPath"
}
finally {
    Pop-Location
}
