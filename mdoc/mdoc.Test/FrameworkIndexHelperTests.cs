using Mono.Documentation;
using Mono.Documentation.Framework;
using Mono.Documentation.Updater.Frameworks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

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
        public void CreateFrameworkIndex()
        {
            var testFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Consts.FrameworksIndexFolderName);
            if (!Directory.Exists(testFolder))
            {
                Directory.CreateDirectory(testFolder);
            }

            var filePath = Path.Combine(testFolder, "frameworks.xml");
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, XmlConsts.FrameworkIndexXml);
            }
            var frameworkIndex = FrameworkIndexHelper.CreateFrameworkIndex(AppDomain.CurrentDomain.BaseDirectory, "framework1");
            Assert.IsNotNull(frameworkIndex);
            Assert.AreEqual(1, frameworkIndex.Count);
            Assert.AreEqual(2, frameworkIndex["Namespace1"].Types.Count);
            Directory.Delete(testFolder, true);
        }


        [Test]
        public void Test_Frameworks_Path()
        {
            var fxpath = "/some/path/with/frameworks.xml";
            var assemblyPath = "/some/path/with/fxname/thing.dll";
            var fxname = FrameworkIndex.GetFrameworkNameFromPath(fxpath, assemblyPath);

            Assert.AreEqual("fxname", fxname);
        }

        [Test]
        public void Test_Frameworks_Path_NoXml()
        {
            var fxpath = "/some/path/with";
            var assemblyPath = "/some/path/with/fxname/thing.dll";
            var fxname = FrameworkIndex.GetFrameworkNameFromPath(fxpath, assemblyPath);

            Assert.AreEqual("fxname", fxname);
        }

        [Test]
        public void Test_Frameworks_Path_MismatchedSlashes()
        {
            var fxpath = "/some\\path/with";
            var assemblyPath = "/some/path/with\\fxname/thing.dll";
            var fxname = FrameworkIndex.GetFrameworkNameFromPath(fxpath, assemblyPath);

            Assert.AreEqual("fxname", fxname);
        }
    }
}