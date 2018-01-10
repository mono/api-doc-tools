using System;
using Mono.Cecil;
using NUnit.Framework;
using Mono.Documentation.Updater.Formatters.CppFormatters;
using Mono_DocTest_Generic;
using Mono_DocTest;

namespace mdoc.Test
{
    public class CppFormatterTests : BasicFormatterTests<CppMemberFormatter>
    {
        #region Types
        private static readonly CppMemberFormatter cppMemberFormatter = new CppMemberFormatter();
        protected override CppMemberFormatter formatter => cppMemberFormatter;

        [Test]
        [Category("Types")]
        public void TypeSignature_Extensions() =>
            TestTypeSignature(typeof(Extensions), @"public ref class Extensions abstract sealed");

        [Test]
        [Category("Types")]
        public void TypeSignature_GenericBase() =>
            TestTypeSignature(typeof(GenericBase<>), @"generic <typename U>
public ref class GenericBase");

        [Test]
        [Category("Types")]
        public void TypeSignature_MyList() =>
            TestTypeSignature(typeof(MyList<>), @"generic <typename T>
public ref class MyList : Mono_DocTest_Generic::GenericBase<T>, System::Collections::Generic::IEnumerable<cli::array <int> ^>");

        [Test]
        [Category("Types")]
        public void TypeSignature_IFoo() =>
            TestTypeSignature(typeof(IFoo<>), @"generic <typename T>
public interface class IFoo");

        [Test]
        [Category("Types")]
        public void TypeSignature_MyList1() =>
            TestTypeSignature(typeof(MyList1<,>), @"generic <typename A, typename B>
public ref class MyList1 : Mono_DocTest_Generic::GenericBase<System::Collections::Generic::Dictionary<A, B> ^>, Mono_DocTest_Generic::IFoo<A>, System::Collections::Generic::ICollection<A>, System::Collections::Generic::IEnumerable<A>, System::Collections::Generic::IEnumerator<A>");

        [Test]
        [Category("Types")]
        public void TypeSignature_Color() =>
            TestTypeSignature(typeof(Color), @"public enum class Color");

        [Test]
        [Category("Types")]
        public void TypeSignature_Nonamespace() =>
            TestTypeSignature(typeof(NoNamespace), @"public ref class NoNamespace");

        [Test]
        [Category("Types")]
        public void TypeSignature_DocAttribute() =>
            TestTypeSignature(typeof(DocAttribute), @"public ref class DocAttribute : Attribute");
        [Test]
        [Category("Types")]
        public void TypeSignature_Environment1() =>
            TestTypeSignature(typeof(Environment1), @"public ref class Environment1 abstract sealed");

        [Test]
        [Category("Types")]
        public void TypeSignature_IProcess() =>
            TestTypeSignature(typeof(IProcess), @"public interface class IProcess");

        [Test]
        [Category("Types")]
        public void TypeSignature_DocValueType() =>
            TestTypeSignature(typeof(DocValueType), @"public value class DocValueType : Mono_DocTest::IProcess");

        [Test]
        [Category("Types")]
        public void TypeSignature_D() =>
            TestTypeSignature(typeof(D), @"public delegate System::Object ^ D(Func<System::String ^, System::Object ^, System::Object ^> ^ value);");


        [Test]
        [Category("Fields")]
        public void FieldSignature_NonFlagEnum() =>
            TestFieldSignature(typeof(DocAttribute),
                "public: Color NonFlagsEnum;",
                nameof(DocAttribute.NonFlagsEnum));

        [Test]
        [Category("Types")]
        public void TypeSignature_Widget() =>
            TestTypeSignature(typeof(Widget), @"public ref class Widget : Mono_DocTest::IProcess");
        
        [Test]
        [Category("Types")]
        public void TypeSignature_FooEventArgs() =>
            TestTypeSignature(typeof(GenericBase<>.FooEventArgs), @"public: ref class GenericBase<U>::FooEventArgs : EventArgs");

        [Test]
        [Category("Fields")]
        public void FieldSignature_StaticField1() =>
            TestFieldSignature(typeof(GenericBase<>),
                "public: static initonly GenericBase<U> ^ StaticField1;",
                nameof(GenericBase<int>.StaticField1));

        [Test]
        [Category("Fields")]
        public void FieldSignature_NonFlagsEnum() =>
            TestFieldSignature(typeof(DocAttribute),
                "public: Color NonFlagsEnum;",
                nameof(DocAttribute.NonFlagsEnum));

        [Test]
        [Category("Fields")]
        public void FieldSignature_Pointers_ppValues() =>
            TestFieldSignature(typeof(Widget),
                "public: float** ppValues;",
                nameof(Widget.ppValues));

        [Test]
        [Category("Fields")]
        public void FieldSignature_Pointers_pCount() =>
            TestFieldSignature(typeof(Widget),
                "public: int* pCount;",
                nameof(Widget.pCount));
        
        [Test]
        [Category("Fields")]
        public void FieldSignature_value() =>
            TestFieldSignature(typeof(Widget.NestedClass),
                "public: int value;",
                nameof(Widget.NestedClass.value));

        [Test]
        [Category("Fields")]
        public void FieldSignature_protectedPublic_monthlyAverage()
        {
            TestFieldSignature(typeof(Widget), @"protected public: initonly double monthlyAverage;", "monthlyAverage");
        }

        [Test]
        [Category("Properties")]
        public void PropertySignature_IndexedPropertyNamed()
        {
            TestPropertySignature(typeof(Widget), @"public:
 property long indexedProperty[long] { long get(long index); void set(long index, long value); };",
                "indexedProperty");
        }

        [Test]
        [Category("Properties")]
        public void PropertySignature_IndexedPropertyDefault()
        {
            TestPropertySignature(typeof(Widget), @"public:
 property int default[System::String ^, int] { int get(System::String ^ s, int i); void set(System::String ^ s, int i, int value); };",
                "Item");
        }
       

        [Test]
        [Category("Properties")]
        public void PropertySignature_Current3() =>
            TestPropertySignature(typeof(MyList1<,>), @"public:
 virtual property System::Object ^ Current3 { System::Object ^ get(); };",
                nameof(MyList1<int,int>.Current3));

        [Test]
        [Category("Properties")]
        public void PropertySignature_B() =>
            TestPropertySignature(typeof(Widget.IMenuItem), @"public:
 property int B { int get(); void set(int value); };",
                nameof(Widget.IMenuItem.B));
        [Test]
        [Category("Properties")]
        public void PropertySignature_Count() =>
            TestPropertySignature(typeof(MyList1<,>), @"public:
 virtual property int Count { int get(); };",
                nameof(MyList1<int, int>.Count));

        [Test]
        [Category("Types")]
        public void TypeSignature_AsyncCallback() =>
            TestTypeSignature(typeof(AsyncCallback), @"public delegate void AsyncCallback(IAsyncResult ^ ar);");

        #endregion

        protected override TypeDefinition GetType(Type type)
        {
            var moduleName = type.Module.FullyQualifiedName;

            var tref = GetType(moduleName, type.FullName.Replace("+", "/"));
            return tref;
        }

    }
}
