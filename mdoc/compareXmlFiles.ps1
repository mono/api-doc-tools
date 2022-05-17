param (
    [string]$paramsJson,
    [string]$githubTokenBase64,
    [string]$githubOptionsAccountName,
    [string]$githubOptionsAccountEmail,
    [string]$vstsTokenBase64,
    [bool]$needRunReleaseMdoc,
    [string]$step
)

function Git-Init([string]$githubAccountName, [string]$githubAccountEmail)
{
	& git config --global --unset-all include.path
	& git config --global --unset-all credential.helper
	& git config --system --unset-all include.path
	& git config --system --unset-all credential.helper
	& git config --global credential.helper store
	& git config --global user.name $githubAccountName
    & git config --global user.email $githubAccountEmail
}

function Git-Clone([string]$repoUrl, [string]$repoPath, [string] $token, [string]$branch = "main") 
{
	Write-Host 'git -c http.extraHeader="Authorization: Basic '$token'" clone -b '$branch' '$repoUrl' '$repoPath' --depth 1 --shallow-submodules'
	& git -c http.extraHeader="Authorization: Basic $token" clone -b $branch $repoUrl $repoPath --depth 1 --shallow-submodules

	Push-Location $repoPath
	Write-Host | & git branch
	Pop-Location
}

function Git-Push([string]$rootPath, [string] $token, [string] $commitMessage, [string]$branch = "main") 
{
	Push-Location $rootPath

	$result = & git status
	if($result.Contains("nothing to commit"))
	{
		Write-Host "$result"
	}
	else
	{
		& git add --all
		& git commit -m $commitMessage
		
		& git config pull.rebase false
		Write-Host 'git -c http.extraHeader="Authorization: Basic '$token'" pull'
		& git -c http.extraHeader="Authorization: Basic $token" pull
		
		& git -c http.extraHeader="Authorization: Basic $token" push --set-upstream origin $branch --force-with-lease
	}
} 

function Run-Mdoc([string] $mdocPath, [string] $fwPath, [string] $xmlPath)
{
	Write-Host "$mdocPath fx-bootstrap $fwPath"
	& $mdocPath fx-bootstrap $fwPath

	$dnpath = [System.IO.Path]::GetDirectoryName((get-command dotnet).Source)
	$langs=@("VB.NET","F#","C++/CLI")
	$allArgs = @("update",
	 "-o", "$xmlPath",
	 "-fx", "$fwPath",
	 "-lang", "docid",
	 "-index", "false",
	 "--debug",
	 "--delete",
	 "-L", """C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\PublicAssemblies"""
	 "-L", """C:\Program Files (x86)\Microsoft.NET\Primary Interop Assemblies"""
	 "-L", """C:\Program Files\WindowsPowerShell\Modules\PackageManagement\1.0.0.1"""
	 "-L", """$dnpath""");
	if ($langs) {
		foreach ($lang in $langs) {
			$allArgs += "-lang"
			$allArgs += "$lang"
		}
	}
	Write-Host "& $mdocPath $allArgs"
	& $mdocPath $allArgs
}

