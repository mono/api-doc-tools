using mdoc.Test.SampleClasses;
using Mono.Documentation.Updater;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture]
    public class UWPDocUtilsTests : BasicTests
    {
        private const string UWPTestComponentWinMD = "../../../../external/Test/UWPTestComponentCSharp.winmd";

        [Test]
        public void IsIgnored_PutMethodIsGeneratedByProperty_IsIgnoredTrue()
        {
            var classWithProperties = GetType(UWPTestComponentWinMD, "mdoc.Test.UWP.TestComponent.UwpClassWithProperties");

            var method = GetMethod(classWithProperties, i => i.Name == "put_MyReadWriteProperty");

            var isIgnoredPropertyGeneratedMethod = DocUtils.IsIgnored(method);

            Assert.IsTrue(isIgnoredPropertyGeneratedMethod);
        }
    }
}