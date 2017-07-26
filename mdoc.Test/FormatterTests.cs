using NUnit.Framework;
using System;
using System.Linq;
using Mono.Documentation;
using Mono.Cecil;

using mdoc.Test.SampleClasses;
using System.Linq.Expressions;

namespace mdoc.Test
{
    [TestFixture ()]
    public class FormatterTests
    {
        [Test ()]
        public void Formatters_VerifyPrivateConstructorNull ()
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
		public void CSharp_op_Addition () =>
			TestBinaryOp ("Addition", "+");

		[Test]
		public void CSharp_op_Subtraction () =>
			TestBinaryOp ("Subtraction", "-");

        [Test]
        public void CSharp_op_Division () =>
            TestBinaryOp ("Division", "/");

        [Test]
        public void CSharp_op_Multiplication () =>
            TestBinaryOp ("Multiply", "*");

        [Test]
        public void CSharp_op_Modulus () =>
            TestBinaryOp ("Modulus", "%");

        void TestBinaryOp(string name, string op)
		{
            var addition = GetMember<TestClass> (m => m.Name == $"op_{name}");
			var formatter = new CSharpMemberFormatter ();
			var sig = formatter.GetDeclaration (addition);
            Assert.AreEqual ($"public static TestClass operator {op} (TestClass c1, TestClass c2);", sig);
        }

        MethodDefinition GetMember<T> (Func<MethodDefinition, bool> query) 
        {
			var testclass = GetType<T> ();
            var member = testclass.Methods.FirstOrDefault (query).Resolve();
            return member;
        }

		TypeDefinition GetType<T> ()
		{
			var classtype = typeof (T);
			var module = ModuleDefinition.ReadModule (classtype.Module.FullyQualifiedName);
			var types = module.GetTypes ();
			var testclass = types
                .SingleOrDefault (t => t.FullName == classtype.FullName);
            if (testclass == null) {
                throw new Exception ($"Test was unable to find type {classtype.FullName}");
            }
			return testclass.Resolve ();
		}
    }
}
