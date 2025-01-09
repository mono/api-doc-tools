using System.IO;
using System.Xml;
using mdoc.Test;
using Mono.Documentation.Util;
using NUnit.Framework;

namespace Mono.Documentation.Updater.Tests
{
    [TestFixture]
    public class MsxdocDocumentationImporterTest : BasicTests
    {
        [Test]
        public void ImportDocumentation_SummaryOverwrite()
        {
            // Arrange
            var type = GetType(typeof(mdoc.Test2.InternalEIICalss));
            var member = type.GetMember("Getstring");
            var filePath = Path.Combine(Path.GetDirectoryName(this.GetType().Module.Assembly.Location), "SampleClasses\\testImportDoc2.xml");
            MsxdocDocumentationImporter importer = new MsxdocDocumentationImporter(filePath);
            var node = CreateXmlElement("<member><summary>Old summary</summary></member>");
            var info = new DocsNodeInfo(node, member);

            // Act
            importer.ImportDocumentation(info);

            // Assert
            var summaryNode = info.Node.SelectSingleNode("summary");
            Assert.IsNotNull(summaryNode);
            Assert.AreEqual("Extension methods for .", summaryNode.InnerText.Trim());
        }

        private XmlElement CreateXmlElement(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }
    }
}