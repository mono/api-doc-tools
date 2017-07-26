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
            // this is a private constructor
            var method = GetMember<TestClass> (m => m.IsConstructor && !m.IsPublic && m.Parameters.Count() == 1);

            MemberFormatter[] formatters = new MemberFormatter[]
            {
                new CSharpFullMemberFormatter (),
                new CSharpMemberFormatter(),
                new ILMemberFormatter(),
                new ILFullMemberFormatter()
            };
            var sigs = formatters.Select(f => f.GetDeclaration (method));

            foreach (var sig in sigs)
                Assert.IsNull (sig);
        }

        // TODO: test for sig of every operator in TestClass .
        // starting with op_Addition
        [Test]
        public void CSharp_op_Addition() 
        {
            var addition = GetMember<TestClass> (m => m.Name == "op_Addition");
            var formatter = new CSharpMemberFormatter ();
            var sig = formatter.GetDeclaration (addition);
            Assert.AreEqual ("public static TestClass operator + (TestClass c1, TestClass c2);", sig);
        }

        MethodDefinition GetMember<T> (Func<MethodDefinition, bool> query) 
        {
			var testclass = GetType<TestClass> ();
            return testclass.Methods.FirstOrDefault (query).Resolve();
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