# Clone binary repo, xml repo
# Generate xml file, push and log commit id
# Again to generate xml file, push and log commit id
# Compare two commits
function Run($source_repo,$target_repo,$origin_target_repo)
{
	if([String]::IsNullOrEmpty($source_repo.url)){
		Write-Host "source repo url is null or empty!"
	}
	if([String]::IsNullOrEmpty($source_repo.branch)){
		Write-Host "source repo branch is null or empty!"
	}
	if([String]::IsNullOrEmpty($source_repo.folder)){
		Write-Host "source repo folder is null or empty!"
	}
	if([String]::IsNullOrEmpty($target_repo.url)){
		Write-Host "target repo url is null or empty!"
	}
	if([String]::IsNullOrEmpty($target_repo.branch)){
		Write-Host "target repo branch is null or empty!"
	}
	if([String]::IsNullOrEmpty($target_repo.folder)){
		Write-Host "target repo folder is null or empty!"
	}
	if([String]::IsNullOrEmpty($origin_target_repo.url)){
		Write-Host "origin target repo url is null or empty!"
	}
	if([String]::IsNullOrEmpty($origin_target_repo.branch)){
		Write-Host "origin target repo branch is null or empty!"
	}
	if([String]::IsNullOrEmpty($origin_target_repo.folder)){
		Write-Host "origin target repo folder is null or empty!"
	}

	$sourceRepoUrl = $source_repo.url
	$sourceRepoBranch = $source_repo.branch
	$sourceFolder = $source_repo.folder
	$sourceRepoPath= $source_repo.repo_root
	$targetRepoUrl = $target_repo.url
	$targetRepoBranch = $target_repo.branch
	$targetfolder = $target_repo.folder
	$targetRepoPath= $target_repo.repo_root
	$originTargetRepoUrl = $origin_target_repo.url
	$originTargetRepoBranch = $origin_target_repo.branch
	$originTargetfolder = $origin_target_repo.folder
	$originTargetRepoPath= $origin_target_repo.repo_root

	$frameworksPath = Join-Path $sourceRepoPath $sourceFolder
	$originRepoXmlPath = Join-Path $originTargetRepoPath $originTargetfolder
	$xmlPath = Join-Path $targetRepoPath $targetfolder

	Write-Host "==================== Clone source repo: $sourceRepoUrl"
	if($sourceRepoUrl.Contains("github.com/")){
		Git-Clone $sourceRepoUrl $sourceRepoPath $githubTokenBase64 $sourceRepoBranch
	}
	else{
		Git-Clone $sourceRepoUrl $sourceRepoPath $vstsTokenBase64 $sourceRepoBranch
	}
	
	Write-Host "==================== Clone origin target repo: $originTargetRepoUrl"
	Git-Clone $originTargetRepoUrl $originTargetRepoPath $githubTokenBase64 $targetRepoBranch
	
	Write-Host "==================== Clone target repo: $targetRepoUrl"
	Git-Clone $targetRepoUrl $targetRepoPath $githubTokenBase64 $targetRepoBranch
	
	if (Test-Path $xmlPath) 
	{
		Write-Host "Delete files under path: $xmlPath"
		Remove-Item -Recurse -Force $xmlPath\*
		Write-Host "Delete files done."
	}
	Copy-Item "$originRepoXmlPath\*" -Destination "$xmlPath\" -Recurse -Force -Container
	
	# This part(if) run in Job_1
	if($step -eq "1"){
		if ($needRunReleaseMdoc -eq $true)
		{
			Write-Host "==================== Run Mdoc(release version) tool to generated xml files."
			Run-Mdoc $releaseMdocPath $frameworksPath $xmlPath
			if ($lastexitcode -ne 0)
			{
				exit $lastexitcode
			}
		}
		
		Write-Host "==================== First to commit xml files"
		$message = "CI Update 1 with build number " + $env:BUILD_BUILDNUMBER
		Git-Push $targetRepoPath $githubTokenBase64 $message $targetRepoBranch
		$commitid1 = & git rev-parse HEAD
		Write-Host "Commit Id1: $commitid1"
		Pop-Location
		
		Write-Host "##vso[task.setvariable variable=commit1;isOutput=true]$commitid1"
	} else { # This part(else) run in Job_2
	
		Write-Host "==================== Run Mdoc(pr version) tool to generated xml files."
		Run-Mdoc $prMdocPath $frameworksPath $xmlPath
		if ($lastexitcode -ne 0)
		{
			exit $lastexitcode
		}
		
		Write-Host "==================== Sencond to commit xml files"
		$message = "CI Update 2 with build number " + $env:BUILD_BUILDNUMBER
		Git-Push $targetRepoPath $githubTokenBase64 $message $targetRepoBranch
		$commitid2 = & git rev-parse HEAD
		Write-Host "Commit Id2: $commitid2"
		Pop-Location
		
		Write-Host "==================== Compare two version xml files."
		$commitid1 = $(commit1)    # commit1 from job_1
		$shortCommitId1 = $commitid1.Substring(0, 7)
		$shortCommitId2 = $commitid2.Substring(0, 7)
		if($targetRepoUrl.EndsWith(".git"))
		{
			$compareUrl = $targetRepoUrl.Substring(0, $ymlRepoUrl.Length - 4)
		}
		else
		{
			$compareUrl = $targetRepoUrl
		}
		
		$compareUrl = $compareUrl + "/compare/"
		$compareUrl = $compareUrl + "$shortCommitId1...$shortCommitId2/"
		
		Write-Host ("##vso[task.setvariable variable=CompareUrl;]$compareUrl")
		Write-Host "Compare Url: $compareUrl"
	}
}

