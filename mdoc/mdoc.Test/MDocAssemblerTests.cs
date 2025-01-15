using Mono.Documentation;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace mdoc.Test
{

    [TestFixture]
    public class MDocAssemblerTests
    {
        [Test]
        public void CreateFormatOptions_ValidFormat_ShouldReturnOptions()
        {
            // Arrange
            var assembler = new MDocAssembler();
            var formats = new Dictionary<string, List<string>>();

            // Act
            var options = MDocAssembler.CreateFormatOptions(assembler, formats);

            // Assert
            Assert.AreEqual(2, options.Length);
            Assert.IsNotNull(options[0]);
            Assert.IsNotNull(options[1]);
        }

        [Test]
        public void Run_WithValidArguments_ShouldPopulateTree()
        {
            // Arrange
            var tempDir = Path.Combine(Path.GetTempPath(), "framework1");
            var outputDir = Path.Combine(tempDir, "output");
            _ = Directory.CreateDirectory(tempDir);
            File.WriteAllText(Path.Combine(tempDir, "ns-System.Test.xml"),
@"<Namespace Name=""System.Test"">
  <Docs>
    <summary>To be added.</summary>
    <remarks>To be added.</remarks>
  </Docs>
</Namespace>
");
            var testDir = Path.Combine(tempDir, "System.Test");
            _ = Directory.CreateDirectory(testDir);
            var testClassContent =
@"<Type Name=""TestClass"" FullName=""System.Text.TestClass"">
  <Base>
    <BaseTypeName>System.Object</BaseTypeName>
  </Base>
  <Docs>
    <summary>To be added.</summary>
    <remarks>To be added.</remarks>
  </Docs>
  <Members>
    <Member MemberName=""TestMethod"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Int32</ReturnType>
      </ReturnValue>
      <Docs>
        <summary>To be added.</summary>
        <value>To be added.</value>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>
  </Members>
</Type>
";
            File.WriteAllText(Path.Combine(testDir, "System.Test.TestClass.xml"), testClassContent);
            var assembler = new MDocAssembler();
            var args = new List<string> { "assembler", "--format=ecma", $"--out={outputDir}", tempDir };

            // Act
            assembler.Run(args);

            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(tempDir, "output.tree")));
            Assert.IsTrue(File.Exists(Path.Combine(tempDir, "output.zip")));
            Directory.Delete(tempDir, true);
        }
    }
}
