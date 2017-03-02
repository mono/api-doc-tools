using System;
using System.Collections.Generic;

namespace Mono.Documentation
{
	class FrameworkTypeEntry : IComparable<FrameworkTypeEntry>
	{
		SortedSet<string> members = new SortedSet<string> ();
		FrameworkEntry fx;

		public static FrameworkTypeEntry Empty = new EmptyTypeEntry (FrameworkEntry.Empty) { Name = "Empty" };

		public FrameworkTypeEntry (FrameworkEntry fx)
		{
			this.fx = fx;
		}

		public string Name { get; set; }
		public string Namespace { get; set; }

		public ISet<string> Members
		{
			get
			{
				return this.members;
			}
		}

		public virtual void ProcessMember (string sig)
		{
			members.Add (sig);
		}

		public virtual void ProcessMember (IEnumerable<string> signatures)
		{
			foreach (var sig in signatures)
			{
				ProcessMember (sig);
			}
		}

		public override string ToString ()
		{
			return $"{this.Name} in {this.fx.Name}";
		}

		public int CompareTo (FrameworkTypeEntry other)
		{
			if (other == null) return -1;
			if (this.Name == null) return 1;

			return string.Compare (this.Name, other.Name, StringComparison.CurrentCulture);
		}

		class EmptyTypeEntry : FrameworkTypeEntry
		{
			public EmptyTypeEntry (FrameworkEntry fx) : base (fx) { }
			public override void ProcessMember (IEnumerable<string> signatures) { }
			public override void ProcessMember (string sig) { }
		}
	}
}
