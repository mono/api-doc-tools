using Mono.Documentation.Updater;
using NUnit.Framework;
using System;
using System.Linq;

namespace mdoc.Test.Enumeration
{
    [TestFixture ()]
    public class InterfaceTests : CecilBaseTest
    {

        [Test ()]
        public void TestCase ()
        {
            var type = GetTypeDef<TestClassWithInterfaces> ();
            var ifaces = DocUtils.GetAllPublicInterfaces (type).ToArray ();

             Assert.AreEqual (3, ifaces.Count ());
            Assert.AreEqual ("I2", ifaces[0].Name);
            Assert.AreEqual ("ICombined", ifaces[1].Name);
            Assert.AreEqual ("I1", ifaces[2].Name);
        }

        [Test ()]
        public void TestCase_WithComplexCombinations ()
        {
            var type = GetTypeDef<TestClassWithAllInterfaces> ();
            var ifaces = DocUtils.GetAllPublicInterfaces (type).ToArray ();

            Assert.AreEqual (4, ifaces.Count ());
            Assert.AreEqual ("ICombined2", ifaces[0].Name);
            Assert.AreEqual ("I1", ifaces[1].Name);
            Assert.AreEqual ("ICombined", ifaces[2].Name);
            Assert.AreEqual ("I2", ifaces[3].Name);
        }

        [Test ()]
        public void TestCase_OnlyDirectlyImplemented ()
        {
            var type = GetTypeDef<TestClassWithInterfaces> ();
            var ifaces = DocUtils.GetUserImplementedInterfaces (type).ToArray ();

            Assert.AreEqual (2, ifaces.Count ());
            Assert.AreEqual ("I2", ifaces[0].Name);
            Assert.AreEqual ("ICombined", ifaces[1].Name);
        }
    }

    public interface I1 {}
    public interface I2 {}
    public interface ICombined : I1 {}
    public interface ICombined2 : I1, ICombined, I2 {}
    public interface TestClassWithInterfaces : I2, ICombined {}
    public interface TestClassWithAllInterfaces : ICombined2 {}
}
