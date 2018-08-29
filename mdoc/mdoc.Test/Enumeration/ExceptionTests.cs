using Mono.Documentation;
using NUnit.Framework;
using System;
using System.Linq;

namespace mdoc.Test.Enumeration
{
    [TestFixture ()]
    public class ExceptionTests : CecilBaseTest
    {
        [Test ()]
        public void TestExceptionEnumerations ()
        {
            var type = GetTypeDef<ExceptionTestClass> ();
            var member = type.Methods.Single (m => m.Name == "ThrowAnException");

            var sources = new ExceptionLookup (ExceptionLocations.DependentAssemblies)[member];

            Assert.IsNotNull (sources);
            Assert.AreEqual (1, sources.Count ());
            var source = sources.First ();
            Assert.AreEqual ("ThrowAnException", source.Sources.First ().Name);
        }

        [Test ()]
        public void TestExceptionEnumerations_FromPrivateMethod ()
        {
            var type = GetTypeDef<ExceptionTestClass> ();
            var member = type.Methods.Single (m => m.Name == "ThrowFromPrivateMethod");

            var sources = new ExceptionLookup (ExceptionLocations.DependentAssemblies)[member];

            Assert.IsNotNull (sources);
            Assert.AreEqual (0, sources.Count ());
        }

        [Test ()]
        public void TestExceptionEnumerations_FromPublicMethod ()
        {
            var type = GetTypeDef<ExceptionTestClass> ();
            var member = type.Methods.Single (m => m.Name == "ThrowFromPublicMethod");

            var sources = new ExceptionLookup (ExceptionLocations.Assembly)[member];

            Assert.IsNotNull (sources);
            Assert.AreEqual (1, sources.Count ());
            var source = sources.First ();
            Assert.AreEqual ("ThrowItPublic", source.Sources.First ().Name);
        }

        public class ExceptionTestClass
        {
            public void ThrowAnException()
            {
                throw new NotImplementedException ();
            }

            public void ThrowFromPrivateMethod () => ThrowItPrivate ();
            private void ThrowItPrivate () => throw new NotImplementedException ();


            public void ThrowFromPublicMethod () => ThrowItPublic ();
            public void ThrowItPublic () => throw new NotImplementedException ();
        }
    }
}
