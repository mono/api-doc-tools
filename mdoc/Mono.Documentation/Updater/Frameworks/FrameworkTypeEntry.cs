using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Mono.Documentation.Updater.Frameworks
{
	public class FrameworkTypeEntry : IComparable<FrameworkTypeEntry>
	{
		SortedSet<string> members = new SortedSet<string> ();
		SortedSet<string> memberscsharpsig = new SortedSet<string> ();
        Dictionary<string, bool> sigMap = new Dictionary<string, bool> ();

		ILFullMemberFormatter formatter = new ILFullMemberFormatter ();
        DocIdFormatter docidFormatter = new DocIdFormatter ();

		FrameworkEntry fx;

        Lazy<FrameworkTypeEntry[]> previouslyProcessedFXTypes;

        /// <summary>
        /// Returns a list of all corresponding type entries,
        /// which have already been processed.
        /// </summary>
        public FrameworkTypeEntry[] PreviouslyProcessedFrameworkTypes {
            get
            {
                if (previouslyProcessedFXTypes == null)
                {
                    if (this.Framework == null || this.Framework.Frameworks == null)
                    {
                        previouslyProcessedFXTypes = new Lazy<FrameworkTypeEntry[]> (() => new FrameworkTypeEntry[0]);
                    }
                    else
                    {
                        previouslyProcessedFXTypes = new Lazy<FrameworkTypeEntry[]> (
                           () => this.Framework.Frameworks
                               .Where (f => f.Index < this.Framework.Index)
                                .Select (f => f.FindTypeEntry (this))
                                .ToArray ()
                        );
                    }
                }
                return previouslyProcessedFXTypes.Value;
            }
        }

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
                var docid = docidFormatter.GetDeclaration (member);
				members.Add (docid);
			}
			else 
				members.Add (member.FullName);

			// this is for lookup purposes
			try {
                var sig = formatter.GetDeclaration (member);
				memberscsharpsig.Add(sig);
                if (sig != null && !sigMap.ContainsKey (sig))
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

        public override int GetHashCode ()
        {
            return this.Name?.GetHashCode () ?? base.GetHashCode ();
        }

		class EmptyTypeEntry : FrameworkTypeEntry
		{
			public EmptyTypeEntry (FrameworkEntry fx) : base (fx) { }
			public override void ProcessMember (MemberReference member) { }
		}
	}
}
