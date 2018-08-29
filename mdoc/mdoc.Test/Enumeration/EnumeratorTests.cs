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

        #region Test infrastructure

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

        #endregion
    }
}
