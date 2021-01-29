using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;

namespace mdoc.Test
{
    // Fix DefaultAssemblyResolver don't load .NET Core Assemblies in macOS, WSL, and Ubuntu OS environment.
    public class DotnetCoreAssemblyResolver : DefaultAssemblyResolver
    {
        public DotnetCoreAssemblyResolver()
        {
            AddDotnetCoreSearchDirectory();
        }

        private void AddDotnetCoreSearchDirectory()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                foreach (var item in GetDotnetCorePlatformAssembliesPath())
                {
                    AddSearchDirectory(item);
                }
            }
        }

        private IEnumerable<string> GetDotnetCorePlatformAssembliesPath()
        {
            foreach (var installedSdkVersion in GetInstalledSdkVersions())
            {
                if (File.Exists(Path.Combine(installedSdkVersion, "System.dll")))
                {
                    yield return installedSdkVersion;
                }
            }
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
                // Hard code the path of .NET Core for macOS.
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
