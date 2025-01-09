using Mono.Documentation;
using NUnit.Framework;
using System.Text;
using System.Xml;

namespace mdoc.Test
{
    [TestFixture]
    public class DelegatingXmlWriterTests
    {
        private StringBuilder output;
        private XmlWriter baseWriter;
        private DelegatingXmlWriter delegatingWriter;

        [SetUp]
        public void SetUp()
        {
            output = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment
            };
            baseWriter = XmlWriter.Create(output, settings);
            delegatingWriter = new DelegatingXmlWriter(baseWriter);
        }

        [TearDown]
        public void TearDown()
        {
            delegatingWriter.Close();
        }

        [Test]
        public void TestWriteStartElement()
        {
            delegatingWriter.WriteStartElement("prefix", "localName", "namespace");
            delegatingWriter.WriteEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<prefix:localName xmlns:prefix=\"namespace\" />", output.ToString());
        }

        [Test]
        public void TestWriteString()
        {
            delegatingWriter.WriteStartElement("root");
            delegatingWriter.WriteString("content");
            delegatingWriter.WriteEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<root>content</root>", output.ToString());
        }

        [Test]
        public void TestWriteFullEndElement()
        {
            delegatingWriter.WriteStartElement("root");
            delegatingWriter.WriteFullEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<root></root>", output.ToString());
        }

        [Test]
        public void TestWriteCData()
        {
            delegatingWriter.WriteStartElement("root");
            delegatingWriter.WriteCData("cdata content");
            delegatingWriter.WriteEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<root><![CDATA[cdata content]]></root>", output.ToString());
        }

        [Test]
        public void TestWriteComment()
        {
            delegatingWriter.WriteComment("comment");
            delegatingWriter.Flush();

            Assert.AreEqual("<!--comment-->", output.ToString());
        }

        [Test]
        public void TestWriteProcessingInstruction()
        {
            delegatingWriter.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"style.xsl\"");
            delegatingWriter.Flush();

            Assert.AreEqual("<?xml-stylesheet type=\"text/xsl\" href=\"style.xsl\"?>", output.ToString());
        }

        [Test]
        public void TestWriteWhitespace()
        {
            delegatingWriter.WriteWhitespace("   ");
            delegatingWriter.Flush();

            Assert.AreEqual("   ", output.ToString());
        }

        [Test]
        public void TestWriteRaw()
        {
            delegatingWriter.WriteRaw("<raw>data</raw>");
            delegatingWriter.Flush();

            Assert.AreEqual("<raw>data</raw>", output.ToString());
        }

        [Test]
        public void TestWriteEntityRef()
        {
            delegatingWriter.WriteEntityRef("entity");
            delegatingWriter.Flush();

            Assert.AreEqual("&entity;", output.ToString());
        }

        [Test]
        public void TestWriteCharEntity()
        {
            delegatingWriter.WriteCharEntity('c');
            delegatingWriter.Flush();

            Assert.AreEqual("&#x63;", output.ToString());
        }
        [Test]
        public void TestWriteBase64()
        {
            byte[] buffer = Encoding.UTF8.GetBytes("base64 content");
            delegatingWriter.WriteStartElement("root");
            delegatingWriter.WriteBase64(buffer, 0, buffer.Length);
            delegatingWriter.WriteEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<root>YmFzZTY0IGNvbnRlbnQ=</root>", output.ToString());
        }

        [Test]
        public void TestWriteBinHex()
        {
            byte[] buffer = Encoding.UTF8.GetBytes("binhex content");
            delegatingWriter.WriteStartElement("root");
            delegatingWriter.WriteBinHex(buffer, 0, buffer.Length);
            delegatingWriter.WriteEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<root>62696E68657820636F6E74656E74</root>", output.ToString());
        }

        [Test]
        public void TestWriteChars()
        {
            char[] buffer = "char content".ToCharArray();
            delegatingWriter.WriteStartElement("root");
            delegatingWriter.WriteChars(buffer, 0, buffer.Length);
            delegatingWriter.WriteEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<root>char content</root>", output.ToString());
        }

        [Test]
        public void TestWriteStartAttribute()
        {
            delegatingWriter.WriteStartElement("root");
            delegatingWriter.WriteStartAttribute("prefix", "attrName", "namespace");
            delegatingWriter.WriteString("attrValue");
            delegatingWriter.WriteEndAttribute();
            delegatingWriter.WriteEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<root prefix:attrName=\"attrValue\" xmlns:prefix=\"namespace\" />", output.ToString());
        }

        [Test]
        public void TestWriteQualifiedName()
        {
            delegatingWriter.WriteStartElement("root");
            delegatingWriter.WriteAttributeString("xmlns", "namespace", null, "namespace");
            delegatingWriter.WriteQualifiedName("qualifiedName", "namespace");
            delegatingWriter.WriteEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<root xmlns:namespace=\"namespace\">namespace:qualifiedName</root>", output.ToString());
        }

        [Test]
        public void TestWriteSurrogateCharEntity()
        {
            delegatingWriter.WriteStartElement("root");
            delegatingWriter.WriteSurrogateCharEntity('\uDC00', '\uD800');
            delegatingWriter.WriteEndElement();
            delegatingWriter.Flush();

            Assert.AreEqual("<root>&#x10000;</root>", output.ToString());
        }
    }
}
