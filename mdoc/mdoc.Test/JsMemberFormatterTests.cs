using mdoc.Mono.Documentation.Updater.Formatters;
using mdoc.Test.SampleClasses;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture]
    [Category("Javascript")]
    [Category("Usage")]
    class JsMemberFormatterTests : BasicFormatterTests<JsMemberFormatter>
    {
        protected override JsMemberFormatter formatter => new JsMemberFormatter();


        #region Methods
        [Test]
        [Category("Methods")]
        public void Test_AyncMethod()
            => TestMethodSignature(typeof(SomeClass),
                "function asyncMethod()",
                nameof(SomeClass.AsyncMethod));
        #endregion

        #region Types
        [Test]
        [Category("Types")]
        [Category("Constructors")]
        public void Test_Constructor_0()
            => TestTypeSignature(typeof(TestClass),
                "function TestClass()");

        [Test]
        [Category("Types")]
        [Category("Constructors")]
        public void Test_Constructor_1()
            => TestTypeSignature(typeof(SomeClass),
                "function SomeClass(i, j)");
        #endregion
    }
}