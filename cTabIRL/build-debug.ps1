dotnet publish cTabExtension\cTabExtension.csproj -r win-x64 -o ".\@cTabIRL"

cd "@cTabIRL"

.\hemtt.exe build

$OneDriveMods = "$Env:OneDrive\Documents\1erGTD\Mods"
if ( Test-Path "$OneDriveMods" -PathType Container ) {
    Write-Output "Copy to OneDrive"
	Copy-Item "$PSScriptRoot\@cTabIRL\*.cpp" -Destination "$OneDriveMods\@cTabIRLDebug\"
	Copy-Item "$PSScriptRoot\@cTabIRL\*.dll" -Destination "$OneDriveMods\@cTabIRLDebug\"
	Copy-Item "$PSScriptRoot\@cTabIRL\addons\*.pbo" -Destination "$OneDriveMods\@cTabIRLDebug\addons\"
}

cd ..