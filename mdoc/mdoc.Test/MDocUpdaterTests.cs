using System.Collections.Generic;
using System.Linq;
using mdoc.Test.SampleClasses;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;
using Mono.Documentation;
using Mono.Documentation.Updater;
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
    }
}