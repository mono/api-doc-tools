using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Mono.Documentation.Updater.Frameworks
{
    public class FrameworkEntry
    {
        SortedSet<FrameworkTypeEntry> types = new SortedSet<FrameworkTypeEntry> ();

        IList<FrameworkEntry> allframeworks;
		ISet<AssemblySet> allAssemblies = new SortedSet<AssemblySet> ();

        public int Index = 0;
        int _fxCount;
        public int FrameworksCount {
            get => _fxCount < 1 ? allframeworks.Count : _fxCount;
        }

        public FrameworkEntry (IList<FrameworkEntry> frameworks) : this(frameworks, -1) {}

        public FrameworkEntry (IList<FrameworkEntry> frameworks, int fxCount)
        {
            allframeworks = frameworks;
            if (allframeworks == null)
                allframeworks = new List<FrameworkEntry> (0);

            Index = allframeworks.Count;
            _fxCount = fxCount;
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public string Id { get; set; }

        /// <summary>Gets a value indicating whether this <see cref="T:Mono.Documentation.Updater.Frameworks.FrameworkEntry"/> is last framework being processed.</summary>
        public bool IsLastFramework {
            get => Index == FrameworksCount - 1;
        }

        string _allFxString = "";
        public string AllFrameworksString {
            get 
            {
                Lazy<string> fxString = new Lazy<string>(() => string.Join (";", allframeworks.Select (f => f.Name).ToArray ()));

                if (!this.IsLastFramework) return fxString.Value;
                if (string.IsNullOrWhiteSpace(_allFxString)) 
                {
                    _allFxString = fxString.Value;
                }
                return _allFxString;
            }
        }

        public readonly List<Tuple<string,string>> AssemblyNames = new List<Tuple<string, string>> ();

        public void AddProcessedAssembly (AssemblyDefinition assembly)
        {
            AssemblyNames.Add (new Tuple<string, string>(assembly.Name.Name, assembly.Name.Version.ToString()));
        }

        public IEnumerable<FrameworkEntry> PreviousFrameworks {
            get => allframeworks.Where (f => f.Index < this.Index);
        }

		public ISet<AssemblySet> AllProcessedAssemblies { get => allAssemblies; }

		public void AddAssemblySet (AssemblySet assemblySet)
		{
			allAssemblies.Add (assemblySet);
		}

		/// <summary>Gets a value indicating whether this <see cref="T:Mono.Documentation.Updater.Frameworks.FrameworkEntry"/> is first framework being processed.</summary>
		public bool IsFirstFramework { 
            get => this.Index == 0; 
        }

        /// <summary>Only Use in Unit Tests</summary>
        public string Replace="";

        /// <summary>Only Use in Unit Tests</summary>
        public string With ="";

        public IEnumerable<DocumentationImporter> Importers { get; set; }

        public ISet<FrameworkTypeEntry> Types { get { return this.types; } }
        Dictionary<string, FrameworkTypeEntry> typeMap = new Dictionary<string, FrameworkTypeEntry> ();

        public FrameworkTypeEntry FindTypeEntry (FrameworkTypeEntry type) 
        {
            return FindTypeEntry (Str(type.Name));    
        }

        /// <param name="name">The value from <see cref="FrameworkTypeEntry.Name"/>.</param>
        public FrameworkTypeEntry FindTypeEntry (string name) {
            FrameworkTypeEntry entry;
            typeMap.TryGetValue (Str(name), out entry);
            return entry;
        }

		public IEnumerable<FrameworkEntry> Frameworks { get { return this.allframeworks; } }

		public static readonly FrameworkEntry Empty = new EmptyFrameworkEntry () { Name = "Empty" };

		public virtual FrameworkTypeEntry ProcessType (TypeDefinition type)
		{
            FrameworkTypeEntry entry;

            if (!typeMap.TryGetValue (Str(type.FullName), out entry))
            {
				var docid = DocCommentId.GetDocCommentId (type);
                string nstouse = GetNamespace (type);
                entry = new FrameworkTypeEntry (this) { Id = Str (docid), Name = Str (type.FullName), Namespace = nstouse };
                types.Add (entry);

                typeMap.Add (Str (entry.Name), entry);
            }
            return entry;
        }

        private string GetNamespace (TypeDefinition type)
        {
            var nstouse = Str (type.Namespace);
            if (string.IsNullOrWhiteSpace (nstouse) && type.DeclaringType != null)
            {
                return GetNamespace(type.DeclaringType);
            }

            return nstouse;
        }

        string Str(string value) {
            if (!string.IsNullOrWhiteSpace (Replace))
                return value.Replace (Replace, With);
            return value;
        }

        public override string ToString () => Str(this.Name);

		class EmptyFrameworkEntry : FrameworkEntry
		{
			public EmptyFrameworkEntry () : base (null, 1) { }
			public override FrameworkTypeEntry ProcessType (TypeDefinition type) { return FrameworkTypeEntry.Empty; }
		}
	}
}
