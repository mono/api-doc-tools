using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Reflection;
using System.Collections.Generic;

namespace Monodoc.Generators.Html
{
	public class Toc2Html : IHtmlExporter
	{
		XslCompiledTransform transform;

		public Toc2Html ()
		{
			transform = new XslCompiledTransform ();
			var assembly = Assembly.GetAssembly (typeof (Toc2Html));
			var stream = assembly.GetManifestResourceStream ("toc-html.xsl");
			XmlReader xml_reader = new XmlTextReader (stream);
			transform.Load (xml_reader, null, null);
		}

		public string Export (Stream input, Dictionary<string, string> extraArgs)
		{
			var output = new StringWriter ();
			transform.Transform (new XPathDocument (input), null, output);
			return output.ToString ();
		}

		public string Export (string input, Dictionary<string, string> extraArgs)
		{
			var output = new StringWriter ();
			transform.Transform (new XPathDocument (new StringReader (input)), null, output);
			return output.ToString ();
		}

		public string CssCode {
			get {
				return string.Empty;
			}
		}
	}
}
