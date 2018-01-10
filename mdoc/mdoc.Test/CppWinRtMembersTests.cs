using Mono.Documentation.Updater.CppFormatters;
using Mono_DocTest;
using NUnit.Framework;
using Cpp = Mono_DocTest_Generic;

namespace mdoc.Test
{
    [TestFixture]
    public class CppWinRtMembersTests: BasicFormatterTests<CppWinRtFullMemberFormatter>
    {
        private static readonly CppWinRtFullMemberFormatter CppWinRtFullMemberFormatter = new CppWinRtFullMemberFormatter();
        protected override CppWinRtFullMemberFormatter formatter => CppWinRtFullMemberFormatter;

        private string CppCxTestLibName = "../../../../external/Test/UwpTestWinRtComponentCpp.winmd";
        private const string CSharpTestLib = "../../../../external/Test/CSharpExample.dll";

        [Test]
        [Category("Method")]
        public void Method_ComputeResult()
        {
            TestMethodSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "ComputeResult",
                @"winrt::Windows::Foundation::Collections::IVector<double> ComputeResult(double input);");
        }

        [Test]
        [Category("Method")]
        public void Method_GetPrimesOrdered()
        {
            TestMethodSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "GetPrimesOrdered",
                @"winrt::Windows::Foundation::IAsyncOperationWithProgress<Windows::Foundation::Collections::IVector<int>, double> GetPrimesOrdered(int first, int last);");

        }

        [Test]
        [Category("Method")]
        public void Method_GetPrimesUnordered()
        {
            TestMethodSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "GetPrimesUnordered",
                @"winrt::Windows::Foundation::IAsyncActionWithProgress<double> GetPrimesUnordered(int first, int last);");
        }

        [Test]
        [Category("Method")]
        public void Method_DefaultParameters()
        {
            TestMethodSignature(CSharpTestLib, "Mono.DocTest.Widget", "Default",
                @"void Default(int a = 1, int b = 2);");
        }

        [Test]
        [Category("Method")]
        public void Method_RefType()
        {
            TestMethodSignature(CppCxTestLibName, "Namespace222.App", "SetWindow1",
                @"void SetWindow1(winrt::Windows::UI::Core::CoreWindow const & window);");
        }

        [Test]
        [Category("Method")]
        public void Method_WinRtTypeInterfaceImplementation()
        {
            TestMethodSignature(CppCxTestLibName, "Namespace222.App", "SetWindow",
                @"void SetWindow(winrt::Windows::UI::Core::CoreWindow const & window);");
        }

        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstLong() =>
            TestFieldSignature(typeof(Cpp.GenericBase<>),
                "const long ConstLong;",
                nameof(Cpp.GenericBase<int>.ConstLong));

        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstChar() =>
            TestFieldSignature(typeof(Cpp.GenericBase<>),
                "const char ConstChar;",
                nameof(Cpp.GenericBase<int>.ConstChar));



        #region NoSupport
        [Test]
        [Category("NoSupport")]
        public void NoSupport_Property()
        {
            TestPropertySignature(CppCxTestLibName, "Namespace2.Class3", "LongProperty", null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_Event()
        {
            TestEventSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "primeFoundEvent", null);
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_ExtensionMethod()
        {
            TestMethodSignature(typeof(Cpp.Extensions), null, nameof(Cpp.Extensions.Bar));
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_SystemTypes()
        {
            TestMethodSignature(typeof(Cpp.Extensions), null, nameof(Cpp.Extensions.Bar));
        }


        [Test]
        [Category("NoSupport")]
        public void MethodSignature_ParamsKeyword_M6()
        {
            TestMethodSignature(typeof(Widget), null, "M6");
        }

        [Test]
        [Category("NoSupport")]
        public void NoSupport_StandardType()
        {
            TestMethodSignature(typeof(UseLists), null, nameof(UseLists.Process));
        }

        [Test]
        [Category("Fields")]
        public void NoSupport_Decimal() =>
            TestFieldSignature(typeof(Cpp.GenericBase<>),
                null,
                nameof(Cpp.GenericBase<int>.ConstDecimal));
#endregion
    }
}
