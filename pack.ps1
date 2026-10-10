# Thunderstore zip: DLL + manifest + docs (config is created on first run only).
param(
    [string] $Version = "1.0.3"
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$out = Join-Path $root "Package"
$zip = Join-Path $out "BossAffixByPlayers-$Version.zip"
$stage = Join-Path $out "stage"

dotnet build (Join-Path $root "Cjayride.BossGroupAffix.csproj") -c Release
if ($LASTEXITCODE -ne 0) { throw "Build failed" }

Remove-Item $stage -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path (Join-Path $stage "BepInEx\plugins") | Out-Null

Copy-Item (Join-Path $root "bin\Release\net481\Cjayride.BossGroupAffix.dll") (Join-Path $stage "BepInEx\plugins\Cjayride.BossGroupAffix.dll")
foreach ($f in @("manifest.json", "README.md", "CHANGELOG.md", "LICENSE", "icon.png")) {
    $src = Join-Path $out $f
    if (Test-Path $src) {
        Copy-Item $src (Join-Path $stage $f)
    }
}

Remove-Item $zip -Force -ErrorAction SilentlyContinue
Compress-Archive -Path (Join-Path $stage "*") -DestinationPath $zip
Write-Host $zip
