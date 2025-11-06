using Mono.Documentation.Updater;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture]
    public class DocUtilsFSharpTests : BasicFSharpFormatterTests<FSharpFormatter>
    {
        protected override FSharpFormatter formatter { get; }
        
        [Test]
        public void IsIgnored_GetMethodIsGeneratedByProperty_IsIgnoredTrue()
        {
            var method = GetMethod(typeof(DiscriminatedUnions.Shape.Circle),
                "get_" + nameof(DiscriminatedUnions.Shape.Circle.radius));

            var isIgnoredPropertyGeneratedMethod = DocUtils.IsIgnored(method);

            Assert.IsTrue(isIgnoredPropertyGeneratedMethod);
        }

        [Test]
        public void IsIgnored_GetMethodIsGeneratedByProperty2_IsIgnoredTrue()
        {
            var method = GetMethod(typeof(DiscriminatedUnions.SizeUnion),
                "get_" + nameof(DiscriminatedUnions.SizeUnion.Large));

            var isIgnoredPropertyGeneratedMethod = DocUtils.IsIgnored(method);

            Assert.IsTrue(isIgnoredPropertyGeneratedMethod);
        }

        [Test]
        public void IsIgnored_GetMethodIsNotGeneratedByProperty_IsIgnoredFalse()
        {
            var method = GetMethod(typeof(Functions),
                nameof(Functions.get_function));

            var isIgnoredPropertyGeneratedMethod = DocUtils.IsIgnored(method);

            Assert.IsFalse(isIgnoredPropertyGeneratedMethod);
        }
    }
}