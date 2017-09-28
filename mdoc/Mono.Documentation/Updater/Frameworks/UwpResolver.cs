using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using Mono.Cecil;

namespace Mono.Documentation.Updater.Frameworks
{
    /// <summary>Mono.Cecil resolver for the windows universal platform</summary>
    class UwpResolver : DefaultAssemblyResolver
    {
        public override AssemblyDefinition Resolve (AssemblyNameReference name)
        {
            var ver = name.Version;
            if (ver.Major == 255 && ver.Minor == 255 && ver.Revision == 255 && name.Name == "mscorlib")
            {
                var v = new Version (4, 5, 0);
                var anr = new AssemblyNameReference (name.Name, v);
                return base.Resolve (anr);
            }
            else
                return base.Resolve (name);
        }
    }
}