using NUnit.Framework;
using System;
using System.Linq;
using System.Xml;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Documentation.Updater;
using Mono.Documentation.Updater.Frameworks;

namespace mdoc.Test
{
    /// <summary>
    /// This is for testing the DocumentationEnumerator
    /// </summary>
    [TestFixture ()]
    public class EnumeratorTests : CecilBaseTest
    {


        [Test]
        public void FindProperty_NonEII () => TestProperty ("AProperty");

        [Test]
        public void FindProperty_EII1 () => TestProperty ("mdoc.Test.EnumeratorTests.IFace1.AProperty");

        [Test]
        public void FindProperty_EII2 () => TestProperty ("mdoc.Test.EnumeratorTests.IFace2.AProperty");

        [Test]
        public void FindProperty_NonExistent ()
        {
            string propertyName = "mdoc.Test.EnumeratorTests.IDontExist.AProperty";

            TypeDefinition theclass = GetTypeDef<ConcreteClass> ();

            var members = DocumentationEnumerator.GetReflectionMembers (theclass, propertyName, "Property").ToArray ();


            Assert.IsFalse (members.Any (), "Invalid Member Matched");
        }

        [Test]
        public void GetMember () => TestPropertyMember ("AProperty", XML_PROPERTY);

        [Test]
        public void GetMember_EII1 () => TestPropertyMember ("mdoc.Test.EnumeratorTests.IFace1.AProperty", XML_PROPERTY_IFACE1);

        [Test]
        public void GetMember_EII2 () => TestPropertyMember ("mdoc.Test.EnumeratorTests.IFace2.AProperty", XML_PROPERTY_IFACE2);

        [Test]
        public void GetMethod() => TestMethodMember("BeginRead", XML_METHOD_TESTMETHOD);

        [Test]
        public void MergeDiffReturnTypes_MatchExplicitConversion_String() => testReturnType("op_Explicit", "System.String");

        [Test]
        public void MergeDiffReturnTypes_MatchExplicitConversion_CharArray() => testReturnType("op_Explicit", "System.Char[]");

        [Test]
        public void MergeDiffReturnTypes_MatchExplicitConversion_Int() => testReturnType("op_Implicit", "System.Int32");
        [Test]
        public void MergeDiffReturnTypes_MatchExplicitConversion_IntArray() => testReturnType("op_Implicit", "System.Int32[]");

        #region Test infrastructure

