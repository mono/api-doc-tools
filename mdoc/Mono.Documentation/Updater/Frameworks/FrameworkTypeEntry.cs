using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Mono.Documentation.Updater.Frameworks
{
	public class FrameworkTypeEntry : IComparable<FrameworkTypeEntry>
	{
        Dictionary<string, string> sigMap = new Dictionary<string, string> ();

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

		public IEnumerable<string> Members {
			get {
				return this.sigMap.Values.OrderBy(v => v).Distinct();
			}
		}

		public virtual void ProcessMember (MemberReference member)
        {
            string key = null;

            // this is for lookup purposes
            try
            {
                var sig = formatter.GetDeclaration(member);
                if (sig != null && !sigMap.ContainsKey (sig))
                {
                    sigMap.Add (sig, string.Empty);
                    key = sig;
                }
            }
            catch { }

            if (key == null)
                return;

            var resolvedMember = member.Resolve ();
			if (resolvedMember != null) {
                var docid = docidFormatter.GetDeclaration (member);
				sigMap[key] = docid;
			}
			else 
				sigMap[key] = member.FullName;

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
