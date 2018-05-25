﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using Mono.Cecil;

namespace Mono.Documentation.Updater.Frameworks
{

	public class FrameworkIndex
	{
		List<FrameworkEntry> frameworks = new List<FrameworkEntry> ();
		string path;

		public FrameworkIndex (string pathToFrameworks) 
		{
			path = pathToFrameworks;
		}

		public IList<FrameworkEntry> Frameworks {
			get {
				return this.frameworks;
			}
		}

        public FrameworkEntry StartProcessingAssembly (AssemblySet set, AssemblyDefinition assembly, IEnumerable<DocumentationImporter> importers, string Id, string Version) 
		{
            if (string.IsNullOrWhiteSpace (this.path))
            {
                set.Framework = FrameworkEntry.Empty;
                return FrameworkEntry.Empty;
            }

			string assemblyPath = assembly.MainModule.FileName;
			var frameworksDirectory = this.path.EndsWith ("frameworks.xml", StringComparison.OrdinalIgnoreCase)
	                                        ? Path.GetDirectoryName (this.path) : this.path;
			string relativePath = assemblyPath.Replace (frameworksDirectory, string.Empty);
			string shortPath = Path.GetDirectoryName (relativePath);
			if (shortPath.StartsWith (Path.DirectorySeparatorChar.ToString (), StringComparison.InvariantCultureIgnoreCase))
				shortPath = shortPath.Substring (1, shortPath.Length - 1);
			

			var entry = frameworks.FirstOrDefault (f => f.Name.Equals (shortPath));
			if (entry == null) {
				entry = new FrameworkEntry (frameworks) { Name = shortPath, Importers = importers, Id = Id, Version = Version};
				frameworks.Add (entry);
			}

            set.Framework = entry;
			return entry;
		}

		/// <summary>Writes the framework indices to disk.</summary>
		/// <param name="path">The folder where one file for every FrameworkEntry will be written.</param>
		public void WriteToDisk (string path) 
		{
			if (string.IsNullOrWhiteSpace (this.path))
				return;
			
			string outputPath = Path.Combine (path, "FrameworksIndex");
			if (!Directory.Exists (outputPath))
				Directory.CreateDirectory (outputPath);

			foreach (var fx in this.frameworks)
			{
				XElement frameworkElement = new XElement("Framework", new XAttribute("Name", fx.Name));
				XDocument doc = new XDocument(
					frameworkElement
					);
				if (fx.Version!=null && fx.Id!= null)
				{
					frameworkElement.Add(new XElement("package", new XAttribute("Id", fx.Id),
						new XAttribute("Version", fx.Version)
						));
				}
				frameworkElement.Add(fx.Types.GroupBy(t => t.Namespace)
					.Select(g => new XElement("Namespace",
						new XAttribute("Name", g.Key),
						g.Select(t => new XElement("Type",
							new XAttribute("Name", t.Name),
							new XAttribute("Id", t.Id),
							t.Members.Select(m =>
								new XElement("Member",
									new XAttribute("Id", m))))))));
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
