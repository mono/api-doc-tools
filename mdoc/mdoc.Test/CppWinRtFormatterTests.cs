using System;
using Mono.Cecil;
using Mono.Documentation.Updater.Formatters.CppFormatters;
using Mono_DocTest;
using Mono_DocTest_Generic;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture]
    public class CppWinRtFormatterTests : BasicFormatterTests<CppWinRtMemberFormatter>
    {
        private static readonly CppWinRtMemberFormatter CppWinRtMemberFormatter = new CppWinRtMemberFormatter();
        protected override CppWinRtMemberFormatter formatter => CppWinRtMemberFormatter;

        private string _cppWinRtTestLibName = "../../../../external/Windows/Windows.Foundation.UniversalApiContract.winmd";
        private string _cppCxTestLibName = "../../../../external/Test/UwpTestWinRtComponentCpp.winmd";
        private const string CSharpTestLib = "../../../../external/Test/CSharpExample.dll";

        protected override TypeDefinition GetType(Type type)
        {
            var moduleName = type.Module.FullyQualifiedName;

            var tref = GetType(moduleName, type.FullName?.Replace("+", "/"));
            return tref;
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_enum()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.Color1", "enum class Color1");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_publicSealedClass_Class1()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.Class1",
                "class Class1 final");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_publicUnsealedClass_Class2()
        {
            TestTypeSignature(_cppCxTestLibName, "Namespace2.Class2", @"class Class2 : winrt::Windows::UI::Xaml::Application");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_Struct_Class4()
        {
            TestTypeSignature(_cppCxTestLibName, "Namespace2.Class4", "struct Class4");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsMatrix3x2()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Matrix3x2", "struct float3x2");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsMatrix4x4()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Matrix4x4", "struct float4x4");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsPlane()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Plane", "struct plane");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsQuaternion()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Quaternion", "struct quaternion");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsVector2()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Vector2", "struct float2");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsVector3()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Vector3", "struct float3");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NumericsVector4()
        {
            TestTypeSignature(_cppWinRtTestLibName, "Windows.Foundation.Numerics.Vector4", "struct float4");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_GenericInterface()
        {
            TestTypeSignature(typeof(IFoo<>), @"template <typename T>
struct IFoo");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_GenericClass()
        {
            TestTypeSignature(typeof(MyList<>.Helper<,>), @"template <typename U, typename V>
[Windows::Foundation::Metadata::WebHostHidden]
class MyList<T>::Helper");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_NestedClass()
        {
            TestTypeSignature(typeof(Widget.NestedClass.Double), @"[Windows::Foundation::Metadata::WebHostHidden]
class Widget::NestedClass::Double");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_WebHostHiddenAttribute()
        {
            TestTypeSignature(typeof(Widget), @"[Windows::Foundation::Metadata::WebHostHidden]
class Widget : Mono_DocTest::IProcess");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_ValueGuid()
        {
            TestTypeSignature(typeof(Guid), 
                "struct winrt::guid : IComparable, IComparable<winrt::guid>, IEquatable<winrt::guid>, IFormattable");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_ValueSingle()
        {
            TestTypeSignature(typeof(Single), 
                "struct float : IComparable, IComparable<float>, IConvertible, IEquatable<float>, IFormattable");
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_Delegate()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.PrimeFoundHandlerWithSpecificType", null);
        }
        
        [Test]
        [Category("NoSupport")]
        public void NoSupport_DelegateWithCustomType()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.SomethingHappenedEventHandler",
                null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_CustomAttribute()
        {
            TestTypeSignature(_cppCxTestLibName, "UwpTestWinRtComponentCpp.CustomAttribute1", null);
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

        [Test]
        [Category("NoSupport")]
        public void NoSupport_Exception_InterfaceWithGenericConstraints()
        {
            TestTypeSignature(typeof(IFooNew<>), null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_Exception_ClassWithGenericConstraints()
        {
            TestTypeSignature(typeof(GenericConstraintClass<>), null);
        }
        [Test]
        [Category("NoSupport")]
        public void NoSupport_Exception_NestedClassWithSameName()
        {
            TestTypeSignature(CSharpTestLib, "Mono.DocTest.Widget/NestedClass", null);
        }
    }
}
