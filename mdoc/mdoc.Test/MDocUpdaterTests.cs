using System.Collections.Generic;
using Mono.Cecil;
using Mono.Collections.Generic;
using Mono.Documentation;
using NUnit.Framework;

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
    }
}