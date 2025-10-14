using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using NUnit.Framework;

using Monodoc;

namespace MonoTests.Monodoc
{
	[TestFixture]
	public class NodeTest
	{
		[Test]
		public void LegacyNodesTest_30 ()
		{
			TestLegacyNodesSameAsChildNodes ("tree-from-3-0.tree");
		}

		[Test]
		public void LegacyNodesTest_210 ()
		{
			TestLegacyNodesSameAsChildNodes ("tree-from-2-10.tree");
		}

		[Test]
		public void LegacyNodesTest_30old ()
		{
			TestLegacyNodesSameAsChildNodes ("tree-from-3-0-old.tree");
		}

		void TestLegacyNodesSameAsChildNodes (string treeFileName, [CallerFilePath] string baseDir = "")
		{
			var filePath = Path.Combine (Path.GetDirectoryName (baseDir), "..", "monodoc_test", "trees", treeFileName);

			var tree = new Tree (null, filePath);
			CollectionAssert.AreEqual (tree.RootNode.ChildNodes, tree.RootNode.Nodes);
		}
	}
}
