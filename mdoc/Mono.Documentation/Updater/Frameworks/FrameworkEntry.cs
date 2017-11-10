using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Mono.Documentation.Updater.Frameworks
{
	class FrameworkEntry
	{
		SortedSet<FrameworkTypeEntry> types = new SortedSet<FrameworkTypeEntry> ();

		List<FrameworkEntry> allframeworks;

		public FrameworkEntry (List<FrameworkEntry> frameworks)
		{
			allframeworks = frameworks;
			if (allframeworks == null)
				allframeworks = new List<FrameworkEntry> (0);
		}

		public string Name { get; set; }

		public IEnumerable<DocumentationImporter> Importers { get; set; }

		public ISet<FrameworkTypeEntry> Types { get { return this.types; } }

		public IEnumerable<FrameworkEntry> Frameworks { get { return this.allframeworks; } }

		public static readonly FrameworkEntry Empty = new EmptyFrameworkEntry () { Name = "Empty" };

		public virtual FrameworkTypeEntry ProcessType (TypeDefinition type)
		{
			var entry = types.FirstOrDefault (t => t.Name.Equals (type.FullName, StringComparison.Ordinal));
			if (entry == null) {
				var docid = DocCommentId.GetDocCommentId (type);
				entry = new FrameworkTypeEntry (this) { Id = docid, Name = type.FullName, Namespace = type.Namespace };
				types.Add (entry);
			}
			return entry;
		}

		public override string ToString () => this.Name;

		class EmptyFrameworkEntry : FrameworkEntry
		{
			public EmptyFrameworkEntry () : base (null) { }
			public override FrameworkTypeEntry ProcessType (TypeDefinition type) { return FrameworkTypeEntry.Empty; }
		}
	}
}
