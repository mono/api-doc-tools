using Mono.Documentation.Updater;
using NUnit.Framework;
using System.Linq;
using System.Xml;

namespace mdoc.Test
{
    [TestFixture]
    public class DocumentationMemberTest
    {
        [Test]
        public void Test_GetTypeParametersFromXMLElements()
        {
            var actual = DocumentationMember.GetTypeParametersFromXMLElements(null);
            Assert.IsNull(actual);

            actual = DocumentationMember.GetTypeParametersFromXMLElements(new XmlElement[0]);
            Assert.IsNull(actual);

            var doc1 = new XmlDocument();
            doc1.LoadXml("<TypeParameters> " +
            "<TypeParameter Name = \"T\" Index = \"0\" FrameworkAlternate = \"net-5.0;net-6.0;netcore-3.0;netcore-3.1\" />" +
            "<TypeParameter Name = \"TFrom\" Index = \"0\" FrameworkAlternate = \"net-7.0\" />" +
            "<TypeParameter Name = \"U\" Index = \"1\" FrameworkAlternate = \"net-5.0;net-6.0;netcore-3.0;netcore-3.1\" />" +
            "<TypeParameter Name = \"TTo\" Index = \"1\" FrameworkAlternate = \"net-7.0\" />" +
            "</TypeParameters>");

            var tpElements = doc1.SelectNodes("TypeParameters/TypeParameter[not(@apistyle) or @apistyle='classic']").Cast<XmlElement>().ToArray();
            actual = DocumentationMember.GetTypeParametersFromXMLElements(tpElements);
            Assert.AreEqual(2, actual.Count);

            var doc2 = new XmlDocument();
            doc2.LoadXml("<TypeParameters> <TypeParameter Name = \"T\" /> <TypeParameter Name = \"U\" /><TypeParameter Name = \"V\" /></TypeParameters>");
            tpElements = doc2.SelectNodes("TypeParameters/TypeParameter[not(@apistyle) or @apistyle='classic']").Cast<XmlElement>().ToArray();
            actual = DocumentationMember.GetTypeParametersFromXMLElements(tpElements);
            Assert.AreEqual(3, actual.Count);
        }
    }
}
