using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace Mono.Documentation.Updater.Frameworks
{
    /// <summary>resolver to handle assembly lookups</summary>
    /// <remarks><para>This resolver handles two scenarios. First is UWP
    /// references to 'mscorlib' (which simply picks up the 4.5 version, 
    /// or whatever you have installed). And second, it tries its 
    /// best to match on version. If it can't find the exact version, it 
    /// will try to find the next highest version, and if that fails, 
    /// a lower version if available.</para>
    /// <para>Please note that you will need to provide a reference
    /// to the UWP framework directory, if you are trying to document
    /// a UWP library. </para></remarks>
    class MDocResolver : MDocBaseResolver
    {
        IEnumerable<string> fxpaths;
        IEnumerable<string> gacpaths;

        public override AssemblyDefinition Resolve (AssemblyNameReference name, ReaderParameters parameters)
        {
            var ver = name.Version;
            if (ver.Major == 255 && ver.Minor == 255 && ver.Revision == 255 && name.Name == "mscorlib")
            {
                var v = new Version (4, 5, 0);
                var anr = new AssemblyNameReference (name.Name, v);
                return base.Resolve (anr, parameters);
            }
            else
                return base.Resolve (name, parameters);
        }

        protected override AssemblyDefinition SearchDirectory (AssemblyNameReference name, IEnumerable<string> directories, ReaderParameters parameters)
        {
            // look for an assembly that matches the name in all the search directories
            string[] extensions = new[] { ".dll", ".exe", ".winmd" };

            // Add the additional lookup directories
            if (fxpaths == null) fxpaths = GetFrameworkPaths ().ToArray ();
            if (gacpaths == null) gacpaths = GetGacPaths ().ToArray ();
            directories = directories
                .Concat (fxpaths)
                .Concat (gacpaths);
            
            var npaths = directories
                .SelectMany (d => extensions.Select (e => Path.Combine (d, name.Name + e)))
                .Distinct ();
            var namedPaths = npaths
                .Where (f => File.Exists (f));

            if (!namedPaths.Any ()) return null;

            Func<Version, int> aggregateVersion = version => version.Major * 100000 +
                        version.Minor * 10000 +
                        version.Build * 10 +
                        version.Revision;

            Func<string, AssemblyDefinition> getAssemblies = (path) => {
                return GetAssembly (path, parameters);
            };

            var applicableVersions = namedPaths.Select (getAssemblies)
                .Select (a => new
                {
                    Assembly = a,
                    VersionSort = aggregateVersion (a.Name.Version),
                    VersionDiff = aggregateVersion (a.Name.Version) - aggregateVersion (name.Version),
                    MajorMatches = a.Name.Version.Major == name.Version.Major
                })
                .ToArray ();

            // Perfect Match
            var exactMatch = applicableVersions.FirstOrDefault (v => v.VersionDiff == 0);
            if (exactMatch != null)
                return exactMatch.Assembly;

            // closest high version
            var newerVersions = applicableVersions
                .Where (v => v.VersionDiff > 0)
                .OrderBy (a => a.VersionSort)
                .Select (v => v.Assembly)
                .ToArray ();
            if (newerVersions.Any ())
                return newerVersions.First ();

            // Are there any lower versions as a last resort?
            var olderVersions = applicableVersions
                .Where (v => v.VersionDiff < 0)
                .OrderByDescending (v => v.VersionSort)
                .Select (v => v.Assembly)
                .ToArray ();
            if (olderVersions.Any ())
                return olderVersions.First ();

            // return null if you don't find anything
            return null;

        }
    }

    /// <summary>
    /// Wraps a resolver, and caches assemblies to avoid excessive calls to disk
    /// </summary>
    /// <remarks>
    /// Inspired by Mono.Cecil's DefaultAssemblyResolver (<see href="https://github.com/jbevain/cecil/blob/master/Mono.Cecil/DefaultAssemblyResolver.cs"/>).
    /// The big difference with this implementation, is one of using composition over inheritance.
    /// </remarks>
    class CachedResolver : IAssemblyResolver
    {
        readonly IDictionary<string, AssemblyDefinition> cache;

        public IAssemblyResolver Resolver { get; private set; }

        public CachedResolver (IAssemblyResolver resolver)
        {
            Resolver = resolver;
            cache = new Dictionary<string, AssemblyDefinition> (StringComparer.Ordinal);
        }

        public AssemblyDefinition Resolve (AssemblyNameReference name)
        {
            return Resolve (name, new ReaderParameters () { AssemblyResolver = this });
        }


        public AssemblyDefinition Resolve (AssemblyNameReference name, ReaderParameters parameters)
        {

            AssemblyDefinition assembly;
            if (cache.TryGetValue (name.FullName, out assembly))
                return assembly;

            assembly = this.Resolver.Resolve (name, parameters);
            cache[name.FullName] = assembly;

            return assembly;
        }

        protected void RegisterAssembly (AssemblyDefinition assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException (nameof(assembly));

            var name = assembly.Name.FullName;
            if (cache.ContainsKey (name))
                return;

            cache[name] = assembly;
        }

        bool disposed = false;
        public void Dispose ()
        {
            if (disposed) return;

            foreach (var assembly in cache.Values)
                assembly.Dispose ();

            cache.Clear ();

            Resolver.Dispose ();

            disposed = true;
        }

    }

    /// <summary>
    /// Sourced from Mono.Cecil's BaseAssemblyResolver: <see href="https://github.com/jbevain/cecil/blob/master/Mono.Cecil/BaseAssemblyResolver.cs" /> 
    /// </summary>
    /// <remarks>
    /// There are two changes made from the original source
    /// </remarks>
    abstract class MDocBaseResolver : BaseAssemblyResolver 
    {
        static readonly bool on_mono = Type.GetType ("Mono.Runtime") != null;

        Collection<string> gac_paths;

        protected AssemblyDefinition GetAssembly (string file, ReaderParameters parameters)
        {
            if (parameters.AssemblyResolver == null)
                parameters.AssemblyResolver = this;

            return ModuleDefinition.ReadModule (file, parameters).Assembly;
        }

        public override AssemblyDefinition Resolve (AssemblyNameReference name)
        {
            return this.Resolve (name, new ReaderParameters ());
        }

        public override AssemblyDefinition Resolve (AssemblyNameReference name, ReaderParameters parameters)
        {
            var directories = this.GetSearchDirectories ();
            var assembly = SearchDirectory (name, directories, parameters);
            if (assembly != null)
                return assembly;

            if (name.IsRetargetable)
            {
                // if the reference is retargetable, zero it
                name = new AssemblyNameReference (name.Name, MDocResolverMixin.ZeroVersion)
                {
                    PublicKeyToken = Empty<byte>.Array,
                };
            }

            string[] framework_dirs = GetFrameworkPaths ();

            if (IsZero (name.Version))
            {
                assembly = SearchDirectory (name, framework_dirs, parameters);
                if (assembly != null)
                    return assembly;
            }

            if (name.Name == "mscorlib")
            {
                assembly = GetCorlib (name, parameters);
                if (assembly != null)
                    return assembly;
            }

            assembly = GetAssemblyInGac (name, parameters);
            if (assembly != null)
                return assembly;

            assembly = SearchDirectory (name, framework_dirs, parameters);
            if (assembly != null)
                return assembly;

            throw new AssemblyResolutionException (name);
        }

        protected virtual AssemblyDefinition SearchDirectory (AssemblyNameReference name, IEnumerable<string> directories, ReaderParameters parameters)
        {
            var extensions = name.IsWindowsRuntime ? new[] { ".winmd", ".dll" } : new[] { ".exe", ".dll" };
            foreach (var directory in directories)
            {
                foreach (var extension in extensions)
                {
                    string file = Path.Combine (directory, name.Name + extension);
                    if (!File.Exists (file))
                        continue;
                    try
                    {
                        return GetAssembly (file, parameters);
                    }
                    catch (System.BadImageFormatException)
                    {
                        continue;
                    }
                }
            }

            return null;
        }

        static bool IsZero (Version version)
        {
            return version.Major == 0 && version.Minor == 0 && version.Build == 0 && version.Revision == 0;
        }

        AssemblyDefinition GetCorlib (AssemblyNameReference reference, ReaderParameters parameters)
        {
            var version = reference.Version;
            var corlib = typeof (object).Assembly.GetName ();

            if (corlib.Version == version || IsZero (version))
                return GetAssembly (typeof (object).Module.FullyQualifiedName, parameters);

            var path = Directory.GetParent (
                Directory.GetParent (
                    typeof (object).Module.FullyQualifiedName).FullName
                ).FullName;

            if (on_mono)
            {
                if (version.Major == 1)
                    path = Path.Combine (path, "1.0");
                else if (version.Major == 2)
                {
                    if (version.MajorRevision == 5)
                        path = Path.Combine (path, "2.1");
                    else
                        path = Path.Combine (path, "2.0");
                }
                else if (version.Major == 4)
                    path = Path.Combine (path, "4.0");
                else
                    throw new NotSupportedException ("Version not supported: " + version);
            }
            else
            {
                switch (version.Major)
                {
                    case 1:
                        if (version.MajorRevision == 3300)
                            path = Path.Combine (path, "v1.0.3705");
                        else
                            path = Path.Combine (path, "v1.0.5000.0");
                        break;
                    case 2:
                        path = Path.Combine (path, "v2.0.50727");
                        break;
                    case 4:
                        path = Path.Combine (path, "v4.0.30319");
                        break;
                    default:
                        throw new NotSupportedException ("Version not supported: " + version);
                }
            }

            var file = Path.Combine (path, "mscorlib.dll");
            if (File.Exists (file))
                return GetAssembly (file, parameters);

            return null;
        }

        protected static Collection<string> GetGacPaths ()
        {
            if (on_mono)
                return GetDefaultMonoGacPaths ();

            var paths = new Collection<string> (2);
            var windir = Environment.GetEnvironmentVariable ("WINDIR");
            if (windir == null)
                return paths;

            paths.Add (Path.Combine (windir, "assembly"));
            paths.Add (Path.Combine (windir, Path.Combine ("Microsoft.NET", "assembly")));
            return paths;
        }

        protected static string[] GetFrameworkPaths ()
        {
            var framework_dir = Path.GetDirectoryName (typeof (object).Module.FullyQualifiedName);
            var framework_dirs = on_mono
                ? new[] { framework_dir, Path.Combine (framework_dir, "Facades") }
                : new[] { framework_dir };
            return framework_dirs;
        }

        static Collection<string> GetDefaultMonoGacPaths ()
        {
            var paths = new Collection<string> (1);
            var gac = GetCurrentMonoGac ();
            if (gac != null)
                paths.Add (gac);

            var gac_paths_env = Environment.GetEnvironmentVariable ("MONO_GAC_PREFIX");
            if (string.IsNullOrEmpty (gac_paths_env))
                return paths;

            var prefixes = gac_paths_env.Split (Path.PathSeparator);
            foreach (var prefix in prefixes)
            {
                if (string.IsNullOrEmpty (prefix))
                    continue;

                var gac_path = Path.Combine (Path.Combine (Path.Combine (prefix, "lib"), "mono"), "gac");
                if (Directory.Exists (gac_path) && !paths.Contains (gac))
                    paths.Add (gac_path);
            }

            return paths;
        }

        static string GetCurrentMonoGac ()
        {
            return Path.Combine (
                Directory.GetParent (
                    Path.GetDirectoryName (typeof (object).Module.FullyQualifiedName)).FullName,
                "gac");
        }

        AssemblyDefinition GetAssemblyInGac (AssemblyNameReference reference, ReaderParameters parameters)
        {
            if (reference.PublicKeyToken == null || reference.PublicKeyToken.Length == 0)
                return null;

            if (gac_paths == null)
                gac_paths = GetGacPaths ();

            if (on_mono)
                return GetAssemblyInMonoGac (reference, parameters);

            return GetAssemblyInNetGac (reference, parameters);
        }

        AssemblyDefinition GetAssemblyInMonoGac (AssemblyNameReference reference, ReaderParameters parameters)
        {
            for (int i = 0; i < gac_paths.Count; i++)
            {
                var gac_path = gac_paths[i];
                var file = GetAssemblyFile (reference, string.Empty, gac_path);
                if (File.Exists (file))
                    return GetAssembly (file, parameters);
            }

            return null;
        }

        AssemblyDefinition GetAssemblyInNetGac (AssemblyNameReference reference, ReaderParameters parameters)
        {
            var gacs = new[] { "GAC_MSIL", "GAC_32", "GAC_64", "GAC" };
            var prefixes = new[] { string.Empty, "v4.0_" };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < gacs.Length; j++)
                {
                    var gac = Path.Combine (gac_paths[i], gacs[j]);
                    var file = GetAssemblyFile (reference, prefixes[i], gac);
                    if (Directory.Exists (gac) && File.Exists (file))
                        return GetAssembly (file, parameters);
                }
            }

            return null;
        }

        static string GetAssemblyFile (AssemblyNameReference reference, string prefix, string gac)
        {
            var gac_folder = new StringBuilder ()
                .Append (prefix)
                .Append (reference.Version)
                .Append ("__");

            for (int i = 0; i < reference.PublicKeyToken.Length; i++)
                gac_folder.Append (reference.PublicKeyToken[i].ToString ("x2"));

            return Path.Combine (
                Path.Combine (
                    Path.Combine (gac, reference.Name), gac_folder.ToString ()),
                reference.Name + ".dll");
        }

        // some helper classes
        static class Empty<T>
        {
            public static readonly T[] Array = new T[0];
        }

        class MDocResolverMixin
        {
            public static Version ZeroVersion = new Version (0, 0, 0, 0);
        }
    }
}
