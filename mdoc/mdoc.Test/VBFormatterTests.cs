using System;
using System.Linq;
using mdoc.Test.SampleClasses;
using Mono.Cecil;
using Mono.Documentation.Updater;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture()]
    public class VBFormatterTests
    {
        [Test]
        public void VB_op_Addition() =>
            TestBinaryOp("Addition", "+");

        [Test]
        public void VB_op_Subtraction() =>
            TestBinaryOp("Subtraction", "-");

        [Test]
        public void VB_op_Division() =>
            TestBinaryOp("Division", "/");
        
        [Test]
        public void VB_op_Multiplication() =>
            TestBinaryOp("Multiply", "*");

        [Test]
        public void VB_op_Modulus() =>
            TestBinaryOp("Modulus", "Mod");

        [Test]
        public void VB_op_BitwiseAnd() =>
            TestBinaryOp("BitwiseAnd", "And");

        [Test]
        public void VB_op_BitwiseOr() =>
            TestBinaryOp("BitwiseOr", "Or");

        [Test]
        public void VB_op_ExclusiveOr() =>
            TestBinaryOp("ExclusiveOr", "Xor");

        [Test]
        public void VB_op_LeftShift() =>
            TestBinaryOp("LeftShift", "<<", secondType: "Integer");

        [Test]
        public void VB_op_RightShift() =>
            TestBinaryOp("RightShift", ">>", secondType: "Integer");

        [Test]
        public void VB_op_UnaryPlus() =>
            TestUnaryOp("UnaryPlus", "+");

        [Test]
        public void VB_op_UnaryNegation() =>
            TestUnaryOp("UnaryNegation", "-");

        [Test]
        public void VB_op_LogicalNot() =>
            TestUnaryOp("LogicalNot", "Not");

        [Test]
        public void VB_op_OnesComplement() =>
            TestUnaryOp("OnesComplement", "Not");
        
        [Test]
        public void VB_op_True() =>
            TestUnaryOp("True", "IsTrue", returnType: "Boolean");

        [Test]
        public void VB_op_False() =>
            TestUnaryOp("False", "IsFalse", returnType: "Boolean");

        [Test]
        public void VB_op_Equality() =>
            TestComparisonOp("Equality", "==");

        [Test]
        public void VB_op_Inequality() =>
            TestComparisonOp("Inequality", "!=");

        [Test]
        public void VB_op_LessThan() =>
            TestComparisonOp("LessThan", "<");

        [Test]
        public void VB_op_GreaterThan() =>
            TestComparisonOp("GreaterThan", ">");

        [Test]
        public void VB_op_LessThanOrEqual() =>
            TestComparisonOp("LessThanOrEqual", "<=");

        [Test]
        public void VB_op_GreaterThanOrEqual() =>
            TestComparisonOp("GreaterThanOrEqual", ">=");

        [Test]
        public void VB_op_Implicit() =>
            TestConversionOp("Implicit", "Widening", "TestClass", "TestClassTwo");

        [Test]
        public void VB_op_Implicit_inverse() =>
            TestConversionOp("Implicit", "Widening", "TestClassTwo", "TestClass");

        [Test]
        public void VB_op_Explicit() =>
            TestConversionOp("Explicit", "Narrowing", "Integer", "TestClass");

        [Test]
        public void VB_op_Explicit_inverse() =>
            TestConversionOp("Explicit", "Narrowing", "TestClass", "Integer");
        
        
        [Test]
        public void Params()
        {
            var member = GetMethod<TestClass>(m => m.Name == "DoSomethingWithParams");
            var formatter = new VBMemberFormatter();
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual("Public Sub DoSomethingWithParams (ParamArray values As Integer())", sig);
        }

        #region Helper Methods
        string RealTypeName(string name)
        {
            switch (name)
            {
                case "Integer": return "Int32";
                default: return name;
            }
        }

        void TestConversionOp(string name, string type, string leftType, string rightType)
        {
            TestOp(name, $"Public Shared {type} Operator CType (c1 As {rightType}) As {leftType}", argCount: 1, returnType: leftType);
        }

        void TestComparisonOp(string name, string op)
        {
            TestOp(name, $"Public Shared Operator {op} (c1 As TestClass, c2 As TestClass) As Boolean", argCount: 2, returnType: "Boolean");
        }

        void TestUnaryOp(string name, string op, string returnType = "TestClass")
        {
            TestOp(name, $"Public Shared Operator {op} (c1 As TestClass) As {returnType}", argCount: 1, returnType: returnType);
        }

        void TestBinaryOp(string name, string op, string returnType = "TestClass", string secondType = "TestClass")
        {
            TestOp(name, $"Public Shared Operator {op} (c1 As TestClass, c2 As {secondType}) As {returnType}", argCount: 2, returnType: returnType);
        }

        void TestOp(string name, string expectedSig, int argCount, string returnType = "TestClass")
        {
            var member = GetMethod<TestClass>(m => m.Name == $"op_{name}" && m.Parameters.Count == argCount && m.ReturnType.Name == RealTypeName(returnType));
            var formatter = new VBMemberFormatter();
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual(expectedSig, sig);
        }

        MethodDefinition GetMethod<T>(Func<MethodDefinition, bool> query)
        {
            var type = typeof(T);
            var moduleName = type.Module.FullyQualifiedName;
            return GetMethod(GetType(moduleName, type.FullName), query);
        }

        MethodDefinition GetMethod(TypeDefinition testclass, Func<MethodDefinition, bool> query)
        {
            var methods = testclass.Methods;
            var member = methods.FirstOrDefault(query).Resolve();
            if (member == null)
                throw new Exception("Did not find the member in the test class");
            return member;
        }

        TypeDefinition GetType(string filepath, string classname)
        {
            var module = ModuleDefinition.ReadModule(filepath);
            var types = module.GetTypes();
            var testclass = types
                .SingleOrDefault(t => t.FullName == classname);
            if (testclass == null)
            {
                throw new Exception($"Test was unable to find type {classname}");
            }
            return testclass.Resolve();
        }
        #endregion
    }
}