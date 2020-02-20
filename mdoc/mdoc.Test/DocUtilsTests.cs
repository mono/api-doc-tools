using mdoc.Test.SampleClasses;
using Mono.Documentation.Updater;
using NUnit.Framework;
using System.Xml;

namespace mdoc.Test
{
    [TestFixture]
    public class DocUtilsTests : BasicTests
    {
        [Test]
        public void IsIgnored_MethodGeneratedByProperty_IsIgnoredTrue()
        {
            var method = GetMethod(typeof(SomeClass), "get_" + nameof(SomeClass.Property));

            var isIgnoredPropertyGeneratedMethod = DocUtils.IsIgnored(method);

            Assert.True(isIgnoredPropertyGeneratedMethod);
        }

        [Test]
        public void IsIgnored_GetMethodIsNotGeneratedByProperty_IsIgnoredFalse()
        {
            var method = GetMethod(typeof(SomeClass), nameof(SomeClass.get_Method));

            var isIgnoredPropertyGeneratedMethod = DocUtils.IsIgnored(method);

            Assert.False(isIgnoredPropertyGeneratedMethod);
        }

        [Test]
        public void IsIgnored_GetMethodInIterfaceIsGeneratedByProperty_IsIgnoredTrue()
        {
            var method = GetMethod(typeof(SomeInterface), "get_B");

            var isIgnoredPropertyGeneratedMethod = DocUtils.IsIgnored(method);

            Assert.IsTrue(isIgnoredPropertyGeneratedMethod);
        }

        [Test]
        public void IsIgnored_SetMethodIsGeneratedByProperty_IsIgnoredTrue()
        {
            var method = GetMethod(typeof(SomeClass), "set_" +nameof(SomeClass.Property4));

            var isIgnoredPropertyGeneratedMethod = DocUtils.IsIgnored(method);

            Assert.IsTrue(isIgnoredPropertyGeneratedMethod);
        }

        [Test]
        public void TestNodeCleaner()
        {
            string xml = @"<Docs>
    <summary>To be added.</summary>
    <param name=""one"">To be added.</param>
    <param name=""two"">written docs</param>
<param name=""three"">Written but not provided</param>
</Docs>"; string xml2 = @"<Docs>
    <param name=""two"">written docs</param>
</Docs>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlDocument incomingDoc = new XmlDocument();
            incomingDoc.LoadXml(xml2);
            DocUtils.ClearNodesIfNotDefault(doc.FirstChild, incomingDoc.FirstChild);
            Assert.IsTrue(doc.FirstChild.ChildNodes.Count == 3) ;
        }
    }
}