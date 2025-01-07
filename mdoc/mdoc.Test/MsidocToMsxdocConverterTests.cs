using Mono.Documentation;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace mdoc.Test
{
    [TestFixture]
    public class MsidocToMsxdocConverterTests
    {
        private MsidocToMsxdocConverter converter;

        [SetUp]
        public void SetUp()
        {
            converter = new MsidocToMsxdocConverter();
        }

        [Test]
        public void TestRun_ValidInput_ShouldCreateOutputFiles()
        {
            string output = Path.Combine(Path.GetTempPath(), "outDir");
            string sourceDir = Path.Combine(Path.GetTempPath(), "sourceDir");
            var args = new List<string> { "msitomsx", $"-o={output}", sourceDir };
            _ = Directory.CreateDirectory($"{sourceDir}/system.string");
            _ = Directory.CreateDirectory(output);
            File.WriteAllText($"{sourceDir}/system.string/asset.xml", @"
                <doc>
                    <assembly>
                        <name>mscorlib</name>
                    </assembly>
                    <members>
                        <member name='T:System.String'>
                            <summary>Represents text as a series of Unicode characters.</summary>
                        </member>
                    </members>
                </doc>");

            converter.Run(args);

            Assert.IsTrue(File.Exists($"{output}/mscorlib.xml"));

            Directory.Delete(sourceDir, true);
            Directory.Delete(output, true);
        }


    }
}
