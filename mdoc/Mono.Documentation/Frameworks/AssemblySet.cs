﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace Mono.Documentation
{
	/// <summary>
	/// Represents a set of assemblies that we want to document
	/// </summary>
	class AssemblySet : IDisposable
	{
		readonly DefaultAssemblyResolver resolver = new Frameworks.UwpResolver ();
		HashSet<string> assemblyPaths = new HashSet<string> ();
		HashSet<string> assemblySearchPaths = new HashSet<string> ();
		HashSet<string> forwardedTypes = new HashSet<string> ();
        IEnumerable<string> importPaths;
		public IEnumerable<DocumentationImporter> Importers { get; private set; }

		public AssemblySet (IEnumerable<string> paths) : this ("Default", paths, new string[0], null) { }

        public AssemblySet (string name, IEnumerable<string> paths, IEnumerable<string> resolverSearchPaths, IEnumerable<string> imports)
		{
			Name = name;

			foreach (var path in paths)
				assemblyPaths.Add (path);

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
                this.Importers = this.importPaths.Select (p => MDocUpdater.Instance.GetImporter (p, supportsEcmaDoc: false));
            }
            else
                this.Importers = new DocumentationImporter[0];
		}

		public string Name { get; private set; }

		public IEnumerable<AssemblyDefinition> Assemblies { get { return this.LoadAllAssemblies ().Where(a => a != null); } }
		public IEnumerable<string> AssemblyPaths { get { return this.assemblyPaths; } }

		/// <returns><c>true</c>, if in set was contained in the set of assemblies, <c>false</c> otherwise.</returns>
		/// <param name="name">An assembly file name</param>
		public bool Contains (string name)
		{
			return assemblyPaths.Any (p => Path.GetFileName (p) == name);
		}

		/// <summary>Tells whether an already enumerated AssemblyDefinition, contains the type.</summary>
		/// <param name="name">Type name</param>
		public bool ContainsForwardedType (string name)
		{
			return forwardedTypes.Contains (name);
		}

		public void Dispose () => resolver.Dispose ();

		public override string ToString ()
		{
			return string.Format ("[AssemblySet: Name={0}, Assemblies={1}]", Name, assemblyPaths.Count);
		}

		IEnumerable<AssemblyDefinition> LoadAllAssemblies ()
		{
			foreach (var path in this.assemblyPaths) {
				var assembly = MDocUpdater.Instance.LoadAssembly (path, this.resolver);
				if (assembly != null) {
					foreach (var type in assembly.MainModule.ExportedTypes.Where (t => t.IsForwarder).Select (t => t.FullName))
						forwardedTypes.Add (type);
				}
				yield return assembly;
			}
		}
	}
}
