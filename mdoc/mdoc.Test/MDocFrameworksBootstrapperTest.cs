using Mono.Documentation;
using Mono.Documentation.Updater.Frameworks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace mdoc.Test
{
    [TestFixture]
    public class MDocFrameworksBootstrapperTest : BasicTests
    {
        [Test]
        public void Test_Run()
        {
            string inputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input");
            List<string> paras = new List<string> { "-fx", inputFolder };
            var command = new MDocFrameworksBootstrapper();
            command.Run(paras);
            string frameworksFile = Path.Combine(inputFolder, "frameworks.xml");
            var assemblyPath = Path.Combine(inputFolder, "net-5.0", "System.Console.dll");
            var fxname = FrameworkIndex.GetFrameworkNameFromPath(inputFolder, assemblyPath);
            Assert.True(File.Exists(frameworksFile));
            Assert.AreEqual("net-5.0", fxname);
        }
    }
}
