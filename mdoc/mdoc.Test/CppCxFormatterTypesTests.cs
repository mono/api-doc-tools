﻿using System;
using Mono.Cecil;
using Mono.Documentation.Updater.Formatters.CppFormatters;
#if !NETCOREAPP
using Mono_DocTest;
using Mono_DocTest_Generic;
#endif //!NETCOREAPP
using NUnit.Framework;

namespace mdoc.Test
{
#if !NETCOREAPP

    [TestFixture]
    [Category("CppCx")]
    public class CppCxFormatterTypesTests : BasicFormatterTests<CppCxMemberFormatter>
    {
        protected override CppCxMemberFormatter formatter => new CppCxMemberFormatter();

        private string _cppWinRtTestLibName = "../../../../external/Windows/Windows.Foundation.UniversalApiContract.winmd";
        private string _cppCxTestLibName = "../../../../external/Test/UwpTestWinRtComponentCpp.winmd";

        protected override TypeDefinition GetType(Type type)
        {
            var moduleName = type.Module.FullyQualifiedName;

            var tref = GetType(moduleName, type.FullName?.Replace("+", "/"));
            return tref;
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_CustomAttribute()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.CustomAttribute1",
                "public ref class CustomAttribute1 sealed : Platform::Metadata::Attribute");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_Class1()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.Class1",
                "public ref class Class1 sealed");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_delegate()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.PrimeFoundHandler",
                "public delegate void PrimeFoundHandler(int result);");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_delegateWithSpecificType()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.PrimeFoundHandlerWithSpecificType",
                "public delegate void PrimeFoundHandlerWithSpecificType(IMap<double, float> ^ result);");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_delegateWithCustomType()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.SomethingHappenedEventHandler",
                "public delegate void SomethingHappenedEventHandler(Class1 ^ sender, Platform::String ^ s);");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_enum()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.Color1", "public enum class Color1");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_publicUnsealedClass()
        {
            TestTypeSignature(_cppCxTestLibName, "Namespace2.Class2", @"public ref class Class2 : Windows::UI::Xaml::Application");

        }

        [Test]
        [Category("Type")]
        public void TypeSignature_ValueClass()
        {
            TestTypeSignature(_cppCxTestLibName, "Namespace2.Class4", "public value class Class4");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsMatrix3x2()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Matrix3x2", "public value class float3x2");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsMatrix4x4()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Matrix4x4", "public value class float4x4");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsQuaternion()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Quaternion", "public value class quaternion");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsVector2()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Vector2", "public value class float2");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsVector3()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Vector3", "public value class float3");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsVector4()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Vector4", "public value class float4");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_ValueGuid()
        {
            TestTypeSignature(typeof(Guid), 
                "public value class Platform::Guid : IComparable, IComparable<Platform::Guid>, IEquatable<Platform::Guid>, IFormattable");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_ValueSingle()
        {
            TestTypeSignature(typeof(Single), 
                "public value class float : IComparable, IComparable<float>, IConvertible, IEquatable<float>, IFormattable");
        }

        #region NoSupport

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsPlane()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Plane", null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_GenericDelegate()
        {
            TestTypeSignature(typeof(Action22<>), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_DelegateWithSystemType()
        {
            TestTypeSignature(typeof(DelegateWithNetSystemType), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_NoNamespace()
        {
            TestTypeSignature(typeof(NoNamespace), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_StandardType()
        {
            TestMethodSignature(typeof(UseLists), null, nameof(UseLists.Process));
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_PublicNestedType()
        {
            TestTypeSignature(typeof(Widget.NestedClass), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_PublicNestedEnum()
        {
            TestTypeSignature(typeof(Widget.NestedEnum), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_JaggedArrays()
        {
            TestMethodSignature(typeof(Widget), null, "M2");
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_PublicConstructorUnsealedClass()
        {
            TestTypeSignature(typeof(DocAttribute), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_PublicRefClassWithGeneric()
        {
            TestTypeSignature(typeof(GenericBase<>), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_GenericInterfaceWithConstraints()
        {
            TestTypeSignature(typeof(IFooNew<>), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_ValueTypeWithNotAllowedType()
        {
            TestTypeSignature(typeof(ValueClassSpecificField), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_PublicIndexedProperty()
        {
            TestPropertySignature(typeof(Widget), null, "indexedProperty");
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_CustomException()
        {
            TestTypeSignature(typeof(CustomException), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_Exception_ArgumentNullExceptionField()
        {
            TestFieldSignature(typeof(CustomException), null, "ArgumentNullExceptionField");
        }

        #endregion

        [TearDown]
        public void TearDown()
        {
            moduleCash.Clear();
        }
    }

#endif //!NETCOREAPP

}

