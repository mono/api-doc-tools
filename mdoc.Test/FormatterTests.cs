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
            var method = GetMethod<TestClass> (m => m.IsConstructor && !m.IsPublic && m.Parameters.Count () == 1);

            MemberFormatter[] formatters = new MemberFormatter[]
            {
                new CSharpFullMemberFormatter (),
                new CSharpMemberFormatter(),
                new ILMemberFormatter(),
                new ILFullMemberFormatter()
            };
            var sigs = formatters.Select (f => f.GetDeclaration (method));

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

        [Test]
        public void CSharp_op_BitwiseAnd () =>
            TestBinaryOp ("BitwiseAnd", "&");

        [Test]
        public void CSharp_op_BitwiseOr () =>
            TestBinaryOp ("BitwiseOr", "|");

        [Test]
        public void CSharp_op_ExclusiveOr () =>
            TestBinaryOp ("ExclusiveOr", "^");

        [Test]
        public void CSharp_op_LeftShift () =>
            TestBinaryOp ("LeftShift", "<<", secondType: "int");

        [Test]
        public void CSharp_op_RightShift () =>
            TestBinaryOp ("RightShift", ">>", secondType: "int");

        [Test]
        public void CSharp_op_UnaryPlus () =>
			TestUnaryOp ("UnaryPlus", "+");

		[Test]
		public void CSharp_op_UnaryNegation () =>
			TestUnaryOp ("UnaryNegation", "-");

		[Test]
		public void CSharp_op_LogicalNot () =>
			TestUnaryOp ("LogicalNot", "!");

        [Test]
        public void CSharp_op_OnesComplement () =>
            TestUnaryOp ("OnesComplement", "~");

        [Test]
        public void CSharp_op_Decrement () =>
            TestUnaryOp ("Decrement", "--");

		[Test]
		public void CSharp_op_Increment () =>
			TestUnaryOp ("Increment", "++");

        [Test]
        public void CSharp_op_True () =>
            TestUnaryOp ("True", "true", returnType: "bool");

		[Test]
		public void CSharp_op_False () =>
            TestUnaryOp ("False", "false", returnType: "bool");

        [Test]
        public void CSharp_op_Equality () =>
            TestComparisonOp ("Equality", "==");

        [Test]
        public void CSharp_op_Inequality () =>
            TestComparisonOp ("Inequality", "!=");

#region Helper Methods
        void TestComparisonOp (string name, string op)
        {
            TestOp (name, $"public static bool operator {op} (TestClass c1, TestClass c2);", argCount: 2);    
        }

        void TestUnaryOp (string name, string op, string returnType = "TestClass")
        {
            TestOp (name, $"public static {returnType} operator {op} (TestClass c1);", argCount: 1);
        }

        void TestBinaryOp (string name, string op, string returnType = "TestClass", string secondType = "TestClass")
        {
            TestOp (name, $"public static {returnType} operator {op} (TestClass c1, {secondType} c2);", argCount: 2);
        }

        void TestOp (string name, string expectedSig, int argCount)
        {
            var member = GetMethod<TestClass> (m => m.Name == $"op_{name}" && m.Parameters.Count == argCount);
            var formatter = new CSharpMemberFormatter ();
            var sig = formatter.GetDeclaration (member);
            Assert.AreEqual (expectedSig, sig);
        }

        MethodDefinition GetMethod<T> (Func<MethodDefinition, bool> query)
        {
            var testclass = GetType<T> ();
            var methods = testclass.Methods;
            var member = methods.FirstOrDefault (query).Resolve ();
            if (member == null)
                throw new Exception ("Did not find the member in the test class");
            return member;
        }

        TypeDefinition GetType<T> ()
        {
            var classtype = typeof (T);
            var module = ModuleDefinition.ReadModule (classtype.Module.FullyQualifiedName);
            var types = module.GetTypes ();
            var testclass = types
                .SingleOrDefault (t => t.FullName == classtype.FullName);
            if (testclass == null)
            {
                throw new Exception ($"Test was unable to find type {classtype.FullName}");
            }
            return testclass.Resolve ();
        }
#endregion
    }
}
