using mdoc.Test.SampleClasses;
using Mono.Documentation.Updater;
using Mono.Documentation.Updater.Frameworks;
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
            Assert.IsTrue(doc.FirstChild.ChildNodes.Count == 3);
        }

        [Test]
        public void TestNodeCleaner2()
        {
            string xml = @"<Docs>
    <summary>To be added.</summary>
    <param name=""one"" name2=""n2"">To be added.</param>
<!-- this is an xml comment -->
    <param name=""two"">written docs</param>
random text
<param name=""three"">Written but not provided</param>
<![CDATA[
  for some reason
]]>
</Docs>"; string xml2 = @"<Docs>
    <summary>new summary</summary>
<!-- this is another xml comment -->
    <param name=""one"" name2=""n2"">To be added.</param>
    random text
    <param name=""two"">written docs but changed</param>
<param name=""three"">Written but and</param>
<![CDATA[
  for some reason
]]>
</Docs>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlDocument incomingDoc = new XmlDocument();
            incomingDoc.LoadXml(xml2);
            DocUtils.ClearNodesIfNotDefault(doc.FirstChild, incomingDoc.FirstChild);
            Assert.IsTrue(doc.FirstChild.ChildNodes.Count == 0);
        }

        [Test]
        public void DocidCheck()
        {
            XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(XmlConsts.CheckDocidXml);

            //c# MemberSignature is the same,but docid not
            var listA = doc.SelectNodes("/Type/Members/Member[@MemberName='op_Implicit']");
            if (listA.Count == 2)
            {
                var Notequal = DocUtils.DocIdCheck(listA[0], (XmlElement)listA[1]);
                Assert.IsTrue(Notequal);
            }

            //note:c not have docid item in xml
            var b = doc.SelectSingleNode("/Type/Members/Member[@MemberName='op_Implicit']");
            var c = doc.SelectSingleNode("/Type/Members/Member[@MemberName='.ctor']");

            var flg1 = DocUtils.DocIdCheck(b, (XmlElement)c);
            Assert.IsFalse(flg1);

            //Parameter change position
            var flg2 = DocUtils.DocIdCheck(c, (XmlElement)b);
            Assert.IsFalse(flg2);

            // c# MemberSignature is not same,docid also
            var d = doc.SelectSingleNode("/Type/Members/Member[@MemberName='Value']");
            var flg3 = DocUtils.DocIdCheck(b, (XmlElement)d);
            Assert.IsTrue(flg3);
        }
    }
}