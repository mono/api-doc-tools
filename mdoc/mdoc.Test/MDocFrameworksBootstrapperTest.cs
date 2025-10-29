using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Mono.Documentation;
using System.Xml.Linq;

namespace mdoc.Test
{
    [TestFixture]
    public class MDocFrameworksBootstrapperTests
    {
        private MDocFrameworksBootstrapper bootstrapper;
        private string tempDirectory;

        [SetUp]
        public void SetUp()
        {
            bootstrapper = new MDocFrameworksBootstrapper();
            tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }

        [Test]
        public void Run_ShouldThrowError_WhenFrameworkPathIsEmpty()
        {
            var ex = Assert.Throws<Exception>(() => bootstrapper.Run(new string[] { "--frameworks=" }));
            Assert.AreEqual("Framework Path should not be null or empty.", ex.Message);
        }

        [Test]
        public void Run_ShouldThrowError_WhenFrameworkPathDoesNotExist()
        {
            var nonExistentPath = Path.Combine(tempDirectory, "nonexistent");
            var ex = Assert.Throws<Exception>(() => bootstrapper.Run(new string[] { $"--frameworks={nonExistentPath}" }));
            Assert.AreEqual($"Path not found: {nonExistentPath}", ex.Message);
        }

        [Test]
        public void Run_ShouldCreateFrameworksXml_WhenValidFrameworkPathIsProvided()
        {
            var frameworkDir = Path.Combine(tempDirectory, "net5.0");
            Directory.CreateDirectory(frameworkDir);
            File.WriteAllText(Path.Combine(frameworkDir, "test.xml"), "<doc></doc>");
            File.WriteAllText(Path.Combine(frameworkDir, "test.dll"), string.Empty);

            bootstrapper.Run(new string[] { $"--frameworks={tempDirectory}" });

            var configPath = Path.Combine(tempDirectory, "frameworks.xml");
            Assert.IsTrue(File.Exists(configPath));
        }

        [Test]
        public void Run_ShouldImportContent_WhenImportContentIsTrue()
        {
            var frameworkDir = Path.Combine(tempDirectory, "net5.0");
            Directory.CreateDirectory(frameworkDir);
            File.WriteAllText(Path.Combine(frameworkDir, "test.xml"), "<doc></doc>");
            File.WriteAllText(Path.Combine(frameworkDir, "test.dll"), string.Empty);

            bootstrapper.Run(new string[] { $"--frameworks={tempDirectory}", "--importContent=true" });

            var configPath = Path.Combine(tempDirectory, "frameworks.xml");
            var doc = XDocument.Load(configPath);
            var importElements = doc.Descendants("import").ToList();
            Assert.IsNotEmpty(importElements);
        }

        [Test]
        public void Run_ShouldNotImportContent_WhenImportContentIsFalse()
        {
            var frameworkDir = Path.Combine(tempDirectory, "net5.0");
            Directory.CreateDirectory(frameworkDir);
            File.WriteAllText(Path.Combine(frameworkDir, "test.xml"), "<doc></doc>");
            File.WriteAllText(Path.Combine(frameworkDir, "test.dll"), string.Empty);

            bootstrapper.Run(new string[] { $"--frameworks={tempDirectory}", "--importContent=false" });

            var configPath = Path.Combine(tempDirectory, "frameworks.xml");
            var doc = XDocument.Load(configPath);
            var importElements = doc.Descendants("import").ToList();
            Assert.IsEmpty(importElements);
        }
    }
}