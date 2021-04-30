using System;
using Mono.Cecil;
using NUnit.Framework;
using Mono.Documentation.Updater.Formatters.CppFormatters;
using Mono_DocTest_Generic;
using Cpp=Mono_DocTest_Generic;
using Mono_DocTest;

namespace mdoc.Test
{
    public class CppFullFormatterTests: BasicFormatterTests<CppFullMemberFormatter>
    {
        private static readonly CppFullMemberFormatter cppFullMemberFormatter = new CppFullMemberFormatter();
        protected override CppFullMemberFormatter formatter => cppFullMemberFormatter;
        private const string CSharpTestLib = "../../../../external/Test/CSharpExample.dll";
        [Test]
        [Category("Methods")]
        public void MethodSignature_Bar() =>
            TestMethodSignature(typeof(Cpp.Extensions), @"public:
generic <typename T>
[System::Runtime::CompilerServices::Extension]
 static void Bar(IFoo<T> ^ self, System::String ^ s);",
                nameof(Cpp.Extensions.Bar));

        [Test]
        [Category("Methods")]
        public void MethodSignature_ForEach() =>
            TestMethodSignature(typeof(Cpp.Extensions), @"public:
generic <typename T>
[System::Runtime::CompilerServices::Extension]
 static void ForEach(System::Collections::Generic::IEnumerable<T> ^ self, Action<T> ^ a);",
                nameof(Cpp.Extensions.ForEach));

        [Test]
        [Category("Methods")]
        public void MethodSignature_ToEnumerable() =>
            TestMethodSignature(typeof(Cpp.Extensions), @"public:
generic <typename T>
[System::Runtime::CompilerServices::Extension]
 static System::Collections::Generic::IEnumerable<T> ^ ToEnumerable(T self);",
                nameof(Cpp.Extensions.ToEnumerable));

        [Test]
        [Category("Methods")]
        public void MethodSignature_ToDouble() =>
            TestMethodSignature(typeof(Cpp.Extensions), @"public:
generic <typename T>
 where T : IFoo<T>[System::Runtime::CompilerServices::Extension]
 static double ToDouble(T val);",
                nameof(Cpp.Extensions.ToDouble));

        [Test]
        [Category("Methods")]
        public void MethodSignature_BaseMethod() =>
            TestMethodSignature(typeof(GenericBase<>), @"public:
generic <typename S>
 U BaseMethod(S genericParameter);",
                nameof(GenericBase<int>.BaseMethod));

