// Command to preserve member documentation for types that are changing in a subsequent version
// By Joel Martinez <joel.martinez@xamarin.com
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Mono.Options;

namespace Mono.Documentation
{
	public class MDocFrameworksBootstrapper : MDocCommand
	{
		public override void Run (IEnumerable<string> args)
		{
			args = args.Skip (1);
			if (args.Count () != 1)
				Error ("Need to supply a single directory, which contain folders that represent frameworks.");

			string frameworkPath = args.Single ();
			int slashOffset = frameworkPath.EndsWith (Path.DirectorySeparatorChar.ToString (), StringComparison.InvariantCultureIgnoreCase) ? 0 : 1;

			if (!Directory.Exists(frameworkPath)) 
				Error ($"Path not found: {frameworkPath}");

			var data = Directory.GetDirectories (frameworkPath)
			                    .Select (d => new {
									Path = d.Substring (frameworkPath.Length + slashOffset, d.Length - frameworkPath.Length - slashOffset),
									Name = Path.GetFileName(d)
								})
                                .Where (d => !d.Name.Equals ("dependencies", StringComparison.OrdinalIgnoreCase))
                                .OrderBy(d => d.Name)
                                .ToArray();

			foreach (var d in data)
				Console.WriteLine (d.Name);

			var doc = new XDocument (
				new XElement("Frameworks", 
				             data.Select(d => new XElement(
					             "Framework",
					             new XAttribute("Name", d.Name),
					             new XAttribute("Source", d.Path),
					             new XElement("assemblySearchPath", Path.Combine("dependencies", d.Name)))))
			);

			var configPath = Path.Combine (frameworkPath, "frameworks.xml");
			var settings = new XmlWriterSettings { Indent = true };
			using (var writer = XmlWriter.Create (configPath, settings)) {
				doc.WriteTo (writer);
			}

			Console.WriteLine ($"Framework configuration file written to {configPath}");
		}
	}
}

