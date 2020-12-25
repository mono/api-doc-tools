using Mono.Documentation.Updater.Formatters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdoc.Test
{
    public class NullableReferenceTypesTest : BasicFormatterTests<CSharpMemberFormatter>
    {
        private const string NullableReferenceTypesAssemblyPath = "../../../../external/Test/mdoc.Test.NullableReferenceTypes.dll";

        private CSharpMemberFormatter csharpMemberFormatter = new CSharpMemberFormatter();

        protected override CSharpMemberFormatter formatter => csharpMemberFormatter;

        [Test]
        public void TestMethodParameter_NonNullable()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "NonNullable",
                "public void NonNullable (string s);");
        }

        [Test]
        public void TestMethodParameter_TwoNullable()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "TwoNullable",
                "public void TwoNullable (string? s, object? o);");
        }

        [Test]
        public void TestMethodParameter_NullableArray()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "NullableArray",
                "public void NullableArray (object?[] args);");
        }

        [Test]
        public void TestMethodParameter_NullableArrayOfNullableObject()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "NullableArrayOfNullableObject",
                "public void NullableArrayOfNullableObject (object?[]? args);");
        }

        [Test]
        public void TestMethodParameter_TwoDimensionalNullableArrayOfNonNullableObject()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "TwoDimensionalNullableArrayOfNonNullableObject",
                "public void TwoDimensionalNullableArrayOfNonNullableObject (object[]?[]? args);");
        }

        [Test]
        public void TestMethodParameter_TwoDimensionalNullableArrayOfNullableObject()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "TwoDimensionalNullableArrayOfNullableObject",
                "public void TwoDimensionalNullableArrayOfNullableObject (object?[]?[]? args);");
        }

        [Test]
        public void TestMethodParameter_OneNullableAndTowNonNullable()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "OneNullableAndTowNonNullable",
                "public void OneNullableAndTowNonNullable (string s, object? o, string x);");
        }

        [Test]
        public void TestMethodParameter_NullableInterface()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "NullableInterface",
                "public void NullableInterface (ICollection<string>? list, ICollection<int>? args);");
        }

        [Test]
        public void TestMethodParameter_NullableInterfaceOfNullableObject()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "NullableInterfaceOfNullableObject",
                "public void NullableInterfaceOfNullableObject (ICollection<string?>? list, ICollection<int?>? args);");
        }

        [Test]
        public void TestMethodParameter_NullableInt()
        {
            TestMethodSignature(
                NullableReferenceTypesAssemblyPath,
                "mdoc.Test.NullableReferenceTypes.MethodParameter",
                "NullableInt",
                "public void NullableInt (int? i);");
        }
    }
}
