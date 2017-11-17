using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Mono.Documentation.Updater.Frameworks
{
	class FrameworkTypeEntry : IComparable<FrameworkTypeEntry>
	{
		SortedSet<string> members = new SortedSet<string> ();
		SortedSet<string> memberscsharpsig = new SortedSet<string> ();
        Dictionary<string, bool> sigMap = new Dictionary<string, bool> ();

		ILFullMemberFormatter formatter = new ILFullMemberFormatter ();

		FrameworkEntry fx;

		public static FrameworkTypeEntry Empty = new EmptyTypeEntry (FrameworkEntry.Empty) { Name = "Empty" };

		public FrameworkTypeEntry (FrameworkEntry fx)
		{
			this.fx = fx;
		}

		public string Id { get; set; }
		public string Name { get; set; }
		public string Namespace { get; set; }
		public FrameworkEntry Framework { get { return fx; } }

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

			// this is for lookup purposes
			try {
                var sig = formatter.GetDeclaration (member);
				memberscsharpsig.Add(sig);
                if (!sigMap.ContainsKey (sig))
                    sigMap.Add (sig, true);
			}
			catch {}
		}

		public bool ContainsCSharpSig (string sig)
		{
			return sigMap.ContainsKey (sig);
		}

		public override string ToString () => $"{this.Name} in {this.fx.Name}";

		public int CompareTo (FrameworkTypeEntry other)
		{
			if (other == null) return -1;
			if (this.Name == null) return 1;

			return string.Compare (this.Name, other.Name, StringComparison.CurrentCulture);
		}

		public override bool Equals (object obj)
		{
			FrameworkTypeEntry other = obj as FrameworkTypeEntry;
			if (other == null) return false;
			return this.Name.Equals (other.Name);
		}

		class EmptyTypeEntry : FrameworkTypeEntry
		{
			public EmptyTypeEntry (FrameworkEntry fx) : base (fx) { }
			public override void ProcessMember (MemberReference member) { }
		}
	}
}
