using Mono.Documentation.Updater;
using Mono.Cecil;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Mono.Documentation.Updater.Formatters.CppFormatters;

namespace mdoc.Test
{
    [TestFixture]
    public class TypeMapTests : CecilBaseTest
    {
        [Test]
        public void LoadTypeMap()
        {
            var map = TypeMap.FromXDocument(XDocument.Parse(simpleSourceFile));

            Assert.AreEqual(2, map.Items.Count);

            var item1 = map.Items.First();
            Assert.AreEqual("Windows.Foundation.IClosable", item1.From);
            Assert.AreEqual("System.IDisposable", item1.To);
            Assert.AreEqual("C#;VB.NET;F#", item1.Langs);
            Assert.AreEqual(3, item1.LangList.Count());

            var item2 = map.Items.Last();
            Assert.AreEqual("System.DateTime", item2.From);
            Assert.AreEqual("System.DateTimeOffset", item2.To);
            Assert.AreEqual("C#;F#", item2.Langs);
            Assert.AreEqual(2, item2.LangList.Count());
        }

        [Test]
        public void ReplaceType()
        {
            var map = TypeMap.FromXDocument(XDocument.Parse(simpleSourceFile));

            var actualValue = map.GetTypeName("C#", "System.DateTime");
            Assert.AreEqual("System.DateTimeOffset", actualValue);
        }

        [Test]
        public void ReplaceInFormatter()
        {
            var typedef = GetTypeDef<UWPProjection>();

            var map = TypeMap.FromXDocument(XDocument.Parse(simplerSourceFile));
            CSharpFullMemberFormatter formatter = new CSharpFullMemberFormatter(map);

            string actual = formatter.GetDeclaration(typedef);
            string actualName = formatter.GetName(typedef);

            Assert.AreEqual("public class System.String", actual);
            Assert.AreEqual("System.String", actualName);
        }

        [Test]
        public void ReplaceInFormatter_ButDont()
        {
            var typedef = GetTypeDef<UWPProjection>();

            var map = TypeMap.FromXDocument(XDocument.Parse(simplerSourceFile));
            CSharpFullMemberFormatter formatter = new CSharpFullMemberFormatter(map);

            string actualName = formatter.GetName(typedef, useTypeProjection:false);

            Assert.AreEqual("mdoc.Test.TypeMapTests.UWPProjection", actualName);
        }

        [Test]
        public void ReplaceInFormatter_WrongLanguage()
        {
            var typedef = GetTypeDef<UWPProjection>();

            var map = TypeMap.FromXDocument(XDocument.Parse(simplerSourceFile));
            CppCxFullMemberFormatter formatter = new CppCxFullMemberFormatter(map);

            string actual = formatter.GetDeclaration(typedef);
            string actualName = formatter.GetName(typedef);

            Assert.AreEqual("mdoc::Test::TypeMapTests::UWPProjection", actualName);
        }


        string simpleSourceFile = @"<TypeMap>
    <InterfaceReplace
        From=""Windows.Foundation.IClosable""
        To=""System.IDisposable""
        Langs=""C#;VB.NET;F#"">
        <Members>
            <Member MemberName = ""Dispose"" >
                <stuff />
            </Member>
        </Members>
    </InterfaceReplace>
    <TypeReplace
        From = ""System.DateTime""
        To= ""System.DateTimeOffset""
        Langs= ""C#;F#"" />
</TypeMap> ";

        string simplerSourceFile = @"<TypeMap>
    <TypeReplace
        From = ""mdoc.Test.TypeMapTests.UWPProjection""
        To= ""System.String""
        Langs= ""C#;F#"" />
</TypeMap> ";

        public class UWPProjection
        {
            public void Dispose(){}
        }
    }
}