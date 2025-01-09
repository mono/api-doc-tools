using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Mono.Documentation;
using Mono.Documentation.Framework;
using Mono.Documentation.Updater.Frameworks;
using Mono.Documentation.Util;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture]
    public class MDocFileSourceTests : BasicTests
    {
        #region DropExcludedFrameworks
        [Test]
        public void DropExcludedFrameworks_NoNodesWithFrameworkAlternate_NoChangesInXml()
        {
            XDocument document = ReadXDocument(XmlConsts.NormalSingleXml2);
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");

            mdocFileSource.DropExcludedFrameworks(document);

            Assert.AreEqual(NormalizeXml(XmlConsts.NormalSingleXml2), NormalizeXml(document.ToString()));
        }

        [Test]
        public void DropExcludedFrameworks_FrameworkAlternateWithManyFrameworks_FrameworkOneLeft()
        {
            XDocument document = ReadXDocument(XmlConsts.MultiFrameworkXml2);
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");

            mdocFileSource.DropExcludedFrameworks(document);

            Assert.AreEqual(NormalizeXml(MultiFrameworkXml2FrameworkOne), NormalizeXml(document.ToString()));
        }

        [Test]
        public void DropExcludedFrameworks_FrameworkAlternateWithOneFramework_FrameworkOneLeft()
        {
            XDocument document = ReadXDocument(XmlConsts.MultiFrameworkXml2Second);
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");

            mdocFileSource.DropExcludedFrameworks(document);

            Assert.AreEqual(NormalizeXml(MultiFrameworkXml2SecondFrameworkTwo), NormalizeXml(document.ToString()));
        }

        [Test]
        public void DropExcludedFrameworks_NoFrameworkModeFrameworkAlternateWithManyFrameworks_NoChangesInXml()
        {
            XDocument document = ReadXDocument(XmlConsts.MultiFrameworkXml2);
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, null);

            mdocFileSource.DropExcludedFrameworks(document);

            Assert.AreEqual(NormalizeXml(XmlConsts.MultiFrameworkXml2), NormalizeXml(document.ToString()));
        }

        [Test]
        public void DropExcludedFrameworks_NoFrameworkModeFrameworkAlternateWithOneFramework_NoChangesInXml()
        {
            XDocument document = ReadXDocument(XmlConsts.MultiFrameworkXml2Second);
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, null);

            mdocFileSource.DropExcludedFrameworks(document);

            Assert.AreEqual(NormalizeXml(XmlConsts.MultiFrameworkXml2Second), NormalizeXml(document.ToString()));
        }

        #endregion

        #region DropExcludedFrameworksFromIndex

        [Test]
        public void DropExcludedFrameworksFromIndex_Type1InFrameworkType2IsNot_Type2IsExcluded()
        {
            XDocument document = ReadXDocument(IndexFileXml);
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");
            Dictionary<string, FrameworkNamespaceModel> frameworkIndex = GetFrameworkIndex(XmlConsts.FrameworkIndexXml);

            mdocFileSource.DropExcludedFrameworksFromIndex(document, frameworkIndex, (ns, type) =>
            {
                switch (type)
                {
                    case "Type1":
                        return ReadXDocument(Type1Xml);
                    case "Type2":
                        return ReadXDocument(Type2Xml);
                }
                throw new Exception("Error in test data.");
            });

            Assert.AreEqual(NormalizeXml(IndexFileFilteredByFrameworkXml), NormalizeXml(document.ToString()));
        }

        [Test]
        public void DropExcludedFrameworksFromIndex_Type1AndType2InFramework_NoExcludedTypes()
        {
            XDocument document = ReadXDocument(IndexFileXml);
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");
            Dictionary<string, FrameworkNamespaceModel> frameworkIndex = GetFrameworkIndex(XmlConsts.FrameworkIndexXml2);

            mdocFileSource.DropExcludedFrameworksFromIndex(document, frameworkIndex, (ns, type) =>
            {
                switch (type)
                {
                    case "Type1":
                        return ReadXDocument(Type1Xml);
                    case "Type2":
                        return ReadXDocument(Type2Xml);
                }
                throw new Exception("Error in test data.");
            });

            Assert.AreEqual(NormalizeXml(IndexFileXml), NormalizeXml(document.ToString()));
        }


        [Test]
        public void DropExcludedFrameworksFromIndex_NoFrameworkModeType1InFrameworkType2IsNot_NoChangesInXml()
        {
            XDocument document = ReadXDocument(IndexFileXml);
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, null);
            Dictionary<string, FrameworkNamespaceModel> frameworkIndex = GetFrameworkIndex(XmlConsts.FrameworkIndexXml);

            mdocFileSource.DropExcludedFrameworksFromIndex(document, frameworkIndex, (ns, type) =>
            {
                switch (type)
                {
                    case "Type1":
                        return ReadXDocument(Type1Xml);
                    case "Type2":
                        return ReadXDocument(Type2Xml);
                }
                throw new Exception("Error in test data.");
            });

            Assert.AreEqual(NormalizeXml(IndexFileXml), NormalizeXml(document.ToString()));
        }
        #endregion

        #region IsTypeInFramework

        [Test]
        public void IsTypeInFramework_TypeIsInFramework_ReturnsTrue()
        {
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");
            Dictionary<string, FrameworkNamespaceModel> frameworkIndex = GetFrameworkIndex(XmlConsts.FrameworkIndexXml);
            XDocument typeDoc = ReadXDocument(Type1Xml);

            string nsName = "Namespace1";
            bool isTypeInFramework = mdocFileSource.IsTypeInFramework(typeDoc, nsName, frameworkIndex);

            Assert.IsTrue(isTypeInFramework);
        }

        [Test]
        public void IsTypeInFramework_NoNamespaceInFramework_ReturnsFalse()
        {
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");
            Dictionary<string, FrameworkNamespaceModel> frameworkIndex = GetFrameworkIndex(XmlConsts.FrameworkIndexXml);
            XDocument typeDoc = ReadXDocument(Type1Xml);

            string nsName = "Namespace3";
            bool isTypeInFramework = mdocFileSource.IsTypeInFramework(typeDoc, nsName, frameworkIndex);

            Assert.IsFalse(isTypeInFramework);
        }

        [Test]
        public void IsTypeInFramework_TypeIsNotInFramework_ReturnsFalse()
        {
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");
            Dictionary<string, FrameworkNamespaceModel> frameworkIndex = GetFrameworkIndex(XmlConsts.FrameworkIndexXml);
            XDocument typeDoc = ReadXDocument(Type3Xml);

            string nsName = "Namespace1";
            bool isTypeInFramework = mdocFileSource.IsTypeInFramework(typeDoc, nsName, frameworkIndex);

            Assert.IsFalse(isTypeInFramework);
        }
        #endregion

        #region IsExcludedFramework
        [Test]
        public void IsExcludedFramework_NoFrameworkAlternate_ReturnsFalse()
        {
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");
            var member = XElement.Parse(@"<Member></Member>");

            var isExcludedFramework = mdocFileSource.IsExcludedFramework(member);

            Assert.IsFalse(isExcludedFramework);
        }

        [Test]
        public void IsExcludedFramework_FrameworkAlternateIncludesFrameworkName_ReturnsFalse()
        {
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");
            var member = XElement.Parse(@"<Member FrameworkAlternate=""One""></Member>");

            var isExcludedFramework = mdocFileSource.IsExcludedFramework(member);

            Assert.IsFalse(isExcludedFramework);
        }

        [Test]
        public void IsExcludedFramework_FrameworkAlternateIncludesFrameworkNameInManyValues_ReturnsFalse()
        {
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");
            var member = XElement.Parse(@"<Member FrameworkAlternate=""One;Two""></Member>");

            var isExcludedFramework = mdocFileSource.IsExcludedFramework(member);

            Assert.IsFalse(isExcludedFramework);
        }

        [Test]
        public void IsExcludedFramework_FrameworkAlternateDoesntIncludeFrameworkName_ReturnsTrue()
        {
            var mdocFileSource = new MDocFileSource(null, ApiStyle.Classic, "One");
            var member = XElement.Parse(@"<Member FrameworkAlternate=""Two""></Member>");

            var isExcludedFramework = mdocFileSource.IsExcludedFramework(member);

            Assert.IsTrue(isExcludedFramework);
        }
        #endregion

        #region Helper 
        private Dictionary<string, FrameworkNamespaceModel> GetFrameworkIndex(string frameworkIndexXml)
        {
            return FrameworkIndexHelper.ReadFrameworkIndex(ReadXDocument(frameworkIndexXml).CreateReader());
        }
        #endregion

        #region Consts
        private const string MultiFrameworkXml2FrameworkOne = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""b"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One;Two"" />
        <Parameter Name = ""c"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""One;Two"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        private const string MultiFrameworkXml2SecondFrameworkTwo = @"<Member MemberName=""Meth"">
      <MemberType>Method</MemberType>
      <ReturnValue>
        <ReturnType>System.Void</ReturnType>
      </ReturnValue>
      <Parameters>
        <Parameter Name = ""a"" Type=""System.Int32"" Index=""0"" FrameworkAlternate=""One"" />
        <Parameter Name = ""b"" Type=""System.String"" Index=""1"" FrameworkAlternate=""One"" />
        <Parameter Name = ""c"" Type=""System.Int32"" Index=""2"" FrameworkAlternate=""One"" />
      </Parameters>
      <Docs>
        <summary>To be added.</summary>
        <remarks>To be added.</remarks>
      </Docs>
    </Member>";

        private const string IndexFileXml = @"<Overview>
	<Assemblies>
		<Assembly Name=""Assembly.14.0"" Version=""14.0.0.0""></Assembly>
	</Assemblies>
	<Remarks>To be added.</Remarks>
	<Copyright>To be added.</Copyright>
	<Types>
		<Namespace Name=""Namespace1"">
			<Type Name=""Type1"" Kind=""Class"" />
		</Namespace>
		<Namespace Name=""Namespace2"">
			<Type Name=""Type2"" Kind=""Interface"" />
		</Namespace>
	</Types>
	<Title>Untitled</Title>