$params = $paramsJson | ConvertFrom-Json
if([String]::IsNullOrEmpty($githubTokenBase64))
{
	Write-Host "githubTokenBase64 is null or empty!"
}
if([String]::IsNullOrEmpty($vstsTokenBase64))
{
	Write-Host "vstsTokenBase64 is null or empty!"
}

# Set download Paths
$repoRoot = $($MyInvocation.MyCommand.Definition) | Split-Path | Split-Path
$prMdocPath = "$repoRoot\bin\Release\mdoc.exe"

$parentRoot = $repoRoot | Split-Path
$binPath = Join-Path "$parentRoot\TestCI" "\_bin"
New-Item $binPath -Type Directory -Force

# Download nuget tool
$nugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$mdocPackageSource = "https://api.nuget.org/v3/index.json"
$nugetPath = Join-Path $binPath "\nuget.exe"
Invoke-WebRequest -Uri $nugetUrl -OutFile $nugetPath -Verbose

# Download mdoc package
Write-Host "==================== Download Mdoc tool"
$mdocPackageId = "mdoc"

if([String]::IsNullOrEmpty($params.mdoc_Version))
{
	$versionStr = & $nugetPath list $mdocPackageId -Source $mdocPackageSource
	if($versionStr -is [array])
	{
		$lastVersionStr = $versionStr[$versionStr.Count-1]
	}
	else
	{
		$lastVersionStr = $versionStr
	}
	Write-Host "$mdocPackageId last version string: $lastVersionStr"
	$lastVersion = $lastVersionStr.Split(" ")[1]
}
else
{
	$lastVersion = $params.mdoc_Version
}
Write-Host "$nugetPath install $mdocPackageId -Version $lastVersion -Source $mdocPackageSource -OutputDirectory $binPath"
& $nugetPath install $mdocPackageId -Version $lastVersion -Source $mdocPackageSource -OutputDirectory $binPath

$releaseMdocPath = Join-Path $binPath "mdoc.$lastVersion"
dir $releaseMdocPath
$releaseMdocPath = Join-Path $releaseMdocPath "tools\mdoc.exe"
Write-Host "Download $mdocPackageId to path: $releasemdocPath"


# Init git configure
Git-Init $githubOptionsAccountName $githubOptionsAccountEmail

# Generate ecma xml files
$params.source_repo.repo_root = Join-Path "$parentRoot\TestCI" $params.source_repo.repo_root
$params.target_repo.repo_root = Join-Path "$parentRoot\TestCI" $params.target_repo.repo_root
$params.origin_target_repo.repo_root = Join-Path "$parentRoot\TestCI" $params.origin_target_repo.repo_root
#Run $params.source_repo $params.target_repo $params.origin_target_repo

if($step -eq "1"){
        $commitid1 = "123456789"
	Write-Host "Commit Id1: "

	Write-Host "##vso[task.setvariable variable=commit1;isOutput=true]$commitid1"
} else { # This part(else) run in Job_2
	$commitid2 = "abcdefghigklmn"
	$commitid1 = $('commit1')    # commit1 from job_1
	$shortCommitId1 = $commitid1.Substring(0, 7)
	$shortCommitId2 = $commitid2.Substring(0, 7)
	if($targetRepoUrl.EndsWith(".git"))
	{
		$compareUrl = $targetRepoUrl.Substring(0, $ymlRepoUrl.Length - 4)
	}
	else
	{
		$compareUrl = $targetRepoUrl
	}

	$compareUrl = $compareUrl + "/compare/"
	$compareUrl = $compareUrl + "$shortCommitId1...$shortCommitId2/"

	Write-Host ("##vso[task.setvariable variable=CompareUrl;]$compareUrl")
	Write-Host "Compare Url: $compareUrl"
}
