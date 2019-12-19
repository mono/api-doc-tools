using NUnit.Framework;
using System.Linq;
using mdoc.Test.SampleClasses;
using Mono.Documentation.Updater;

namespace mdoc.Test
{
    [TestFixture]
    public class CsharpFormatterTests : BasicFormatterTests<CSharpMemberFormatter>
    {
        private CSharpMemberFormatter csharpMemberFormatter = new CSharpMemberFormatter();
        protected override CSharpMemberFormatter formatter => csharpMemberFormatter;

        [Test]
        public void FuncParams()
        {
            var member = GetMethod(typeof(TestClass), m => m.Name == "DoWithNullParams");
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual("public string DoWithNullParams (out string a, object b = default, TestClass c = default);", sig);
        }
    }
}
