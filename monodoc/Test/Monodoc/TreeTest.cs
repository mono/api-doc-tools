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
	public class TreeTest
	{
		[Test]
		public void TestLoadingTree_2_10 ()
		{
			TestTreeLoading ("tree-from-2-10.tree", 0, 2);
		}

		[Test]
		public void TestLoadingTree_3_0_old ()
		{
			TestTreeLoading ("tree-from-3-0-old.tree", 1, 2);
		}

		[Test]
		public void TestLoadingTree_3_0 ()
		{
			TestTreeLoading ("tree-from-3-0.tree", 1, 2);
		}

		void TestTreeLoading (string treeFileName, int expectedVersion, int expectedNodeCount, [CallerFilePath] string baseDir = "")
		{
			var filePath = Path.Combine (Path.GetDirectoryName (baseDir), "..", "monodoc_test", "trees", treeFileName);

			var tree = new Tree (null, filePath);
			Assert.AreEqual (expectedVersion, tree.VersionNumber);
			Assert.IsNotNull (tree.RootNode);
			Assert.AreEqual (expectedNodeCount, tree.RootNode.ChildNodes.Count);
		}
	}
}