        private void testReturnType(string methName, string r)
        {
            TypeDefinition theclass = GetTypeDef<ConcreteClass>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(@"<Member MemberName="""+ methName +@""">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>" + r + @"</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name=""value"" Type=""mdoc.Test.EnumeratorTests+ConcreteClass"" />
      </Parameters>
    </Member>");

            DocumentationMember docmember = new DocumentationMember(doc.DocumentElement, typeEntry: null);
            var result = DocumentationEnumerator.GetMember(theclass, docmember) as MethodReference;

            Assert.IsNotNull(result);
            Assert.AreEqual(methName, result.Name);
            Assert.AreEqual(r, result.ReturnType.FullName);
        }
        private void TestProperty (string propertyName)
        {
            TypeDefinition theclass = GetTypeDef<ConcreteClass> ();

            var members = DocumentationEnumerator.GetReflectionMembers (theclass, propertyName, "Property").ToArray ();

            Assert.IsTrue (members.Any (), "no members found");
            Assert.AreEqual (1, members.Count (), "Different # of members found");
            Assert.AreEqual (propertyName, members.Single ().Name);
        }

        private void TestPropertyMember (string propertyName, string theXml)
        {
            TypeDefinition theclass = GetTypeDef<ConcreteClass> ();

            XmlDocument document = new XmlDocument ();
            document.LoadXml (theXml);

            var member = DocumentationEnumerator.GetMember (theclass, new DocumentationMember (document.FirstChild, FrameworkTypeEntry.Empty));

            Assert.NotNull (member, "didn't find the node");
            Assert.AreEqual (propertyName, member.Name);
        }

        private void TestMethodMember(string methodName, string theXml)
        {
            TypeDefinition theclass = GetTypeDef<ConcreteClass>();

            XmlDocument document = new XmlDocument();
            document.LoadXml(theXml);

            var member = DocumentationEnumerator.GetMember(theclass, new DocumentationMember(document.FirstChild, FrameworkTypeEntry.Empty));

            Assert.NotNull(member, "didn't find the node");
            Assert.AreEqual(methodName, member.Name);
        }

        #endregion

        #region Test Types

        public interface IFace1
        {
            string AProperty { get; set; }
        }

        public interface IFace2
        {
            string AProperty { get; set; }
        }

        public class ConcreteClass : IFace1, IFace2
        {
            public string AProperty { get; set; }
            string IFace1.AProperty { get; set; }
            string IFace2.AProperty { get; set; }
            public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState) => null;

            public static explicit operator string(ConcreteClass value) => value.AProperty;
            public static explicit operator char[](ConcreteClass value) => value.AProperty.ToCharArray();
            public static implicit operator int(ConcreteClass value) => value.AProperty.Length;
            public static implicit operator int[](ConcreteClass value) => value.AProperty.ToCharArray().Select(c => (int)c).ToArray();
        }

        #endregion

        #region Test Data

        private string XML_PROPERTY = @"<Member MemberName=""AProperty"">
      <MemberType>Property</MemberType>
      <Implements>
        <InterfaceMember>P:mdoc.Test.EnumeratorTests.IFace1.AProperty</InterfaceMember>
        <InterfaceMember>P:mdoc.Test.EnumeratorTests.IFace2.AProperty</InterfaceMember>
      </Implements>
      <ReturnValue>
        <ReturnType>System.String</ReturnType>
      </ReturnValue>
    </Member>";

        private string XML_PROPERTY_IFACE1 = @"<Member MemberName=""mdoc.Test.EnumeratorTests.IFace1.AProperty"">
      <MemberType>Property</MemberType>
      <Implements>
        <InterfaceMember>P:mdoc.Test.EnumeratorTests.IFace1.AProperty</InterfaceMember>
      </Implements>
      <ReturnValue>
        <ReturnType>System.String</ReturnType>
      </ReturnValue>
    </Member>";

        private string XML_PROPERTY_IFACE2 = @"<Member MemberName=""mdoc.Test.EnumeratorTests.IFace2.AProperty"">
      <MemberType>Property</MemberType>
      <Implements>
        <InterfaceMember>P:mdoc.Test.EnumeratorTests.IFace2.AProperty</InterfaceMember>
      </Implements>
      <ReturnValue>
        <ReturnType>System.String</ReturnType>
      </ReturnValue>
    </Member>";

        private string XML_METHOD_TESTMETHOD = @"<Member MemberName=""BeginRead"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.IAsyncResult</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""buffer"" Type=""System.Byte[]"" Index=""0"" FrameworkAlternate=""netcore-1.0;netcore-1.1;netcore-2.0"" />
        <Parameter Name = ""array"" Type=""System.Byte[]"" Index=""0"" FrameworkAlternate=""netframework-4.5;netframework-4.5.1;netframework-4.5.2;netframework-4.6;netframework-4.6.1;netframework-4.6.2;netframework-4.7;netstandard-2.0"" />
        <Parameter Name = ""offset"" Type=""System.Int32"" Index=""1"" FrameworkAlternate=""netcore-2.0;netframework-4.5;netframework-4.5.1;netframework-4.5.2;netframework-4.6;netframework-4.6.1;netframework-4.6.2;netframework-4.7;netstandard-2.0"" />
        <Parameter Name = ""count"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""netcore-2.0;netframework-4.5;netframework-4.5.1;netframework-4.5.2;netframework-4.6;netframework-4.6.1;netframework-4.6.2;netframework-4.7;netstandard-2.0"" />
        <Parameter Name = ""asyncCallback"" Type=""System.AsyncCallback"" Index=""3"" FrameworkAlternate=""netcore-2.0;netframework-4.5;netframework-4.5.1;netframework-4.5.2;netframework-4.6;netframework-4.6.1;netframework-4.6.2;netframework-4.7;netstandard-2.0"" />
        <Parameter Name = ""asyncState"" Type=""System.Object"" Index=""4"" FrameworkAlternate=""netcore-2.0;netframework-4.5;netframework-4.5.1;netframework-4.5.2;netframework-4.6;netframework-4.6.1;netframework-4.6.2;netframework-4.7;netstandard-2.0"" />
      </Parameters>
    </Member>";

        #endregion
    }
}
