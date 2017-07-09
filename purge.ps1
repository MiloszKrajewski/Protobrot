function purge([string] $name) {
	Get-ChildItem -Include $name -Recurse | ForEach-Object {
		Write-Host -ForegroundColor Green "Deleting: $_"
		Remove-Item -Recurse -Force $_
	}
}

purge "packages"
purge "node_modules"
purge "bin"
purge "obj"
purge ".paket"
