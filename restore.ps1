function Invoke-InFolder([string] $folder, [scriptblock] $code) {
    Push-Location $folder
    Invoke-Command $code
    Pop-Location
}

& ./paket "restore"
& ./paket "install"
Invoke-InFolder "Protobrot.Api" { & yarn }
& dotnet "restore"