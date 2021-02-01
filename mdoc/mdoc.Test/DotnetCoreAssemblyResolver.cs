using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mdoc.Test
{
    // Fix DefaultAssemblyResolver don't load .NET Core platform assemblies in macOS, WSL, and Ubuntu OS environment.
    public class DotnetCoreAssemblyResolver : DefaultAssemblyResolver
    {
        public DotnetCoreAssemblyResolver()
        {
            AddDotnetCoreToSearchDirectory();
        }

        private void AddDotnetCoreToSearchDirectory()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                var latestDotnetCorePath = GetLatestPlatformAssembliesPath();
                if (string.IsNullOrEmpty(latestDotnetCorePath))
                {
                    throw new DirectoryNotFoundException("The platform assemblies of .NET Core was not found, do you have .NET Core installed?");
                }

                AddSearchDirectory(latestDotnetCorePath);
            }
        }

        private string GetLatestPlatformAssembliesPath()
        {
            SortedList<Version, string> versionResults = new SortedList<Version, string>();
            foreach (var installedSdkVersion in GetInstalledSdkVersions())
            {
                if (File.Exists(Path.Combine(installedSdkVersion, "System.dll")))
                {
                    Version sdkVersion;
                    DirectoryInfo sdkDirectoryInfo = new DirectoryInfo(installedSdkVersion);
                    if (Version.TryParse(sdkDirectoryInfo.Name, out sdkVersion))
                    {
                        versionResults.Add(sdkVersion, installedSdkVersion);
                    }
                }
            }

            return versionResults.LastOrDefault().Value;
        }

        private string[] GetInstalledSdkVersions()
        {
            var dotnetCorePackagesPath = GetDotnetCorePath();
            if (Directory.Exists(dotnetCorePackagesPath))
            {
                return Directory.GetDirectories(dotnetCorePackagesPath);
            }

            return Array.Empty<string>();
        }

        private string GetDotnetCorePath()
        {
            var dotnetCorePath = GetMacOSDotnetCorePath();
            if (string.IsNullOrEmpty(dotnetCorePath))
            {
                dotnetCorePath = GetLinuxDotnetCorePath();
            }

            if (!Directory.Exists(dotnetCorePath))
            {
                throw new DirectoryNotFoundException($"The path of .NET Core was not found, do you have .NET Core installed? {dotnetCorePath}");
            }

            return dotnetCorePath;
        }

        private string GetMacOSDotnetCorePath()
        {
            var macOSDotnetCorePath = GetAzureMacOSDotnetCorePath();
            if (string.IsNullOrEmpty(macOSDotnetCorePath))
            {
                // Hard code the local path of .NET Core for macOS.
                macOSDotnetCorePath = "/usr/local/share/dotnet/shared/Microsoft.NETCore.App";
            }

            return Directory.Exists(macOSDotnetCorePath) ? macOSDotnetCorePath : string.Empty;
        }

        private string GetAzureMacOSDotnetCorePath()
        {
            var azureMacOSDotnetCorePath = Environment.GetEnvironmentVariable("DOTNET_ROOT");
            if (!string.IsNullOrEmpty(azureMacOSDotnetCorePath))
            {
                return Path.Combine(azureMacOSDotnetCorePath, "shared/Microsoft.NETCore.App");
            }

            return string.Empty;
        }

        private string GetLinuxDotnetCorePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "dotnet/shared/Microsoft.NETCore.App");
        }
    }
}
