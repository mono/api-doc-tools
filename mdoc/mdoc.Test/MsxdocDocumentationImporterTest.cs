using System.Xml;
using NUnit.Framework;

namespace Mono.Documentation.Updater.Tests
{
    [TestFixture]
    public class MsxdocDocumentationImporterTest
    {
        [Test]
        public void ImportDocumentation_SummaryOverwrite()
        {
            // Arrange
            var importer = new MsxdocDocumentationImporter("path/to/test.xml");
            var node = CreateXmlElement("<member><summary>Old summary</summary></member>");
            var info = new DocsNodeInfo(node);

            // Act
            importer.ImportDocumentation(info);

            // Assert
            var summaryNode = info.Node.SelectSingleNode("summary");
            Assert.IsNotNull(summaryNode);
            Assert.AreEqual("New summary", summaryNode.InnerText);
        }

        [Test]
        public void ImportDocumentation_RemarksOverwrite()
        {
            // Arrange
            var importer = new MsxdocDocumentationImporter("path/to/test.xml");
            var node = CreateXmlElement("<member><remarks>Old remarks</remarks></member>");
            var info = new DocsNodeInfo(node);

            // Act
            importer.ImportDocumentation(info);

            // Assert
            var remarksNode = info.Node.SelectSingleNode("remarks");
            Assert.IsNotNull(remarksNode);
            Assert.AreEqual("New remarks", remarksNode.InnerText);
        }

        [Test]
        public void ImportDocumentation_ParamOverwrite()
        {
            // Arrange
            var importer = new MsxdocDocumentationImporter("path/to/test.xml");
            var node = CreateXmlElement("<member><param name=\"param1\">Old param</param></member>");
            var info = new DocsNodeInfo(node);

            // Act
            importer.ImportDocumentation(info);

            // Assert
            var paramNode = info.Node.SelectSingleNode("param[@name='param1']");
            Assert.IsNotNull(paramNode);
            Assert.AreEqual("New param", paramNode.InnerText);
        }

        private XmlElement CreateXmlElement(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }
    }
}