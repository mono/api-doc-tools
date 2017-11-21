using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace Mono.Documentation.Updater.Frameworks
{
    /// <summary>
    /// Represents a set of assemblies that we want to document
    /// </summary>
    class AssemblySet : IDisposable
    {
        readonly BaseAssemblyResolver resolver = new Frameworks.MDocResolver ();
        IAssemblyResolver cachedResolver;
        HashSet<string> assemblyPaths = new HashSet<string> ();
        Dictionary<string, bool> assemblyPathsMap = new Dictionary<string, bool> ();
        HashSet<string> assemblySearchPaths = new HashSet<string> ();
        HashSet<string> forwardedTypes = new HashSet<string> ();
        IEnumerable<string> importPaths;
        public IEnumerable<DocumentationImporter> Importers { get; private set; }

        public AssemblySet (IEnumerable<string> paths) : this ("Default", paths, new string[0]) { }

        public AssemblySet (string name, IEnumerable<string> paths, IEnumerable<string> resolverSearchPaths, IEnumerable<string> imports = null, string version = null, string id = null)
        {
            this.cachedResolver = new CachedResolver (this.resolver);

            Name = name;
            Version = version;
            Id = id;

            foreach (var path in paths)
            {
                assemblyPaths.Add (path);
                string pathName = Path.GetFileName (path);
                if (!assemblyPathsMap.ContainsKey (pathName))
                    assemblyPathsMap.Add (pathName, true);
            }

            // add default search paths
            var assemblyDirectories = paths
                .Where (p => p.Contains (Path.DirectorySeparatorChar))
                .Select (p => Path.GetDirectoryName (p));

            foreach (var searchPath in resolverSearchPaths.Union (assemblyDirectories))
                assemblySearchPaths.Add (searchPath);

            char oppositeSeparator = Path.DirectorySeparatorChar == '/' ? '\\' : '/';
            Func<string, string> sanitize = p =>
                p.Replace (oppositeSeparator, Path.DirectorySeparatorChar);

            foreach (var searchPath in assemblySearchPaths.Select (sanitize))
                resolver.AddSearchDirectory (searchPath);

            this.importPaths = imports;
            if (this.importPaths != null)
            {
                this.Importers = this.importPaths.Select (p => MDocUpdater.Instance.GetImporter (p, supportsEcmaDoc: false)).ToArray ();
            }
            else
                this.Importers = new DocumentationImporter[0];
        }

        public string Name { get; private set; }
        public string Version { get; private set; }
        public string Id { get; private set; }

        IEnumerable<AssemblyDefinition> assemblies;
        public IEnumerable<AssemblyDefinition> Assemblies
        {
            get
            {
                if (this.assemblies == null)
                    this.assemblies = this.LoadAllAssemblies ().Where (a => a != null).ToArray ();

                return this.assemblies;
            }
        }
        public IEnumerable<string> AssemblyPaths { get { return this.assemblyPaths; } }

        /// <summary>Adds all subdirectories to the search directories for the resolver to look in.</summary>
        public void RecurseSearchDirectories ()
        {
            var directories = resolver
                .GetSearchDirectories ()
                .Select (d => new DirectoryInfo (d))
                .Where (d => d.Exists)
                .Select (d => d.FullName)
                .Distinct ()
                .ToDictionary (d => d, d => d);

            var subdirs = directories.Keys
                .SelectMany (d => Directory.GetDirectories (d, ".", SearchOption.AllDirectories))
                .Where (d => !directories.ContainsKey (d));

            foreach (var dir in subdirs)
                resolver.AddSearchDirectory (dir);
        }

        /// <returns><c>true</c>, if in set was contained in the set of assemblies, <c>false</c> otherwise.</returns>
        /// <param name="name">An assembly file name</param>
        public bool Contains (string name)
        {
            return assemblyPathsMap.ContainsKey (name);//assemblyPaths.Any (p => Path.GetFileName (p) == name);
        }

        /// <summary>Tells whether an already enumerated AssemblyDefinition, contains the type.</summary>
        /// <param name="name">Type name</param>
        public bool ContainsForwardedType (string name)
        {
            return forwardedTypes.Contains (name);
        }

        public void Dispose () 
        {
            this.assemblies = null;
            cachedResolver.Dispose();
        }

		public override string ToString ()
		{
			return string.Format ("[AssemblySet: Name={0}, Assemblies={1}]", Name, assemblyPaths.Count);
		}

		IEnumerable<AssemblyDefinition> LoadAllAssemblies ()
		{
			foreach (var path in this.assemblyPaths) {
                var assembly = MDocUpdater.Instance.LoadAssembly (path, this.cachedResolver);
				if (assembly != null) {
					foreach (var type in assembly.MainModule.ExportedTypes.Where (t => t.IsForwarder).Select (t => t.FullName))
						forwardedTypes.Add (type);
				}
				yield return assembly;
			}
		}
	}
}
