using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using NUnit.Framework;

using Monodoc;
using Monodoc.Generators;

namespace MonoTests.Monodoc
{
	[TestFixture]
	public class RootTreeTest
	{
		RootTree root;
		HtmlGenerator generator;

		[SetUp]
		public void Setup ()
		{
			root = RootTree.LoadTree (GetBaseDir (), includeExternal: false);
			generator = new HtmlGenerator (defaultCache: null);
		}

		static string GetBaseDir ([CallerFilePath] string baseDir = "")
		{
			return Path.Combine (Path.GetDirectoryName (baseDir), "..", "monodoc_test");
		}

		[Test]
		public void RootTreePageTest ()
		{
			var content = root.RenderUrl ("root:", generator);
			Assert.IsNotNull (content);
			StringAssert.Contains ("The following documentation collections are available:", content);
		}

		IEnumerable<Node> GetNodesWithSummaries (Node baseNode)
		{
			return baseNode.ChildNodes.Where (n => n.Element.StartsWith ("root:/")).SelectMany (n => new[] { n }.Concat (GetNodesWithSummaries (n)));
		}

		[Test]
		public void HelpSourceSummariesTest ()
		{
			foreach (var node in GetNodesWithSummaries (root.RootNode)) {
				var content = root.RenderUrl (node.Element, generator);
				Assert.IsNotNull (content, "#1 - " + node.Element);
				if (node.ChildNodes.All (n => n.Element.StartsWith ("root:/")))
					StringAssert.Contains ("This node doesn't have a summary available", content, "#2a - " + node.Element);
				else {
					Assert.IsFalse (content.Contains ("<em>Error:</em>"), "#2b - " + node.Element);
					Assert.IsFalse (content.Contains ("This node doesn't have a summary available"), "#3b - " + node.Element);
				}
			}
		}
	}
}