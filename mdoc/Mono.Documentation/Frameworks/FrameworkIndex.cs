﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using Mono.Cecil;

namespace Mono.Documentation
{

	class FrameworkIndex
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

        public FrameworkEntry StartProcessingAssembly (AssemblyDefinition assembly, IEnumerable<DocumentationImporter> importers) 
		{
			if (string.IsNullOrWhiteSpace (this.path))
				return FrameworkEntry.Empty;

			string assemblyPath = assembly.MainModule.FileName;
			string relativePath = assemblyPath.Replace (this.path, string.Empty);
			string shortPath = Path.GetDirectoryName (relativePath);
			if (shortPath.StartsWith (Path.DirectorySeparatorChar.ToString (), StringComparison.InvariantCultureIgnoreCase))
				shortPath = shortPath.Substring (1, shortPath.Length - 1);
			

			var entry = frameworks.FirstOrDefault (f => f.Name.Equals (shortPath));
			if (entry == null) {
				entry = new FrameworkEntry (frameworks) { Name = shortPath, Importers = importers };
				frameworks.Add (entry);
			}
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
			
			foreach (var fx in this.frameworks) {

				XDocument doc = new XDocument (
					new XElement("Framework",
						new XAttribute ("Name", fx.Name),
			             fx.Types
			               .GroupBy(t => t.Namespace)
			               .Select(g => new XElement("Namespace",
	                           new XAttribute("Name", g.Key),
	                           g.Select (t => new XElement ("Type",
							    	new XAttribute ("Name", t.Name),
                                    new XAttribute("Id", t.Id),
									t.Members.Select (m =>
							  			new XElement ("Member",
								   		new XAttribute ("Id", m)))))))));

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
