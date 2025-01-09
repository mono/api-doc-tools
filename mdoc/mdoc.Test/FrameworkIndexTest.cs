using Mono.Cecil;
using Mono.Documentation.Updater;
using Mono.Documentation.Updater.Frameworks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mdoc.Test
{
    [TestFixture]
    public class FrameworkIndexTest
    {
        private FrameworkIndex frameworkIndex;
        private string testPath;
        private string testFrameworkPath;
        private List<FrameworkEntry> frameworks;

        [SetUp]
        public void Setup()
        {
            testPath = Path.Combine(Path.GetTempPath(), "frameworks");
            _ = Directory.CreateDirectory(testPath);
            frameworks = new List<FrameworkEntry>();
            frameworkIndex = new FrameworkIndex(testPath, 0, frameworks);
            testFrameworkPath = Path.Combine(testPath, "FrameworksIndex", "TestFramework.xml");
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(testPath))
            {
                Directory.Delete(testPath, true);
            }
        }

        [Test]
        public void TestStartProcessingAssembly()
        {
            var assembly = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition("TestAssembly", new Version()), "TestModule", ModuleKind.Dll);
            string path = Path.Combine(testPath, "TestAssembly", "TestModule.dll");
            if (!Directory.Exists(Path.Combine(testPath, "TestAssembly")))
            {
                _ = Directory.CreateDirectory(Path.Combine(testPath, "TestAssembly"));
            }
            assembly.Write(path);

            using var loadedAssembly = AssemblyDefinition.ReadAssembly(path);
            var set = new AssemblySet("Test", new List<string>(), new List<string>());
            var importers = new List<DocumentationImporter>();
            var entry = frameworkIndex.StartProcessingAssembly(set, loadedAssembly, importers, "TestId", "1.0.0");

            Assert.IsNotNull(entry);
            Assert.AreEqual("TestAssembly", entry.Name);
            Assert.AreEqual("TestId", entry.Id);
            Assert.AreEqual("1.0.0", entry.Version);
            Assert.AreEqual(entry, set.Framework);
            Assert.Contains(entry, frameworkIndex.Frameworks.ToList());
        }

        [Test]
        public void TestGetFrameworkNameFromPath()
        {
            string rootPath = Path.Combine(testPath, "root");
            string assemblyPath = Path.Combine(rootPath, "subdir", "assembly.dll");
            string frameworkName = FrameworkIndex.GetFrameworkNameFromPath(rootPath, assemblyPath);

            Assert.AreEqual("subdir", frameworkName);
        }

        [Test]
        public void TestWriteToDisk()
        {
            var entry = new FrameworkEntry(null, 0, null) { Name = "TestFramework" };
            frameworkIndex.Frameworks.Add(entry);
            frameworkIndex.WriteToDisk(testPath);

            Assert.IsTrue(File.Exists(testFrameworkPath));
        }
    }
}