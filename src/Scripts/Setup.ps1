$siteRoot = Resolve-Path "$PSScriptRoot\..\EPiServer.SocialAlloy.Web"
$setupFilesPattern = "$siteRoot\Setup\*"

Write-Host Starting setup for: $siteRoot
Write-Host Copying setup files from: $setupFilesPattern

Copy-Item $setupFilesPattern $siteRoot -recurse -force

if($?) {
	Write-Host Setup complete!
}
else {
	Write-Host Setup failed!
}