using System;
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
		readonly DefaultAssemblyResolver resolver = new DefaultAssemblyResolver ();
		HashSet<string> assemblyPaths = new HashSet<string> ();
		HashSet<string> assemblySearchPaths = new HashSet<string> ();
		HashSet<string> forwardedTypes = new HashSet<string> ();

		public AssemblySet (string path) : this (new string[] { path }) { }

		public AssemblySet (IEnumerable<string> paths) : this ("Default", paths, new string[0]) { }

		public AssemblySet (string name, IEnumerable<string> paths, IEnumerable<string> resolverSearchPaths)
		{
			Name = name;

			foreach (var path in paths)
				assemblyPaths.Add (path);

			// add default search paths
			var assemblyDirectories = paths
				.Where (p => p.Contains (Path.DirectorySeparatorChar))
				.Select (p => Path.GetDirectoryName (p));

			foreach (var searchPath in resolverSearchPaths.Union(assemblyDirectories))
				assemblySearchPaths.Add (searchPath);
			
			foreach (var searchPath in assemblySearchPaths)
				resolver.AddSearchDirectory (searchPath);
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