        [Test]
        [Category("Methods")]
        public void MethodSignature_Method() =>
            TestMethodSignature(typeof(IFoo<>), @"public:
generic <typename U>
 T Method(T t, U u);",
                nameof(IFoo<int>.Method));

        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstInt() =>
    TestFieldSignature(typeof(GenericBase<>),
        "public: const int ConstInt;",
        nameof(GenericBase<int>.ConstInt));

        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstLong() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const long ConstLong;",
                nameof(GenericBase<int>.ConstLong));

        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstDecimal() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const System::Decimal ConstDecimal;",
                nameof(GenericBase<int>.ConstDecimal));

        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstShort() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const short ConstShort;",
                nameof(GenericBase<int>.ConstShort));
        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstUint16() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const System::UInt16 ConstUint16;",
                nameof(GenericBase<int>.ConstUint16));
        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstUint32() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const System::UInt32 ConstUint32;",
                nameof(GenericBase<int>.ConstUint32));
        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstUint64() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const System::UInt64 ConstUint64;",
                nameof(GenericBase<int>.ConstUint64));
        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstFloat() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const float ConstFloat;",
                nameof(GenericBase<int>.ConstFloat));
        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstBool() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const bool ConstBool;",
                nameof(GenericBase<int>.ConstBool));

        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstChar() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const char ConstChar;",
                nameof(GenericBase<int>.ConstChar));

        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstObject() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const System::Object ^ ConstObject;",
                nameof(GenericBase<int>.ConstObject));


        [Test]
        [Category("Fields")]
        public void FieldSignature_ConstString() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: const System::String ^ ConstString;",
                nameof(GenericBase<int>.ConstString));

        [Test]
        [Category("Methods")]
        public void MethodSignature_AsReadOnly() =>
            TestMethodSignature(typeof(Array1), @"public:
generic <typename T>
 static System::Collections::ObjectModel::ReadOnlyCollection<T> ^ AsReadOnly(cli::array <T> ^ array);",
                nameof(Array1.AsReadOnly));

        [Test]
        [Category("Methods")]
        public void MethodSignature_UseT() =>
            TestMethodSignature(typeof(MyList<>.Helper<,>), @"public:
 void UseT(T a, U b, V c);",
                nameof(MyList<int>.Helper<int,int>.UseT));

        [Test]
        [Category("Methods")]
        public void MethodSignature_s() =>
            TestMethodSignature(typeof(Widget), @"public:
 Func<System::String ^, System::Object ^> ^ Dynamic2(Func<System::String ^, System::Object ^> ^ value);",
                nameof(Widget.Dynamic2));

        [Test]
        [Category("Methods")]
        public void MethodSignature_M2() =>
            TestMethodSignature(typeof(Widget), @"public:
 void M2(cli::array <short> ^ x1, cli::array <int, 2> ^ x2, cli::array <cli::array <long> ^> ^ x3);",
                nameof(Widget.M2));

        [Test]
        [Category("Methods")]
        public void MethodSignature_PointersOnHat_JaggedArray_M5() =>
            TestMethodSignature(typeof(Widget), @"protected:
 void M5(void* pv, cli::array <cli::array <double, 2> ^> ^* pd);",
                "M5");

        [Test]
        [Category("Methods")]
        public void MethodSignature_PointersOnHat_ReferenceType_M55() =>
            TestMethodSignature(typeof(Widget), @"protected:
 void M55(void* pv, System::String ^* pd);",
                "M55");

        [Test]
        [Category("Methods")]
        public void MethodSignature_GenericRefParams_RefMethod() =>
            TestMethodSignature(typeof(MyList<>), @"public:
generic <typename U>
 void RefMethod(T % t, U % u);",
                nameof(MyList<int>.RefMethod));

        [Test]
        [Category("Methods")]
        public void MethodSignature_RefParams_M1() =>
            TestMethodSignature(typeof(Widget), @"public:
 void M1(long c, [Runtime::InteropServices::Out] float % f, Mono_DocTest::DocValueType % v);",
                nameof(Widget.M1));

        [Test]
        [Category("Methods")]
        public void MethodSignature_RefParamsWithHat_RefMethod() =>
            TestMethodSignature(typeof(Array1), @"public:
generic <typename T>
 static void Resize(cli::array <T> ^ % array, int newSize);",
                nameof(Array1.Resize));

        [Test]
        [Category("Methods")]
        public void MethodSignature_Reset() =>
            TestMethodSignature(typeof(MyList1<,>), @"public:
 virtual void Reset();",
                nameof(MyList1<int,int>.Reset));

        [Test]
        [Category("Methods")]
        public void MethodSignature_GetEnumerator1() =>
            TestMethodSignature(typeof(MyList<>), @"public:
 virtual System::Collections::IEnumerator ^ GetEnumerator1() = System::Collections::IEnumerable::GetEnumerator;",
                nameof(MyList<int>.GetEnumerator1));

        [Test]
        [Category("Methods")]
        public void MethodSignature_GetEnumeratorIEnumerable() =>
            TestMethodSignature(typeof(MyList1<,>), @"public:
 virtual System::Collections::IEnumerator ^ GetEnumerator1() = System::Collections::IEnumerable::GetEnumerator;",
                nameof(MyList1<int,int>.GetEnumerator1));

        [Test]
        [Category("Methods")]
        public void MethodSignature_GetEnumeratorGenericIEnumerable()
        {
            TestMethodSignature(
                typeof(MyList1<,>), @"public:
 virtual System::Collections::Generic::IEnumerator<A> ^ GetEnumerator() = System::Collections::Generic::IEnumerable<A>::GetEnumerator;",
                nameof(MyList1<int, int>.GetEnumerator));
        }

        [Test]
        [Category("Methods")]
        public void MethodSignature_opAddition() =>
            TestMethodSignature(typeof(Widget), @"public:
 static Mono_DocTest::Widget ^ operator +(Mono_DocTest::Widget ^ x1, Mono_DocTest::Widget ^ x2);",
                "op_Addition");

        [Test]
        [Category("Methods")]
        [Category("NoSupport")]
        public void MethodSignature_NoSupport_DefaultValue() =>
            TestMethodSignature(CSharpTestLib, "Mono.DocTest.Widget", "Default",
                null);

        [Test]
        [Category("NoSupport")]
        public void NoSupport_Exception_NestedClassWithSameName()
        {
            TestTypeSignature(CSharpTestLib, "Mono.DocTest.Widget/NestedClass", null);
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_Widget()
        {
            TestTypeSignature(CSharpTestLib, 
                "Mono.DocTest.DocValueType", "public value class Mono::DocTest::DocValueType : Mono::DocTest::IProcess");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_Single()
        {
            TestTypeSignature(typeof(Single), 
                "public value class float : IComparable, IComparable<float>, IConvertible, IEquatable<float>, IFormattable");
        }

        [Test]
        [Category("Type")]
        public void TypeSignature_Int32()
        {
            TestTypeSignature(typeof(Int32), 
                "public value class int : IComparable, IComparable<int>, IConvertible, IEquatable<int>, IFormattable");
        }

        [Test]
        [Category("Methods")]
        public void MethodSignature_opExplicit() =>
            TestMethodSignature(typeof(Widget), @"public:
 static explicit operator int(Mono_DocTest::Widget ^ x);",
                "op_Explicit");

        [Test]
        [Category("Methods")]
        public void MethodSignature_opImplicit() =>
            TestMethodSignature(typeof(Widget), @"public:
 static operator long(Mono_DocTest::Widget ^ x);",
                "op_Implicit");

        [Test]
        [Category("Methods")]
        public void MethodSignature_constructor()
        {
            TestMethodSignature(typeof(Widget), @"public:
 Widget(Converter<System::String ^, System::String ^> ^ c);",
        ".ctor");
        }

        [Test]
        [Category("Methods")]
        public void MethodSignature_ParamsKeyword_M6()
        {
            TestMethodSignature(typeof(Widget), @"protected:
 void M6(int i, ... cli::array <System::Object ^> ^ args);",
                "M6");
        }
        
        protected override TypeDefinition GetType(Type type)
        {
            var moduleName = type.Module.FullyQualifiedName;

            var tref = GetType(moduleName, type.FullName.Replace("+", "/"));
            return tref;
        }
    }
}
