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

            Assert.AreEqual("public class System.String : mdoc.Test.IInt<int>", actual);
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

        [Test]
        public void ReplaceInFormatter_Generic_Property()
        {
            var typedef = GetTypeDef<UWPProjection>();

            var map = TypeMap.FromXDocument(XDocument.Parse(genericInterfaceSourceFile));
            CSharpFullMemberFormatter formatter = new CSharpFullMemberFormatter(map);

            string actual = formatter.GetDeclaration(typedef.Resolve().Properties.Single(t => t.Name == "Thing"));

            Assert.AreEqual("public System.Collections.Generic.IList<int> Thing { get; }", actual);
        }

        [Test]
        public void ReplaceInFormatter_Generic_Method()
        {
            var typedef = GetTypeDef<UWPProjection>();

            var map = TypeMap.FromXDocument(XDocument.Parse(genericInterfaceSourceFile));
            CSharpFullMemberFormatter formatter = new CSharpFullMemberFormatter(map);

            string actual = formatter.GetDeclaration(typedef.Resolve().Methods.Single(t => t.Name == "Ok"));

            Assert.AreEqual("public System.Collections.Generic.IList<System.Collections.Generic.IList<int>> Ok (System.Collections.Generic.IList<System.Collections.Generic.IList<System.Collections.Generic.IList<int>>> p);", actual);
        }

        [Test]
        public void ReplaceInFormatter_Generic_Method_VB()
        {
            var typedef = GetTypeDef<UWPProjection>();

            var map = TypeMap.FromXDocument(XDocument.Parse(genericInterfaceSourceFile));
            VBMemberFormatter formatter = new VBMemberFormatter(map);

            string actual = formatter.GetDeclaration(typedef.Resolve().Methods.Single(t => t.Name == "Ok"));

            Assert.AreEqual("Public Function Ok (p As IList(Of IList(Of IList(Of Integer)))) As IList(Of IList(Of Integer))", actual);
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

        string genericInterfaceSourceFile = @"<TypeMap>
<InterfaceReplace From=""mdoc.Test.IInt`1"" To=""System.Collections.Generic.IList`1"" Langs=""C#;VB.NET;F#"">
        <Members>
        </Members>
    </InterfaceReplace>
</TypeMap>";

        public class UWPProjection : IInt<int>
        {
            public void Dispose(){}

            public IInt<int> Thing { get; }

            public IInt<IInt<int>> Ok (IInt<IInt<IInt<int>>> p) { return null; }
        }
    }

    public interface IInt<T>
    {
        IInt<T> Thing { get; }
    }
}