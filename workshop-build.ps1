dotnet publish "$PSScriptRoot\cTabIRL\cTabExtension\cTabExtension.csproj" -c Release -r win-x64 -o "$PSScriptRoot\cTabIRL\@cTabIRL"

# Workshop release dot not have the git hash (git hash is only for Github releases)
(Get-Content "$PSScriptRoot\@cTab\.hemtt\project.toml") -replace 'git_hash = 4', 'git_hash = 0' | Set-Content "$PSScriptRoot\@cTab\.hemtt\project.toml"
(Get-Content "$PSScriptRoot\cTabIRL\@cTabIRL\.hemtt\project.toml") -replace 'git_hash = 4', 'git_hash = 0' | Set-Content "$PSScriptRoot\cTabIRL\@cTabIRL\.hemtt\project.toml"

cd "$PSScriptRoot\@cTab"
.\hemtt.exe release

cd "$PSScriptRoot\cTabIRL\@cTabIRL"
.\hemtt.exe release

# Restore the git hash
(Get-Content "$PSScriptRoot\@cTab\.hemtt\project.toml") -replace 'git_hash = 0', 'git_hash = 4' | Set-Content "$PSScriptRoot\@cTab\.hemtt\project.toml"
(Get-Content "$PSScriptRoot\cTabIRL\@cTabIRL\.hemtt\project.toml") -replace 'git_hash = 0', 'git_hash = 4' | Set-Content "$PSScriptRoot\cTabIRL\@cTabIRL\.hemtt\project.toml"

cd $PSScriptRoot