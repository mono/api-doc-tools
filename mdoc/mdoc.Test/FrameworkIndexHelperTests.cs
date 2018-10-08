using System.Collections.Generic;
using System.Linq;
using Mono.Documentation.Framework;
using Mono.Documentation.Updater.Frameworks;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture]
    public class FrameworkIndexHelperTests : BasicTests
    {
        [Test]
        public void ReadFrameworkIndex0()
        {
            Dictionary<string, FrameworkNamespaceModel> frameworkIndex = FrameworkIndexHelper.ReadFrameworkIndex(ReadXDocument(XmlConsts.FrameworkIndexXml).CreateReader());

            Assert.AreEqual(1, frameworkIndex.Count);
            Assert.AreEqual(2, frameworkIndex["Namespace1"].Types.Count);
            Assert.AreEqual("Type1", frameworkIndex["Namespace1"].Types.Single(i => i.Id == "T:Type1").Name);
            Assert.AreEqual("Type2", frameworkIndex["Namespace1"].Types.Single(i => i.Id == "T:Type2").Name);
        }

        [Test]
        public void ReadFrameworkIndex1()
        {
            Dictionary<string, FrameworkNamespaceModel> frameworkIndex = FrameworkIndexHelper.ReadFrameworkIndex(ReadXDocument(XmlConsts.FrameworkIndexXml2).CreateReader());

            Assert.AreEqual(2, frameworkIndex.Count);
            Assert.AreEqual(1, frameworkIndex["Namespace1"].Types.Count);
            Assert.AreEqual("Type1", frameworkIndex["Namespace1"].Types.Single(i => i.Id == "T:Type1").Name);
            Assert.AreEqual(1, frameworkIndex["Namespace2"].Types.Count);
            Assert.AreEqual("Type2", frameworkIndex["Namespace2"].Types.Single(i => i.Id == "T:Type2").Name);
        }



        [Test]
        public void Test_Frameworks_Path ()
        {
            var fxpath = "/some/path/with/frameworks.xml";
            var assemblyPath = "/some/path/with/fxname/thing.dll";
            var fxname = FrameworkIndex.GetFrameworkNameFromPath (fxpath, assemblyPath);

            Assert.AreEqual ("fxname", fxname);
        }

        public void Test_Frameworks_Path_NoXml ()
        {
            var fxpath = "/some/path/with";
            var assemblyPath = "/some/path/with/fxname/thing.dll";
            var fxname = FrameworkIndex.GetFrameworkNameFromPath (fxpath, assemblyPath);

            Assert.AreEqual ("fxname", fxname);
        }

        public void Test_Frameworks_Path_MismatchedSlashes ()
        {
            var fxpath = "/some\\path/with";
            var assemblyPath = "/some/path/with\\fxname/thing.dll";
            var fxname = FrameworkIndex.GetFrameworkNameFromPath (fxpath, assemblyPath);

            Assert.AreEqual ("fxname", fxname);
        }
    }
}