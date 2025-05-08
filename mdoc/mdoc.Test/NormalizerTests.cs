using Mono.Documentation;
using NUnit.Framework;
using System;
using System.IO;

namespace mdoc.Test
{
    [TestFixture]
    public class NormalizerTest
    {
        [Test]
        public void Run_ValidXml_ShouldNormalize()
        {
            string validXmlPath = "valid.xml";
            string validXmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\\r\\n<root>\\r\\n</root>";
            File.WriteAllText(validXmlPath, validXmlContent);

            Normalizer.Run(new string[] { validXmlPath });

            string normalizedContent = File.ReadAllText(validXmlPath);
            Assert.AreEqual(validXmlContent, normalizedContent);

            File.Delete(validXmlPath);
        }

        [Test]
        public void Run_InvalidXml_ShouldPrintError()
        {
            string invalidXmlPath = "invalid.xml";
            File.WriteAllText(invalidXmlPath, "<invalidXml>");

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Normalizer.Run(new string[] { invalidXmlPath });
                Assert.IsTrue(sw.ToString().Contains("is not a wellformed XML document."));
            }
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            File.Delete(invalidXmlPath);
        }
    }
}
