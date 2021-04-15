using mdoc.Test.SampleClasses;
using Mono.Documentation.Updater.Formatters.CppFormatters;
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
                @"winrt::Windows::Foundation::Collections::IVector<double> ComputeResult(double const& input);");
        }

        [Test]
        [Category("Method")]
        public void Method_GetPrimesOrdered()
        {
            TestMethodSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "GetPrimesOrdered",
                @"winrt::Windows::Foundation::IAsyncOperationWithProgress<winrt::Windows::Foundation::Collections::IVector<int>, double> GetPrimesOrdered(int const& first, int const& last);");

        }

        [Test]
        [Category("Method")]
        public void Method_GetPrimesUnordered()
        {
            TestMethodSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "GetPrimesUnordered",
                @"winrt::Windows::Foundation::IAsyncActionWithProgress<double> GetPrimesUnordered(int const& first, int const& last);");
        }

        [Test]
        public void CreateNewGuid()
        {
            var member = GetMethod(typeof(GuidClass), m => m.Name == "CreateNewGuid");
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual(@" static winrt::guid CreateNewGuid();", sig);
        }

        [Test]
        public void ObjectIndentical()
        {
            var member = GetMethod(typeof(GuidClass), m => m.Name == "ObjectIndentical");
            var sig = formatter.GetDeclaration(member);
            Assert.AreEqual(@"bool ObjectIndentical(winrt::guid const& objGuid1, winrt::guid const& objGuid2);", sig);
        }

        [Test]
        [Category("Method")]
        public void Method_DefaultParameters()
        {
            TestMethodSignature(CSharpTestLib, "Mono.DocTest.Widget", "Default",
                @"void Default(int const& a = 1, int const& b = 2);");
        }

        [Test]
        [Category("Method")]
        public void Method_RefType()
        {
            TestMethodSignature(CppCxTestLibName, "Namespace222.App", "SetWindow1",
                @"void SetWindow1(winrt::Windows::UI::Core::CoreWindow const& window);");
        }

        [Test]
        [Category("Method")]
        public void Method_WinRtTypeInterfaceImplementation()
        {
            TestMethodSignature(CppCxTestLibName, "Namespace222.App", "SetWindow",
                @"void SetWindow(winrt::Windows::UI::Core::CoreWindow const& window);");
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



        [Test]
        [Category("Event")]
        public void Event_Class1_primeFoundEvent()
        {
            var expectedSig = @"// Register
event_token primeFoundEvent(UwpTestWinRtComponentCpp::PrimeFoundHandler const& handler) const;

// Revoke with event_token
void primeFoundEvent(event_token const* cookie) const;

// Revoke with event_revoker
primeFoundEvent_revoker primeFoundEvent(auto_revoke_t, UwpTestWinRtComponentCpp::PrimeFoundHandler const& handler) const;";
            TestEventSignature(CppCxTestLibName, "UwpTestWinRtComponentCpp.Class1", "primeFoundEvent", expectedSig);
        }

        #region NoSupport
        [Test]
        [Category("Properties")]
        public void Property_Class3_LongProperty()
        {
            TestPropertySignature(CppCxTestLibName, "Namespace2.Class3", "LongProperty", @"long LongProperty();

void LongProperty(long __set_formal);");
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
