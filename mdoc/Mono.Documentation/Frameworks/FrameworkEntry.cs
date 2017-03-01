using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Mono.Documentation
{
	class FrameworkEntry
	{
		SortedSet<FrameworkTypeEntry> types = new SortedSet<FrameworkTypeEntry> ();

		public string Name { get; set; }

		public ISet<FrameworkTypeEntry> Types
		{
			get
			{
				return this.types;
			}
		}

		public static readonly FrameworkEntry Empty = new EmptyFrameworkEntry () { Name = "Empty" };

		public virtual FrameworkTypeEntry ProcessType (TypeDefinition type)
		{

			var entry = types.FirstOrDefault (t => t.Name.Equals (type.FullName));
			if (entry == null)
			{
				entry = new FrameworkTypeEntry (this) { Name = type.FullName };
				types.Add (entry);
			}
			return entry;
		}

		public override string ToString ()
		{
			return this.Name;
		}

		class EmptyFrameworkEntry : FrameworkEntry
		{
			public override FrameworkTypeEntry ProcessType (TypeDefinition type) { return FrameworkTypeEntry.Empty; }
		}
	}
}
