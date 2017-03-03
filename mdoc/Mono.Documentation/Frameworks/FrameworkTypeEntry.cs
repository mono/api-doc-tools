using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Rocks;

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

		public string Id { get; set; }
		public string Name { get; set; }
		public string Namespace { get; set; }

		public ISet<string> Members {
			get {
				return this.members;
			}
		}

		public virtual void ProcessMember (MemberReference member)
		{
			var resolvedMember = member.Resolve ();
			if (resolvedMember != null) {
				var docid = DocCommentId.GetDocCommentId (resolvedMember);
				members.Add (docid);
			}
			else 
				members.Add (member.FullName);
		}

		public override string ToString () => $"{this.Name} in {this.fx.Name}";

		public int CompareTo (FrameworkTypeEntry other)
		{
			if (other == null) return -1;
			if (this.Name == null) return 1;

			return string.Compare (this.Name, other.Name, StringComparison.CurrentCulture);
		}

		class EmptyTypeEntry : FrameworkTypeEntry
		{
			public EmptyTypeEntry (FrameworkEntry fx) : base (fx) { }
			public override void ProcessMember (MemberReference member) { }
		}
	}
}
