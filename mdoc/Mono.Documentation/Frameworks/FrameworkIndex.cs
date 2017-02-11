using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using Mono.Cecil;

namespace Mono.Documentation
{
	public class FrameworkTypeEntry
	{
		SortedSet<string> members = new SortedSet<string> ();
		FrameworkEntry fx;

		public FrameworkTypeEntry (FrameworkEntry fx) {
			this.fx = fx;
		}

		public string Name { get; set; }

		public ISet<string> Members {
			get {
				return this.members;
			}
		}

		public void ProcessMember (IEnumerable<string> signatures) {
			foreach (var sig in signatures) {
				members.Add (sig);
			}
		}

		public override string ToString () {
			return $"{this.Name} in {this.fx.Name}";
		}
	}

	public class FrameworkEntry
	{
		List<FrameworkTypeEntry> types = new List<FrameworkTypeEntry> ();

		public string Name { get; set; }

		public IList<FrameworkTypeEntry> Types {
			get {
				return this.types;
			}
		}

		public FrameworkTypeEntry ProcessType (TypeDefinition type) {
			
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
