using System;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using Mono.Documentation;
using Mono.Documentation.Updater;
using Mono.Cecil;

namespace mdoc.Test
{
    [TestFixture]
    public class ExtensionMemberTests : BasicTests
    {
        [Test]
        public void ExtensionMembersTestLibrary_HasExtensionMethods()
        {
            // Use the existing test assembly that contains our SampleClasses
            var extensionType = GetType(typeof(mdoc.Test.SampleClasses.ExtensionMembersExample));

            Assert.IsNotNull(extensionType, "ExtensionMembersExample class should exist");

            // Verify extension methods are detected correctly
            var extensionMethods = extensionType.Methods.Where(DocUtils.IsExtensionMethod).ToList();
            Assert.IsTrue(extensionMethods.Count >= 2, $"Should have at least 2 extension methods, found {extensionMethods.Count}");

            // Verify specific extension methods exist
            var getDisplayNameMethod = extensionMethods.FirstOrDefault(m => m.Name == "GetDisplayName");
            Assert.IsNotNull(getDisplayNameMethod, "GetDisplayName extension method should exist");

            var setNameAndValueMethod = extensionMethods.FirstOrDefault(m => m.Name == "SetNameAndValue");
            Assert.IsNotNull(setNameAndValueMethod, "SetNameAndValue extension method should exist");
        }

        [Test]
        public void ExtensionMemberDetection_WithNullValues_ReturnsfalseGracefully()
        {
            // Test null safety of our new extension member detection methods
            Assert.IsFalse(DocUtils.IsExtensionProperty(null));
            Assert.IsFalse(DocUtils.IsExtensionIndexer(null));
            Assert.IsFalse(DocUtils.IsExtensionOperator(null));
        }

        [Test]
        public void ExtensionMemberDetection_WithRegularMembers_ReturnsFalse()
        {
            // Test that regular members are not detected as extension members
            var someClass = GetType(typeof(mdoc.Test.SampleClasses.SomeClass));

            // Test regular property
            var regularProperty = someClass.Properties.FirstOrDefault(p => p.Name == "Property");
            if (regularProperty != null)
            {
                Assert.IsFalse(DocUtils.IsExtensionProperty(regularProperty));
                Assert.IsFalse(DocUtils.IsExtensionIndexer(regularProperty));
            }

            // Test regular method
            var regularMethod = someClass.Methods.FirstOrDefault(m => m.Name == "get_Method");
            if (regularMethod != null)
            {
                Assert.IsFalse(DocUtils.IsExtensionMethod(regularMethod));
                Assert.IsFalse(DocUtils.IsExtensionOperator(regularMethod));
            }
        }

        [Test]
        public void ExtensionMemberDetection_WithExtensionMethods_WorksCorrectly()
        {
            // Integration test that verifies extension member detection works
            var extensionType = GetType(typeof(mdoc.Test.SampleClasses.ExtensionMembersExample));

            // Test extension method detection
            var extensionMethods = extensionType.Methods.Where(DocUtils.IsExtensionMethod).ToList();
            Assert.IsTrue(extensionMethods.Count >= 2, $"Should detect extension methods, found {extensionMethods.Count}");

            // Verify that extension attribute is properly detected
            foreach (var method in extensionMethods)
            {
                Assert.IsTrue(method.CustomAttributes.Any(attr =>
                    attr.AttributeType.FullName == "System.Runtime.CompilerServices.ExtensionAttribute"),
                    $"Extension method {method.Name} should have ExtensionAttribute");
            }
        }
    }
}
