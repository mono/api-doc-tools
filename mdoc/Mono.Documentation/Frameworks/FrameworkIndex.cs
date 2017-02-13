using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using Mono.Cecil;

namespace Mono.Documentation
{
	public class FrameworkTypeEntry : IComparable<FrameworkTypeEntry>
	{
		SortedSet<string> members = new SortedSet<string> ();
		FrameworkEntry fx;

		public static FrameworkTypeEntry Empty = new EmptyTypeEntry (FrameworkEntry.Empty) { Name = "Empty" };

		public FrameworkTypeEntry (FrameworkEntry fx) {
			this.fx = fx;
		}

		public string Name { get; set; }

		public ISet<string> Members {
			get {
				return this.members;
			}
		}

		public virtual void ProcessMember (string sig) {
			members.Add (sig);
		}

		public virtual void ProcessMember (IEnumerable<string> signatures) {
			foreach (var sig in signatures) {
				ProcessMember (sig);
			}
		}

		public override string ToString () {
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

	public class FrameworkEntry
	{
		SortedSet<FrameworkTypeEntry> types = new SortedSet<FrameworkTypeEntry> ();

		public string Name { get; set; }

		public ISet<FrameworkTypeEntry> Types {
			get {
				return this.types;
			}
		}

		public static readonly FrameworkEntry Empty = new EmptyFrameworkEntry () { Name = "Empty" };

		public virtual FrameworkTypeEntry ProcessType (TypeDefinition type) {
			
			var entry = types.FirstOrDefault (t => t.Name.Equals (type.FullName));
			if (entry == null)
			{
				entry = new FrameworkTypeEntry (this) { Name = type.FullName };
				types.Add (entry);
			}
			return entry;
		}

		public override string ToString () {
			return this.Name;
		}

		class EmptyFrameworkEntry : FrameworkEntry
		{
			public override FrameworkTypeEntry ProcessType (TypeDefinition type) { return FrameworkTypeEntry.Empty; }
		}
}

	public class FrameworkIndex
	{
		List<FrameworkEntry> frameworks = new List<FrameworkEntry> ();
		string path;

		public FrameworkIndex (string pathToFrameworks) {
			path = pathToFrameworks;
		}

		public IList<FrameworkEntry> Frameworks {
			get {
				return this.frameworks;
			}
		}

		public FrameworkEntry StartProcessingAssembly (AssemblyDefinition assembly) {
			if (string.IsNullOrWhiteSpace (this.path))
				return FrameworkEntry.Empty;

			string assemblyPath = assembly.MainModule.FileName;
			string relativePath = assemblyPath.Replace (this.path, string.Empty);
			string shortPath = Path.GetDirectoryName (relativePath);
			if (shortPath.StartsWith (Path.DirectorySeparatorChar.ToString (), StringComparison.InvariantCultureIgnoreCase))
				shortPath = shortPath.Substring (1, shortPath.Length - 1);
			

			var entry = frameworks.FirstOrDefault (f => f.Name.Equals (shortPath));
			if (entry == null) {
				entry = new FrameworkEntry { Name = shortPath };
				frameworks.Add (entry);
			}
			return entry;
		}

		public void Write (string path) {
			if (string.IsNullOrWhiteSpace (this.path))
				return;
			
			string outputPath = Path.Combine (path, "FrameworksIndex");
			if (!Directory.Exists (outputPath))
				Directory.CreateDirectory (outputPath);
			
			foreach (var fx in this.frameworks) {

				XDocument doc = new XDocument (
					new XElement("Framework",
						new XAttribute ("Name", fx.Name),
				             fx.Types.Select(t => new XElement("Type",
                                   new XAttribute("Name", t.Name),
                                   t.Members.Select(m => 
	                                	new XElement("Member", 
			                                 new XAttribute("Sig", m)))))));

				// now save the document
				string filePath = Path.Combine (outputPath, fx.Name + ".xml");

				if (File.Exists (filePath))
					File.Delete (filePath);

				var settings = new XmlWriterSettings { Indent = true };
				using (var writer = XmlWriter.Create (filePath, settings)) {
					doc.WriteTo (writer);
				}
			}
		}

	}
}
