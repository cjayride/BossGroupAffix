# Thunderstore zip: DLL + manifest + docs (config is created on first run only).
param(
    [string] $Version = "1.0.5"
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$pkg = Join-Path $root "Package"
$dist = Join-Path $root "dist"
$zip = Join-Path $dist "cjayride-BossAffixByPlayers-$Version.zip"
$stage = Join-Path $dist "package"

dotnet build (Join-Path $root "Cjayride.BossGroupAffix.csproj") -c Release
if ($LASTEXITCODE -ne 0) { throw "Build failed" }

$dll = Join-Path $root "bin\Release\net481\Cjayride.BossGroupAffix.dll"
New-Item -ItemType Directory -Force -Path $dist | Out-Null
Remove-Item $stage -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path (Join-Path $stage "BepInEx\plugins") | Out-Null

Copy-Item $dll (Join-Path $dist "Cjayride.BossGroupAffix.dll")
Copy-Item $dll (Join-Path $stage "BepInEx\plugins\Cjayride.BossGroupAffix.dll")

$licenseRoot = Join-Path $root "LICENSE"
$licensePkg = Join-Path $pkg "LICENSE"
if (-not (Test-Path $licensePkg) -and (Test-Path $licenseRoot)) {
    Copy-Item $licenseRoot $licensePkg
}

foreach ($f in @("manifest.json", "README.md", "CHANGELOG.md", "LICENSE", "icon.png")) {
    $src = Join-Path $pkg $f
    if (Test-Path $src) {
        Copy-Item $src (Join-Path $dist $f)
        Copy-Item $src (Join-Path $stage $f)
    }
}

Remove-Item $zip -Force -ErrorAction SilentlyContinue
Compress-Archive -Path (Join-Path $stage "*") -DestinationPath $zip
Write-Host $dll
Write-Host $zip
