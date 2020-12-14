$packageName = "mdoc"
$packageOwner = "Microsoft"
$nugetConfigPath = "mdoc/mdoc.nuspec"

$nugetSearchResult = nuget list PackageId:$packageName Owner:$packageOwner
$packageSearchResult = $nugetSearchResult.Split(' ')
if ($packageSearchResult.Count -ne 2)
{
	Write-Host "Searching $packageName on nuget.org returns an invalid result: $nugetSearchResult"
	return
}

$remoteLatestVersion = $packageSearchResult[1]
Write-Host "The lastest version of $packageName on nuget.org is: $remoteLatestVersion"
if (!$remoteLatestVersion)
{
	Write-Host "Current lastest version of $packageName on nuget.org is an invalid value."
	return
}

[xml]$localNugetConfig = Get-Content($nugetConfigPath)
$localVersion = $localNugetConfig.package.metadata.version
Write-Host "The local version of $packageName in package metadata file is: $localVersion"
if (!$localVersion)
{
	Write-Host "Current local version of $packageName in package metadata file is an invalid value."
	return
}

$needUpdate = $false
if ($localVersion -gt $remoteLatestVersion)
{
	$needUpdate = $true
	Write-Host "We need to publish a new version to nuget.org."
}

Write-Host "##vso[task.setvariable variable=NeedUpdate;isOutput=true]$needUpdate"
Write-Host "##vso[task.setvariable variable=Version;isOutput=true]$localVersion"