using mdoc.Test.SampleClasses;
using Mono.Documentation.Updater.Formatters.CppFormatters;
using Mono_DocTest;
using NUnit.Framework;
using Cpp = Mono_DocTest_Generic;

namespace mdoc.Test
{
    [TestFixture]
    [Category("CppCx")]
    public class CppCxFormatterMembersTests : BasicFormatterTests<CppCxFullMemberFormatter>
    {
        protected override CppCxFullMemberFormatter formatter { get; } = new CppCxFullMemberFormatter();

        private const string CppCxTestLibName = "../../../../external/Test/UwpTestWinRtComponentCpp.winmd";
        private const string CSharpTestLib = "../../../../external/Test/CSharpExample.dll";

        [Test]
        [Category("Method")]
        public void Method_ComputeResult()
        {
            TestMethodSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "ComputeResult",
                @"public:
 Windows::Foundation::Collections::IVector<double> ^ ComputeResult(double input);");
        }

        [Test]
        [Category("Method")]
        public void Method_GetPrimesOrdered()
        {
            TestMethodSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "GetPrimesOrdered",
                @"public:
 Windows::Foundation::IAsyncOperationWithProgress<Windows::Foundation::Collections::IVector<int> ^, double> ^ GetPrimesOrdered(int first, int last);");

        }

        [Test]
        [Category("Method")]
        public void Method_GetPrimesUnordered()
        {
            TestMethodSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "GetPrimesUnordered",
                @"public:
 Windows::Foundation::IAsyncActionWithProgress<double> ^ GetPrimesUnordered(int first, int last);");
        }

        [Test]
        [Category("Method")]
        public void Method_CreateNewGuid()
        {
            var member = GetMethod(typeof(GuidClass), m => m.Name == "CreateNewGuid");
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual(@"public:
 static Platform::Guid CreateNewGuid();", sig);
        }

        [Test]
        [Category("Method")]
        public void Method_ObjectIndentical()
        {
            var member = GetMethod(typeof(GuidClass), m => m.Name == "ObjectIndentical");
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual(@"public:
 bool ObjectIndentical(Platform::Guid objGuid1, Platform::Guid objGuid2);", sig);
        }

        [Test]
        [Category("Method")]
        public void Method_WinRtTypeInterfaceImplementation()
        {
            TestMethodSignature(CppCxTestLibName, "Namespace222.App", "SetWindow",
                @"public:
 virtual void SetWindow(Windows::UI::Core::CoreWindow ^ window) = Windows::ApplicationModel::Core::IFrameworkView::SetWindow;");
        }

        [Test]
        [Category("Field")]
        public void Field_CustomAttributeFundamentalType()
        {
           TestFieldSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.CustomAttribute1", "Field1", "public: bool Field1;");
        }


        [Test]
        [Category("Field")]
        public void Field_CustomAttributуSpecificType()
        {
            TestFieldSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.CustomAttribute1", "Field2", "public: Windows::Foundation::HResult Field2;");
        }

        [Test]
        [Category("Field")]
        public void Field_EnumField()
        {
            TestFieldSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Color1", "Red", "Red");
        }

        [Test]
        [Category("Field")]
        public void Field_ValueType_String()
        {
            TestFieldSignature(CppCxTestLibName, "Namespace2.Class4", "StringField", "public: Platform::String ^ StringField;");
        }

        [Test]
        [Category("Event")]
        public void Event_Class1_primeFoundEvent()
        {
            TestEventSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "primeFoundEvent", @"public:
 event UwpTestWinRtComponentCpp::PrimeFoundHandler ^ primeFoundEvent;");
        }

        [Test]
        [Category("Properties")]
        public void Property_FundamentalType()
        {
            TestPropertySignature(CppCxTestLibName, "Namespace2.Class3", "LongProperty", @"public:
 property long long LongProperty { long long get(); void set(long long value); };");
        }

        [Test]
        [Category("Properties")]
        public void Property_EII_implementation_correctDelimeter()
        {
            TestPropertySignature(CSharpTestLib, "Mono.DocTest.Generic.MyList`2", "System.Collections.Generic.ICollection<A>.IsReadOnly", @"property bool System::Collections::Generic::ICollection<A>::IsReadOnly { bool get(); };");
        }

        [Test]
        [Category("Properties")]
        public void Property_ArrayOfTypeProperty()
        {
            TestPropertySignature(CppCxTestLibName, "Namespace2.Class3", "ArrayOfTypeProperty", @"public:
 property Platform::Array <Platform::Type ^> ^ ArrayOfTypeProperty { Platform::Array <Platform::Type ^> ^ get(); void set(Platform::Array <Platform::Type ^> ^ value); };");
        }

        [Test]
        [Category("Properties")]
        public void Property_ArrayOfTypePropertyProtected()
        {
            TestPropertySignature(CppCxTestLibName, "Namespace2.Class3", "ArrayOfTypePropertyProtected", @"protected:
 property Platform::Array <Platform::Type ^> ^ ArrayOfTypePropertyProtected { Platform::Array <Platform::Type ^> ^ get(); void set(Platform::Array <Platform::Type ^> ^ value); };");
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_ParamsKeyword_M6()
        {
            TestMethodSignature(typeof(Widget), null, "M6");
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_DefaultParameters()
        {
            TestMethodSignature(CSharpTestLib, "Mono.DocTest.Widget", "Default", null);
        }
        
        [TearDown]
        public void TearDown()
        {
            moduleCash.Clear();
        }
        [Test]
        [Category("NoSupport")]
        public void NoSupport_Exception_NestedClassWithSameName()
        {
            TestTypeSignature(CSharpTestLib, "Mono.DocTest.Widget/NestedClass", null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_GenericMethodInUwp()
        {
            TestMethodSignature(typeof(Cpp.GenericBase<>), null, "BaseMethod2");
        }
    }

}