</Overview>";

        private const string IndexFileFilteredByFrameworkXml = @"<Overview>
	<Assemblies>
		<Assembly Name=""Assembly.14.0"" Version=""14.0.0.0""></Assembly>
	</Assemblies>
	<Remarks>To be added.</Remarks>
	<Copyright>To be added.</Copyright>
	<Types>
		<Namespace Name=""Namespace1"">
			<Type Name=""Type1"" Kind=""Class"" />
		</Namespace>
	</Types>
	<Title>Untitled</Title>
</Overview>";

        private const string Type1Xml = @"<Type Name="""" FullName="""">
  <TypeSignature Language=""DocId"" Value=""T:Type1"" />
</Type>
";

        private const string Type2Xml = @"<Type Name="""" FullName="""">
  <TypeSignature Language=""DocId"" Value=""T:Type2"" />
</Type>
";

        private const string Type3Xml = @"<Type Name="""" FullName="""">
  <TypeSignature Language=""DocId"" Value=""T:Type3"" />
</Type>
";
        #endregion

        [Test]
        public void GetIndexReader_ShouldModifyDocumentCorrectly()
        {
            // Arrange
            var testPath = "frameworks";
            var testFrameworkPath = Path.Combine(testPath, "FrameworksIndex", "TestFramework.xml");
            Directory.CreateDirectory(Path.GetDirectoryName(testFrameworkPath));
            File.WriteAllText(testFrameworkPath, XmlConsts.FrameworkIndexXml);
            var testTypePath = Path.Combine(testPath, "TestNamespace.TestNamespace", "TestType.xml");
            Directory.CreateDirectory(Path.GetDirectoryName(testTypePath));
            File.WriteAllText(testTypePath, XmlConsts.CheckDocidXml);
            string xmlContent = @"<root>
                <see cref='M:System.String.TestMethod(System.String)' />
                <members>
                    <Member MemberName='TestMethod'>
                        <see cref='T:Namespace.Type'/>
                        <Parameters>
                            <Parameter Type='System.String' Name='param1' apistyle='classic'/>
                        </Parameters>
                    </Member>
                </members>
            </root>";
            string stringXml = Path.Combine(testPath, "System", "String.xml");
            string stringXmlContent = @"<Type Name=""System.String"" FullName=""System.String"">
                  <TypeSignature Language=""DocId"" Value=""T:System.String"" /></Type>";
            Directory.CreateDirectory(Path.GetDirectoryName(stringXml));
            File.WriteAllText(stringXml, stringXmlContent);
            string path = Path.Combine(testPath, "test.xml");
            File.WriteAllText(path, xmlContent);

            var fileSource = new MDocFileSource("TestNamespace", ApiStyle.Classic, "framework1");
            fileSource.ReplaceNativeTypes = true;

            // Act
            using (var reader = fileSource.GetIndexReader(path))
            {
                var doc = XDocument.Load(reader);

                // Assert
                Assert.IsNull(doc.NextNode);
            }

            // Cleanup
            Directory.Delete(testPath, true);
        }

        [Test]
        public void GetNamespaceElement_ShouldReturnModifiedNamespaceElement()
        {
            // Arrange
            string path = "test.xml";
            string xmlContent = @"
                <Namespace>
                    <Type Name='TestNamespace.TestClass'>
                        <Member MemberName='TestMethod'>
                            <Parameters>
                                <Parameter Type='TestNamespace.TestType' />
                            </Parameters>
                        </Member>
                    </Type>
                </Namespace>";
            File.WriteAllText(path, xmlContent);

            var fileSource = new MDocFileSource("TestNamespace", ApiStyle.Classic, null);

            // Act
            XElement result = fileSource.GetNamespaceElement(path);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Namespace", result.Name.LocalName);

            // Clean up
            File.Delete(path);
        }

        [Test]
        public void GetTypeXmlPath_ShouldReturnCorrectPath_WhenTypeIsDropped()
        {
            // Arrange
            var fileSource = new MDocFileSource("DroppedNamespace", ApiStyle.Classic, "TestFramework");
            string basePath = "basePath";
            string nsName = "Namespace";
            string typeName = "TypeName";

            // Act
            string result = fileSource.GetTypeXmlPath(basePath, nsName, typeName);

            // Assert
            string expectedResult = "basePath/DroppedNamespace.Namespace/TypeName.xml";
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetNamespaceXmlPath_ShouldReturnCorrectPath_WhenNamespaceIsDropped()
        {
            // Arrange
            string basePath = "basePath";
            string ns = "TestNamespace";
            string droppedNamespace = "DroppedNamespace";
            var fileSource = new MDocFileSource(droppedNamespace, ApiStyle.Classic, null);

            // Act
            string result = fileSource.GetNamespaceXmlPath(basePath, ns);

            // Assert
            string expectedPath = Path.Combine(basePath, $"ns-{droppedNamespace}.{ns}.xml");
            Assert.AreEqual(expectedPath, result);
        }
    }
}