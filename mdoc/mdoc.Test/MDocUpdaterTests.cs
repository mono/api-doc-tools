using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using mdoc.Test.SampleClasses;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;
using Mono.Documentation;
using Mono.Documentation.Updater;
using Mono.Documentation.Updater.Frameworks;
using NUnit.Framework;
using Cpp = Mono_DocTest_Generic;

namespace mdoc.Test
{
    [TestFixture]
    public class MDocUpdaterTests : BasicTests
    {
        readonly MDocUpdater updater = new MDocUpdater();

        [Test]
        public void Test_GetCustomAttributes_IgnoredObsoleteAttribute()
        {
            TypeDefinition testType = GetType(typeof(MDocUpdaterTests).Module.FullyQualifiedName, "System.Span`1");
            Collection<CustomAttribute> attributes = testType.CustomAttributes;

            IEnumerable<string> customAttributes = updater.GetCustomAttributes(attributes, "");

            Assert.AreEqual(1, attributes.Count);
            Assert.IsEmpty(customAttributes);
        }

        [Test]
        public void Test_GetDocParameterType_CppGenericParameterType_ReturnsTypeWithGenericParameters()
        {
            var method = GetMethod(typeof(Cpp.GenericBase<>), "BaseMethod2");

            string parameterType = MDocUpdater.GetDocParameterType(method.Parameters[0].ParameterType);

            Assert.AreEqual("Mono_DocTest_Generic.GenericBase<U>", parameterType);
        }
        [Test]
        public void Test_GetNamespace_IgnoredNamespaceGeneric()
        {
            TypeDefinition testType = GetType(typeof(ReadOnlySpan<>).Module.FullyQualifiedName, "mdoc.Test.SampleClasses.ReadOnlySpan`1");
            var ns = DocUtils.GetNamespace(testType.GenericParameters.First());
            Assert.AreEqual("", ns);
        }

        [Test]
        public void InterNalELLTest()
        {
            XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(XmlConsts.internalEllXml);

            MemberReference oldmember = null;
            var type = GetType(typeof(mdoc.Test2.InternalEIICalss));
            var docEnum = new DocumentationEnumerator();

            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;
            MethodInfo mInfo = typeof(MDocUpdater).GetMethod("IsMemberPublicEII", flags);

            foreach (DocsNodeInfo info in docEnum.GetDocumentationMembers(doc, type, FrameworkTypeEntry.Empty))
            {
                object[] parametors = new object[] { info.Member };
                var flag = (bool)mInfo.Invoke(null, parametors);

                if (!flag)
                {
                    oldmember = info.Member;
                    //Note : The following operation will not be carried out, just prompt
                      //-> DeleteMember();
                      //-> statisticsCollector.AddMetric();
                }
            }
            Assert.AreEqual("System.String mdoc.Test2.InternalEIICalss::mdoc.Test.SampleClasses.InterfaceA.Getstring(System.Int32)", oldmember.FullName);

        }
    }
}