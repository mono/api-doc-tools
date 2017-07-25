using NUnit.Framework;
using System;
using System.Linq;
using Mono.Documentation;
using Mono.Cecil;

using mdoc.Test.SampleClasses;

namespace mdoc.Test
{
    [TestFixture ()]
    public class FormatterTests
    {
        [Test ()]
        public void CSharp_VerifyPrivateConstructorNull ()
        {
            var testclass = GetType<TestClass> ();
            var method = testclass.Methods.FirstOrDefault (m => m.IsConstructor && m.Parameters.Count() == 1);

			var formatter = new CSharpFullMemberFormatter ();
            string sig = formatter.GetDeclaration (method);

            Assert.IsNull (sig);
        }


		TypeDefinition GetType<T> ()
		{
			var classtype = typeof (T);
			var module = ModuleDefinition.ReadModule (classtype.Module.FullyQualifiedName);
			var types = module.GetTypes ();
			var testclass = types
                .Single (t => t.FullName == classtype.FullName);
			return testclass.Resolve ();
		}
    }
}
