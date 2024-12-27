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

        [Test]
        public void Test_StartProcessingAssembly()
        {
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input", "dotnet");
            var fxconfig = XDocument.Load(Path.Combine(folder, "frameworks.xml"));
            var fxd = fxconfig.Root
                                  .Elements("Framework")
                                  .Select(f => new
                                  {
                                      Name = f.Attribute("Name").Value,
                                      Path = Path.Combine(folder, f.Attribute("Source").Value),
                                      SearchPaths = f.Elements("assemblySearchPath")
                                                   .Select(a => Path.Combine(folder, a.Value))
                                                   .ToArray(),
                                      Imports = f.Elements("import")
                                                   .Select(a => Path.Combine(folder, a.Value))
                                                   .ToArray(),
                                      Version = f.Elements("package")
                                          ?.FirstOrDefault()?.Attribute("Version")?.Value,
                                      Id = f.Elements("package")
                                       ?.FirstOrDefault()?.Attribute("Id")?.Value
                                  })
                                  .Where(f => Directory.Exists(f.Path))
                                  .ToArray();
            Func<string, string, IEnumerable<string>> getFiles = (string path, string filters) =>
            {
                var assemblyFiles = filters.Split('|').SelectMany(v => Directory.GetFiles(path, v));
                return new SortedSet<string>(assemblyFiles);
            };
            var sets = fxd.Select(d => new AssemblySet(
                    d.Name,
                    getFiles(d.Path, "*.dll|*.exe|*.winmd"),
                    d.SearchPaths.Union([folder]),
                    d.Imports,
                    d.Version,
                    d.Id
                ));

            FrameworkIndex frameworkIndex = new FrameworkIndex(folder, fxd.Count(), null);
            var frameworkEntry = frameworkIndex.StartProcessingAssembly(sets.FirstOrDefault(), sets.First().Assemblies.First(), null, null, null);
            var frameworkName = "net-5.0";
            Assert.AreEqual(frameworkName, frameworkEntry.Name);
            Assert.AreEqual(1, frameworkEntry.FrameworksCount);

            var output = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
            try
            {
                if (Directory.Exists(output))
                {
                    Directory.Delete(output, true);
                }
                _ = Directory.CreateDirectory(output);
                frameworkIndex.WriteToDisk(output);
                var outputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FrameworksIndex", frameworkName);
                Assert.IsTrue(File.Exists(outputFile));
                Directory.Delete(output, true);
            }
            finally
            {
                Directory.Delete(output, true);
            }
        }
    }
}