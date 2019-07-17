using NUnit.Framework;
using System.Linq;
using mdoc.Test.SampleClasses;
using Mono.Documentation.Updater;

namespace mdoc.Test
{
    [TestFixture ()]
    public class FormatterTests : BasicFormatterTests<CSharpMemberFormatter>
    {
        private CSharpMemberFormatter csharpMemberFormatter = new CSharpMemberFormatter();
        protected override CSharpMemberFormatter formatter => csharpMemberFormatter;

        [Test ()]
        public void Formatters_VerifyPrivateConstructorNull ()
        {
            // this is a private constructor
            var method = GetMethod (typeof(TestClass), m => m.IsConstructor && !m.IsPublic && m.Parameters.Count () == 1);

            MemberFormatter[] formatters = new MemberFormatter[]
            {
                new CSharpFullMemberFormatter (),
                new CSharpMemberFormatter(),
                new ILMemberFormatter(),
                new ILFullMemberFormatter(),
                new VBFullMemberFormatter (),
                new VBMemberFormatter(),
                new FSharpMemberFormatter(),
                new FSharpFullMemberFormatter(), 
            };
            var sigs = formatters.Select (f => f.GetDeclaration (method));

            foreach (var sig in sigs)
                Assert.IsNull (sig);
        }

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

        [Test]
        public void CSharp_op_LessThan () =>
            TestComparisonOp ("LessThan", "<");

        [Test]
        public void CSharp_op_GreaterThan () =>
            TestComparisonOp ("GreaterThan", ">");

        [Test]
        public void CSharp_op_LessThanOrEqual () =>
            TestComparisonOp ("LessThanOrEqual", "<=");

        [Test]
        public void CSharp_op_GreaterThanOrEqual () =>
            TestComparisonOp ("GreaterThanOrEqual", ">=");

        [Test]
        public void CSharp_op_Implicit () =>
            TestConversionOp ("Implicit", "implicit", "TestClass", "TestClassTwo");

        [Test]
        public void CSharp_op_Implicit_inverse () =>
            TestConversionOp ("Implicit", "implicit", "TestClassTwo", "TestClass");

        [Test]
        public void CSharp_op_Explicit () =>
            TestConversionOp ("Explicit", "explicit", "int", "TestClass");

        [Test]
        public void CSharp_op_Explicit_inverse () =>
            TestConversionOp ("Explicit", "explicit", "TestClass", "int");

        [Test]
        public void CSharp_modopt () =>
            TestMod ("SomeFunc2", "public SomeClass* SomeFunc2 (SomeClass param);", returnType: "SomeClass*");

        [Test]
        public void CSharp_modreq () =>
            TestMod ("SomeFunc", "public int SomeFunc (SomeClass* param);", returnType: "int");

        [Test]
        public void CSharp_doublepointer () =>
            TestMod ("SomeFunc3", "public SomeClass** SomeFunc3 (int param);", returnType: "cppcli.SomeClass**");

        [Test]
        public void CSharp_pointerref_modreqparam () =>
            TestMod ("SomeFunc4", "public int SomeFunc4 (SomeClass** param, int param2);", returnType: "int");

        [Test]
        public void DoubleMod ()
        {
            string doubledUp = "System.ValueType modopt(System.DateTime) modopt(System.Runtime.CompilerServices.IsBoxed)";
            string result = MemberFormatter.RemoveMod (doubledUp);
            Assert.AreEqual ("System.ValueType", result);
        }

        [Test]
        public void DoubleMod_Mixed ()
        {
            string doubledUp = "System.ValueType modreq(System.DateTime) modopt(System.Runtime.CompilerServices.IsBoxed)";
            string result = MemberFormatter.RemoveMod (doubledUp);
            Assert.AreEqual ("System.ValueType", result);
        }

        [Test]
        public void DoubleMod_Pointer ()
        {
            string doubledUp = "System.ValueType modreq(System.DateTime) modopt(System.Runtime.CompilerServices.IsBoxed)*";
            string result = MemberFormatter.RemoveMod (doubledUp);
            Assert.AreEqual ("System.ValueType*", result);
        }

        [Test]
        public void Params()
        {
            var member = GetMethod (typeof(TestClass), m => m.Name == "DoSomethingWithParams");
            var sig = formatter.GetDeclaration (member);
            Assert.AreEqual ("public void DoSomethingWithParams (params int[] values);", sig);
        }
        [Test]
        public void IL_RefAndOut ()
        {
            var member = GetMethod (typeof(TestClass), m => m.Name == "RefAndOut");
            var formatter = new ILFullMemberFormatter ();
            var sig = formatter.GetDeclaration (member);
            Assert.AreEqual (".method public hidebysig instance void RefAndOut(int32& a, [out] int32& b) cil managed", sig);
        }
        [Test]
        public void MethodSignature_Finalize() =>
            TestMethodSignature(typeof(SomeGenericClass<>),
                "~SomeGenericClass ();",
                "Finalize");

        [Test]
        public void CSharpReadonlyRefReturn()
        {
            var member = GetMethod(typeof(ReadonlyRefClass), m => m.Name == "ReadonlyRef");
            var formatter = new CSharpFullMemberFormatter();
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual("public ref readonly int ReadonlyRef ();", sig);
        }

        [Test]
        public void CSharpRefReturn()
        {
            var member = GetMethod(typeof(ReadonlyRefClass), m => m.Name == "Ref");
            var formatter = new CSharpFullMemberFormatter();
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual("public ref int Ref ();", sig);
        }

        #region Helper Methods
        string RealTypeName(string name){
            switch (name) {
                case "bool": return "Boolean";
                case "int": return "Int32";
                default: return name;
            }
        }

        void TestConversionOp (string name, string type, string leftType, string rightType) {
            TestOp (name, $"public static {type} operator {leftType} ({rightType} c1);", argCount: 1, returnType: leftType);
        }

        void TestComparisonOp (string name, string op)
        {
            TestOp (name, $"public static bool operator {op} (TestClass c1, TestClass c2);", argCount: 2, returnType: "Boolean");    
        }

        void TestUnaryOp (string name, string op, string returnType = "TestClass")
        {
            TestOp (name, $"public static {returnType} operator {op} (TestClass c1);", argCount: 1, returnType: returnType);
        }

        void TestBinaryOp (string name, string op, string returnType = "TestClass", string secondType = "TestClass")
        {
            TestOp (name, $"public static {returnType} operator {op} (TestClass c1, {secondType} c2);", argCount: 2, returnType: returnType);
        }

        void TestOp (string name, string expectedSig, int argCount, string returnType = "TestClass")
        {
            var member = GetMethod (typeof(TestClass), m => m.Name == $"op_{name}" && m.Parameters.Count == argCount && m.ReturnType.Name == RealTypeName (returnType));
            var formatter = new CSharpMemberFormatter ();
            var sig = formatter.GetDeclaration (member);
            Assert.AreEqual (expectedSig, sig);
        }

        void TestMod (string name, string expectedSig, int argCount = 1, string returnType = "SomeClass")
        {
            var member = GetMethod (
                    GetType ("SampleClasses/cppcli.dll", "cppcli.SomeInterface"), 
                    m => m.Name == name
            );
            var formatter = new CSharpMemberFormatter ();
			var sig = formatter.GetDeclaration (member);
			Assert.AreEqual (expectedSig, sig);
        }
        #endregion
    }
}
