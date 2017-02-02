using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

using NUnit.Framework;

using Monodoc;
using Monodoc.Generators;

namespace MonoTests.Monodoc.Generators
{
	[TestFixture]
	public class RawGeneratorTests
	{
		string BaseDir
		{
			get
			{
				var baseDir = "../../monodoc_test/";
				var assemblyLocation = this.GetType ().Assembly.Location;
				return Path.GetFullPath (
					Path.Combine (
						Path.GetDirectoryName (assemblyLocation),
						baseDir));
			}
		}

		RootTree rootTree;
		RawGenerator generator = new RawGenerator ();

		[SetUp]
		public void Setup ()
		{
			rootTree = RootTree.LoadTree (BaseDir, false);
		}

		void AssertValidXml (string xml)
		{
			var reader = XmlReader.Create (new StringReader (xml));
			try {
				while (reader.Read ());
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				Assert.Fail (e.Message);
			}
		}

		void AssertEcmaFullTypeName (string xml, string fullTypeName)
		{
			var reader = XmlReader.Create (new StringReader (xml));
			Assert.IsTrue (reader.ReadToFollowing ("Type"));
			Assert.AreEqual (fullTypeName, reader.GetAttribute ("FullName"));
		}

		[Test]
		public void TestSimpleEcmaXml ()
		{
			var xml = rootTree.RenderUrl ("T:System.String", generator);
			Assert.IsNotNull (xml);
			Assert.IsNotEmpty (xml);
			AssertValidXml (xml);
			AssertEcmaFullTypeName (xml, "System.String");
		}

		[Test]
		public void TestSimpleEcmaXml2 ()
		{
			var xml = rootTree.RenderUrl ("T:System.Int32", generator);
			Assert.IsNotNull (xml);
			Assert.IsNotEmpty (xml);
			AssertValidXml (xml);
			AssertEcmaFullTypeName (xml, "System.Int32");
		}
	}
}
