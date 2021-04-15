using NUnit.Framework;
using System;
using System.Reflection;
using System.Linq;
using System.Text;
using mdoc.Test.SampleClasses;
using Mono.Documentation.Updater;
using Mono.Documentation;
using Mono.Documentation.Updater.Formatters.CppFormatters;
using Mono.Documentation.Updater.Formatters;

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
        public void CSharp_pointerref_modreqparam() =>
            TestMod("SomeFunc4", "public int SomeFunc4 (SomeClass** param, int param2);", returnType: "int");

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
        public void DoubleMod_Pointer()
        {
            string doubledUp = "System.ValueType modreq(System.DateTime) modopt(System.Runtime.CompilerServices.IsBoxed)*";
            string result = MemberFormatter.RemoveMod(doubledUp);
            Assert.AreEqual("System.ValueType*", result);
        }

        [Test]
        public void Mod_Ref()
        {
            string doubledUp = "System.ValueType& modopt(System.Runtime.CompilerServices.IsBoxed)";
            string result = MemberFormatter.RemoveMod(doubledUp);
            Assert.AreEqual("System.ValueType&", result);
        }

        [Test]
        public void Params()
        {
            var member = GetMethod (typeof(TestClass), m => m.Name == "DoSomethingWithParams");
            var sig = formatter.GetDeclaration (member);
            Assert.AreEqual ("public void DoSomethingWithParams (params int[] values);", sig);
        }

        [Test]
        public void RefProperty()
        {
            var member = GetType(typeof(SpanSpecial<>))
                .Properties.FirstOrDefault(t => t.FullName == "T& mdoc.Test.SpanSpecial`1::Item(System.Int32)");
            var formatterCsharp = new CSharpFullMemberFormatter();
            string sig = formatterCsharp.GetDeclaration(member);
            Assert.AreEqual("public ref T this[int index] { get; }", sig);
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
        public void CreateNewGuid()
        {
            var member = GetMethod(typeof(GuidClass), m => m.Name == "CreateNewGuid");
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual("public static Guid CreateNewGuid ();", sig);
        }

        [Test]
        public void ObjectIndentical()
        {
            var member = GetMethod(typeof(GuidClass), m => m.Name == "ObjectIndentical");
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual("public bool ObjectIndentical (Guid objGuid1, Guid objGuid2);", sig);
        }

        [Test]
        public void MethodSignature_Finalize() =>
            TestMethodSignature(typeof(SomeGenericClass<>),
                "~SomeGenericClass ();",
                "Finalize");
        [Test]
        public void FuncParams()
        {
            var member = GetMethod(typeof(SomeGenericClass<>), m => m.Name == "SomeMethod4");
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual("public void SomeMethod4 (out string a, T t, object b = default);", sig);
        }

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

        [Test]
        public void CSharpTuple()
        {
            var member = GetMethod(typeof(NullablesAndTuples), m => m.Name == "TupleReturn");
            var formatter = new CSharpFullMemberFormatter();
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual("public (int,string) TupleReturn ();", sig);
        }

        [Test]
        public void CSharpNullable()
        {
            var member = GetMethod(typeof(NullablesAndTuples), m => m.Name == "NullableInt");
            var formatter = new CSharpFullMemberFormatter();
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual("public int? NullableInt ();", sig);
        }

        [Test]
        public void PItest()
        {
            string sig = "";
            var member  = GetType(typeof(System.Math)).Fields.FirstOrDefault(t=>t.Name=="PI");
            BindingFlags flags    = BindingFlags.NonPublic | BindingFlags.Static;
            BindingFlags flagsPub = BindingFlags.Public | BindingFlags.Static;

            Type type1 = typeof(MDocUpdater);
            MethodInfo mInfo1 = type1.GetMethod("GetFieldConstValue", flags);
            Object[] parametors1 = new Object[] { member, sig };
            mInfo1.Invoke(null, parametors1);
            sig = (string)parametors1[1];
            Assert.AreEqual("3.1415926535897931", sig);
          
            Type type2 = typeof(ILFullMemberFormatter);
            sig = "";
            MethodInfo mInfo2 = type2.GetMethod("AppendFieldValue", flags);
            Object[] parametors2 = new Object[] { new StringBuilder(), member};
            sig = mInfo2.Invoke(null, parametors2).ToString();
            Assert.AreEqual(" = (3.1415926535897931)", sig);
 
            Type type3 = typeof(DocUtils);
            sig = "";                      
            MethodInfo mInfo3 = type3.GetMethod("AppendFieldValue", flagsPub);
            Object[] parametors3 = new Object[] { new StringBuilder(), member };
            mInfo3.Invoke(null, parametors3);
            sig = parametors3[0].ToString();
            Assert.AreEqual(" = 3.1415926535897931", sig);
 
            Type type4 = typeof(CppFullMemberFormatter);
            sig = "";
            MethodInfo mInfo4 = type4.GetMethod("AppendFieldValue", flags);
            Object[] parametors4 = new Object[] { new StringBuilder(), member };
            sig = mInfo4.Invoke(null, parametors4).ToString();
            Assert.AreEqual(" = 3.1415926535897931", sig);
        }

        [Test]
        public void StaticConstructor()
        {
            var member = GetMethod(
                 GetType(typeof(StaticClass)),
                 m => m.Name == ".cctor"
            );
            var formatter = new CSharpFullMemberFormatter();
            var sigCsharp = formatter.GetDeclaration(member);
            Assert.AreEqual("static StaticClass ();", sigCsharp);

            var formatterVB = new VBFullMemberFormatter();
            var sigVB = formatterVB.GetDeclaration(member);
            Assert.AreEqual("Shared Sub New ()", sigVB);

            var formatterC = new CppFullMemberFormatter();
            var sigC = formatterC.GetDeclaration(member);
            Assert.AreEqual(" static StaticClass();", sigC);
        }

        [Test]
        public void MissSignature()
        {
            var member1 = GetMethod(typeof(System.IO.FileStream), m => m.FullName == "System.Void System.IO.FileStream::.ctor(System.String,System.IO.FileMode,System.Security.AccessControl.FileSystemRights,System.IO.FileShare,System.Int32,System.IO.FileOptions,System.Security.AccessControl.FileSecurity)"); ;
            var fomatter1 = new VBMemberFormatter();
            // Original return null
            var sig1 = fomatter1.GetDeclaration(member1);
            Assert.NotNull(sig1);

            var member2 = GetMethod(typeof(TestClassThree), m => m.FullName == "System.Collections.IEnumerator mdoc.Test.SampleClasses.TestClassThree::System.Collections.IEnumerable.GetEnumerator()"); ;
            var formatter2 = new FSharpFormatter(MDocUpdater.Instance.TypeMap);
            // Original return null
            var sig2 = formatter2.GetDeclaration(member2);
            Assert.NotNull(sig2);
        }
        
        [Test]
        public void ClassInterface()
        {
            // CopyTo :
            // Orignal return value: "System.Void CopyTo (T[], System.Int32, )
            var sig = "System.Void CopyTo (System.Collections.Generic.KeyValuePair`2<System.String,mdoc.Test.SampleClasses.TestClassTwo>[], System.Int32, )";
            var type = GetType(typeof(TestClassThree));
            var interFaceMembers = GetClassInterface(type);
            bool flag = interFaceMembers.ContainsKey(sig);
            Assert.AreEqual(true, flag);
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
