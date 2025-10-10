# mdoc Build System Modernization Summary

## Overview
This repository has been modernized to use modern .NET Core tooling with `dotnet pack` instead of legacy `nuget pack` and `.nuspec` files. The mdoc project can now be packaged as a .NET Global Tool.

## Changes Made

### 1. Updated `mdoc/mdoc.csproj`
- **Removed**: Custom `OutputPath` configurations that interfered with tool packaging
- **Removed**: `AppendTargetFrameworkToOutputPath` setting
- **Added**: Package metadata properties:
  - `PackageId`, `Version`, `Title`, `Authors`, `Owners`
  - `PackageProjectUrl`, `PackageLicenseExpression`, `PackageRequireLicenseAcceptance`
  - `Description`, `Copyright`, `PackageTags`
- **Added**: .NET Tool specific properties (conditional on non-.NET Framework targets):
  - `PackAsTool=true` (only for .NET 6.0, not .NET Framework 4.7.1)
  - `ToolCommandName=mdoc`
- **Added**: `ErrorOnDuplicatePublishOutputFiles=false` to handle multi-targeting

### 2. Removed `mdoc/mdoc.nuspec`
- The `.nuspec` file has been deleted as all metadata is now in the `.csproj` file
- This follows modern .NET best practices

### 3. Updated `Makefile`
- Changed `nuget` target from `nuget pack mdoc/mdoc.nuspec` to `dotnet pack mdoc/mdoc.csproj`
- Maintained backward compatibility with existing make commands

### 4. Updated `azure-pipelines.yml`
- Changed from `NuGetCommand@2` task to `DotNetCoreCLI@2` task for packaging
- Changed `packagesToPack` from `mdoc/mdoc.nuspec` to `mdoc/mdoc.csproj`
- Added `configuration: '$(buildConfiguration)'` parameter

### 5. Updated `README.md`
- Added "Modern .NET Build" section documenting the new build process
- Included examples of `dotnet restore`, `dotnet build`, and `dotnet pack`
- Documented how to install mdoc as a global tool

## How to Use

### Building the Project
```bash
# Restore dependencies
dotnet restore apidoctools.sln

# Build the solution
dotnet build apidoctools.sln --configuration Release
```

### Creating a NuGet Package
```bash
# Create a .NET tool package
dotnet pack mdoc/mdoc.csproj --configuration Release
```

The package will be created at: `mdoc/bin/Release/mdoc.5.9.4.nupkg`

### Installing as a .NET Global Tool
```bash
# Install from local package
dotnet tool install --global mdoc --add-source ./mdoc/bin/Release/

# Or install from NuGet.org (once published)
dotnet tool install --global mdoc

# Verify installation
mdoc --version
```

### Using with Make (Backward Compatible)
```bash
make prepare    # Initialize submodules and restore
make all        # Build everything
make nuget      # Create NuGet package (now uses dotnet pack)
```

## Technical Details

### Why PackAsTool is Conditional
The `PackAsTool` property only works with .NET Core/.NET 5+ targets, not .NET Framework. Since this project targets both `net471` and `net6.0`, we use a conditional property:

```xml
<PropertyGroup Condition="'$(TargetFramework)' != 'net471'">
  <PackAsTool>true</PackAsTool>
  <ToolCommandName>mdoc</ToolCommandName>
</PropertyGroup>
```

This ensures the tool is packaged for .NET 6.0 while still building the .NET Framework 4.7.1 executable.

### Package Structure
The generated package is structured as a .NET tool:
- Package type: `DotnetTool`
- Tools folder: `tools/net6.0/any/`
- Includes: All required assemblies and dependencies
- Includes: `DotnetToolSettings.xml` configuration

## Testing

The modernization was tested by:
1. ✅ Building the solution with `dotnet build`
2. ✅ Creating the package with `dotnet pack`
3. ✅ Installing the package as a global tool
4. ✅ Running `mdoc --version` to verify functionality
5. ✅ Testing backward compatibility with `make nuget`

## No Code Changes
As required, **no code was changed** during this modernization. Only build and packaging configuration files were modified:
- `mdoc/mdoc.csproj` (build configuration)
- `Makefile` (build automation)
- `azure-pipelines.yml` (CI/CD pipeline)
- `README.md` (documentation)

All application code remains unchanged and functions identically to before.

## Benefits

1. **Modern Tooling**: Uses current .NET SDK tooling instead of legacy nuget.exe
2. **Simplified Maintenance**: All package metadata in one place (csproj)
3. **Better Integration**: Works seamlessly with modern .NET build systems
4. **Global Tool Support**: Can be installed and used as `dotnet tool install --global mdoc`
5. **CI/CD Compatible**: Azure Pipelines and other modern CI systems prefer DotNetCoreCLI tasks
6. **Follows Best Practices**: Aligns with official Microsoft Learn documentation for .NET tools

## References
- [Tutorial: Create a .NET tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create)
- [How to manage .NET tools](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools)
- [Support multiple .NET versions](https://learn.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file)
