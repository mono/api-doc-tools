#include "stdafx.h"
#include "TestClass.h"
#include <string>
#include <vector>

using namespace std;
using namespace cli;
using namespace System::Collections::ObjectModel;
using namespace System;
using namespace Runtime::InteropServices;
using namespace System::Runtime::InteropServices;
namespace Generic1 = System::Collections::Generic;
using namespace System::Runtime::CompilerServices;

//using namespace System::att;




TestClass::TestClass()
{
}


TestClass::~TestClass()
{
}

/// <summary>Namespace Test: [<see cref="N:Mono.DocTest" />] <see href="http://www.mono-project.com/">Mono Project</see></summary>
/// <remarks><c>T:NoNamespace</c></remarks>
public ref class NoNamespace {};

namespace System {

	/// <remarks><c>T:System.Action`1</c></remarks>
	generic<typename T>
		[SerializableAttribute]
		public delegate void Action(T obj);
		
		//TODO: Env1 name as not supported
		/// <remarks><c>T:System.Environment</c></remarks>
		public ref class Environment1 abstract sealed {
			//public: Environment1() {};
		public:
			/// <remarks><c>T:System.Environment+SpecialFolder</c></remarks>
			enum class SpecialFolder {};

			/// <param name="folder">
			///   A <see cref="T:System.Environment+SpecialFolder" /> instance.
			/// </param>
			/// <remarks>
			///   <c>M:System.Environment.GetFolderPath(System.Environment+SpecialFolder)</c>
			/// </remarks>
		public:
			static SpecialFolder GetFolderPath(SpecialFolder folder) {
				//throw std::runtime_error("error");
				return folder;

			};

			// Testing whether this extension method shows up for System.Array				
		public:
			generic<typename T>
				where T : value class
					[System::Runtime::CompilerServices::Extension]
				static bool IsAligned(cli::array<T>^ vect, int index)
				{
					return false;
				};


		};

		// to test ECMA doc importing...
		//todo: array1 instad real name
		public ref class Array1 {
			// the ECMA docs have a different return type than .NET -- skip.
		public:
			//todo: cli::array or std::array
			generic<typename T>
				static ReadOnlyCollection<T> ^ AsReadOnly(cli::array<T>^ array)
				{
					throw gcnew NotImplementedException();
				}

				// ECMA docs use <T,U> instead of <TInput,TOutput> --> map them.
		public:
			generic<typename TInput, typename TOutput>
				static cli::array<TOutput>^ ConvertAll(cli::array<TInput>^ array, Converter<TInput, TOutput>^ converter)
				{
					throw gcnew InvalidOperationException();
				};

				// ECMA docs *incorrectly* document parameter -- skip
		public:
			generic<typename T>
				static void Resize(cli::array<T> ^ %  array, int newSize)
				{
					throw gcnew Exception();
				}
		};

		// to test ECMA doc importing...
		//public delegate void AsyncCallback(IAsyncResult^ ar);
}

//todo: no dot in namespace name
namespace Mono_DocTest {
	//todo: no internal modifier
	class Internal {
		class ShouldNotBeDocumented {
		};
	};

	//todo: no internal modifier
	ref class MonoTODOAttribute : public System::Attribute {
	};

	public ref class CustomException : System::Exception {
		protected: System::ArgumentNullException ArgumentNullExceptionField;
	};

	public delegate void DelegateWithNetSystemType(System::Exception parameter);

		generic<typename T>
		[SerializableAttribute]
		public delegate void Action22(T obj);

	/// <remarks>
	///  <para>
	///   cref=<c>T:Mono.DocTest.DocAttribute</c>.
	///  </para>
	///  <format type="text/html">
	///   <table width="100%">
	///     <tr>
	///       <td style="color:red">red</td>
	///       <td style="color:blue">blue</td>
	///       <td style="color:green">green</td>
	///     </tr>
	///   </table>
	///  </format>
	///  <code lang="C#" src="../DocTest.cs#DocAttribute Example" />
	/// </remarks>
	[AttributeUsageAttribute(AttributeTargets::All)]
	public ref class DocAttribute : Attribute {
				/// <remarks><c>C:Mono.DocTest.DocAttribute(System.String)</c></remarks>
	public:	DocAttribute(String ^ docs)
	{
		if (String::IsNullOrEmpty(docs))
			throw gcnew ArgumentNullException();
	};

	private:
		Type^ quantity;
		/// <remarks><c>P:Mono.DocTest.DocAttribute.Property</c></remarks>
	public:
		property Type^ Property {
			Type^ get() { return quantity; }
			void set(Type^ value)
			{
				quantity = value;
			}};

		/// <remarks><c>F:Mono.DocTest.DocAttribute.Field</c></remarks>
		bool Field;
		/// <remarks><c>F:Mono.DocTest.DocAttribute.FlagsEnum</c></remarks>
		ConsoleModifiers FlagsEnum;
		/// <remarks><c>F:Mono.DocTest.DocAttribute.NonFlagsEnum</c></remarks>
		Color NonFlagsEnum;
	};

	/// <summary>Possible colors</summary>
	/// <remarks>
	///   <see cref="T:Mono.DocTest.Color"/>.
	///   Namespace Test: [<see cref="N:Mono.DocTest" />]
	/// </remarks>
	/*[MonoTODO]
	public enum Color {
	/// <summary>Insert Red summary here</summary>
	///<remarks><c>F:Mono.DocTest.Color.Red</c>.</remarks>
	Red,
	///<summary>Insert Blue summary here</summary>
	///<remarks><c>F:Mono.DocTest.Color.Blue</c>.</remarks>
	Blue,
	///<summary>Insert Green summary here</summary>
	///<remarks><c>F:Mono.DocTest.Color.Green</c>.</remarks>
	Green,

	AnotherGreen = Green,
	};*/

	/// <summary>Process interface</summary>
	/// <remarks><c>T:Mono.DocTest.IProcess</c>.</remarks>
	public interface class IProcess
	{

	};

	/// <summary>Process interface</summary>
	/// <remarks><c>T:Mono.DocTest.DocValueType</c>.</remarks>	
	public value class DocValueType : IProcess {

	public:
		/// <remarks><c>F:Mono.DocTest.DocValueType.total</c>.</remarks>
		int total;

	public:
		/// <param name="i">A <see cref="T:System.Int32" />.</param>
		/// <remarks><see cref="M:Mono.DocTest.DocValueType.M(System.Int32)"/>.</remarks>
		void M(int i)
		{
			if (((gcnew Random())->Next() % 2) == 0)
				throw gcnew SystemException();
			throw gcnew ApplicationException();
		}
	};

	public value class ValueClassSpecificField {
	public: DocValueType ExceptionField;
	};
		
	/// <remarks><c>T:Mono.DocTest.D</c></remarks>
	public delegate Object ^ D(Func<String ^ , Object ^ , Object ^ > ^ value );

	/// <remarks><c>T:Mono.DocTest.Widget</c>.</remarks>
	/// <seealso cref="P:Mono.DocTest.Widget.Item(System.Int32)" />
	/// <extra>Some extra tag value</extra>	
	public ref class Widget : IProcess {	
	
		//public: virtual double getVolume() { return 0; };
		/// <remarks><c>T:Mono.DocTest.Widget.NestedClass</c>.</remarks>
	public: ref class NestedClass {
		/// <remarks><c>F:Mono.DocTest.Widget.NestedClass.value</c>.</remarks>
	public: int value;
			/// <param name="i">Some <see cref="T:System.Int32" />.</param>
			/// <remarks><c>M:Mono.DocTest.Widget.NestedClass.M(System.Int32)</c>.</remarks>
			void M(int i) {};
			/// <remarks><c>T:Mono.DocTest.Widget.NestedClass.Double</c>.</remarks>
			ref class Double {
				/// <remarks><c>T:Mono.DocTest.Widget.NestedClass.Double.Triple</c>.</remarks>
			public: ref class Triple {
				/// <remarks><c>T:Mono.DocTest.Widget.NestedClass.Double.Triple.Quadruple</c>.</remarks>
			public: ref class Quadruple {};// for good measure
			};
			};
	};
	public: enum class NestedEnum { Value1, Value2};

			/// <remarks><c>T:Mono.DocTest.Widget.NestedClass`1</c>.</remarks>

	public:
		//todo: cannot use the same class name here with generic par-r
		generic<typename T>
			ref class NestedClass1 {
				/// <remarks><c>F:Mono.DocTest.Widget.NestedClass`1.value</c>.</remarks>
			public: int value;

					/// <param name="i">Another <see cref="T:System.Int32" />.</param>
					/// <remarks><c>M:Mono.DocTest.Widget.NestedClass`1.M(System.Int32)</c>.</remarks>
			public: void M(int i) {};
			};

			/// <remarks><c>F:Mono.DocTest.Widget.classCtorError</c>.</remarks>

	public: static initonly cli::array<String^>^ classCtorError = CreateArray();

	private: static cli::array<String^>^ CreateArray()
	{
		throw gcnew NotSupportedException();
	};

			 /// <remarks><c>F:Mono.DocTest.Widget.message</c>.</remarks>
	public: String^ message;

			/// <remarks><c>F:Mono.DocTest.Widget.defaultColor</c>.</remarks>
	protected: static Color defaultColor;

			   /// <remarks><c>F:Mono.DocTest.Widget.PI</c>.</remarks>
			   //TODO: no internal
	protected:  const double PI = 3.14159;

				/// <remarks><c>F:Mono.DocTest.Widget.monthlyAverage</c>.</remarks>
				
	protected public: initonly double monthlyAverage;

				 /// <remarks><c>F:Mono.DocTest.Widget.array1</c>.</remarks>
	public: cli::array<long^>^ array1;

			/// <remarks><c>F:Mono.DocTest.Widget.array2</c>.</remarks>
			//todo: check if works correctly
	public: cli::array<Widget^, 2> ^  array2;

			//TODO: no possibiiti for unsafe 
			/// <remarks><c>F:Mono.DocTest.Widget.pCount</c>.</remarks>
	public:  int *pCount;

			 //TODO: no possibiiti for unsafe 
			 /// <remarks><c>F:Mono.DocTest.Widget.ppValues</c>.</remarks>
	public:  float** ppValues;

			 /// <remarks><c>T:Mono.DocTest.Widget.IMenuItem</c>.</remarks>
	public: interface class IMenuItem {
		/// <remarks><c>M:Mono.DocTest.Widget.IMenuItem.A</c>.</remarks>
		void A();

		/// <remarks><c>P:Mono.DocTest.Widget.IMenuItem.P</c>.</remarks>
		property int B {
			int get();
			void set(int value);
		};
	};

			/// <remarks><c>T:Mono.DocTest.Widget.Del</c>.</remarks>
	public: delegate void Del(int i);

			/// <remarks><c>T:Mono.DocTest.Widget.Direction</c>.</remarks>
			//todo: no internal
	protected:
		[FlagsAttribute]
		/*internal*/ enum class Direction {
			/// <remarks><c>T:Mono.DocTest.Widget.Direction.North</c>.</remarks>
			North,
			/// <remarks><c>T:Mono.DocTest.Widget.Direction.South</c>.</remarks>
			South,
			/// <remarks><c>T:Mono.DocTest.Widget.Direction.East</c>.</remarks>
			East,
			/// <remarks><c>T:Mono.DocTest.Widget.Direction.West</c>.</remarks>
			West,
		};

		/// <remarks>
		///  <para><c>C:Mono.DocTest.Widget</c>.</para>
		///  <para><c>M:Mono.DocTest.Widget.#ctor</c>.</para>
		///  <para><see cref="C:Mono.DocTest.Widget(System.String)" /></para>
		///  <para><see cref="C:Mono.DocTest.Widget(System.Converter{System.String,System.String})" /></para>
		/// </remarks>
	public: Widget() {};

			/// <param name="s">A <see cref="T:System.String" />.</param>
			/// <remarks>
			///  <para><c>C:Mono.DocTest.Widget(System.String)</c>.</para>
			///  <para><c>M:Mono.DocTest.Widget.#ctor(System.String)</c>.</para>
			/// </remarks>
	public: Widget(String^ s) {};

			/// <param name="c">A <see cref="T:System.Converter{System.String,System.String}" />.</param>
			/// <remarks>
			///  <para><c>C:Mono.DocTest.Widget(System.Converter{System.String,System.String})</c>.</para>
			/// </remarks>
	public: Widget(Converter<String^, String^>^ c) {};

			/// <remarks><c>M:Mono.DocTest.Widget.M0</c>.</remarks>
	public: static void M0() {};

			/// <param name="c">A <see cref="T:System.Char" />.</param>
			/// <param name="f">A <see cref="T:System.Single" />.</param>
			/// <param name="v">A <see cref="T:Mono.DocTest.DocValueType" />.</param>
			/// <remarks><c>M:Mono.DocTest.Widget.M1(System.Char,System.Signle@,Mono.DocTest.DocValueType@)</c>.</remarks>
			/// //TODO: doc attribute is not working
	public:
		[DocAttribute("normal DocAttribute", Field = true)]
		//[return:Doc("return:DocAttribute", Property = typeof(Widget))]
		void M1([Doc("c", FlagsEnum = ConsoleModifiers::Alt | ConsoleModifiers::Control)] long c,
			[Doc("f", NonFlagsEnum = Color::Red)][Runtime::InteropServices::Out]  float % f,
			[DocAttribute("v")]  DocValueType % v) {
			f = 0;
		};

		/// <param name="x1">A <see cref="T:System.Int16" /> array.</param>
		/// <param name="x2">A <see cref="T:System.Int32" /> array.</param>
		/// <param name="x3">A <see cref="T:System.Int64" /> array.</param>
		/// <remarks><c>M:Mono.DocTest.Widget.M2(System.Int16[],System.Int32[0:,0:],System.Int64[][])</c>.</remarks>
	public: void M2(cli::array<short>^ x1, cli::array<int, 2>^ x2, cli::array<cli::array<long >^ >^ x3) {};

			/// <param name="x3">Another <see cref="T:System.Int64" /> array.</param>
			/// <param name="x4">A <see cref="T:Mono.DocTest.Widget" /> array.</param>
			/// <remarks><c>M:Mono.DocTest.Widget.M3(System.Int64[][],Mono.DocTest.Widget[0:,0:,0:][])</c>.</remarks>
	protected: void M3(cli::array<cli::array<long >^ >^ x3, cli::array<cli::array<Widget^, 3>^>^ x4) {};

			   //TODO: no unsafe
			   /// <param name="pc">A <see cref="T:System.Char" /> pointer.</param>
			   /// <param name="ppf">A <see cref="T:Mono.DocTest.Color" /> pointer.</param>
			   /// <remarks><c>M:Mono.DocTest.Widget.M4(System.Char*,Mono.DocTest.Color**)</c>.</remarks>
	protected: void M4(char *pc, Color **ppf) {};

			   //TODO: no unsafe
			   /// <param name="pv">A <see cref="T:System.Void" /> pointer.</param>
			   /// <param name="pd">A <see cref="T:System.Double" /> array.</param>
			   /// <remarks><c>M:Mono.DocTest.Widget.M5(System.Void*,System.Double*[0:,0:][])</c>.</remarks>
	protected: void M5(void *pv, cli::array<cli::array<double, 2>^>^*pd) {};

			   protected: void M55(void *pv, System::String ^ *pd) {};

			   /// <param name="i">Yet another <see cref="T:System.Int32" />.</param>
			   /// <param name="args">An <see cref="T:System.Object" /> array.</param>
			   /// <remarks><c>M:Mono.DocTest.Widget.M6(System.Int32,System.Object[])</c>.</remarks>
	protected: void M6(int i, ... cli::array<Object^>^ args) {};

			   /// <remarks><c>M:Mono.DocTest.Widget.M7(Mono.DocTest.Widget.NestedClass.Double.Triple.Quadruple)</c>.</remarks>
	public: void M7(Widget::NestedClass::Double::Triple::Quadruple ^ a) {};



			/// <value>A <see cref="T:System.Int32" /> value...</value>
			/// <remarks><c>P:Mono.DocTest.Widget.Width</c>.</remarks>

	public:
		[DocAttribute("Width property")]
		property int Width {
			[Doc("Width get accessor")]
			int get() { return 0; };

		protected:
			[Doc("Width set accessor")]
			void set(int value) {};
		};

		/// <value>A <see cref="T:System.Int64" /> value...</value>
		/// <remarks><c>P:Mono.DocTest.Widget.Height</c>.</remarks>
	protected:
		[Doc("Height property")]
		property long Height {  long get() { return 0; }; };

		/// <value>A <see cref="T:System.Int16" /> value...</value>
		/// <remarks><c>P:Mono.DocTest.Widget.X</c>.</remarks>
		//todo: no internal (protected internal)
	protected: property short X { void set(short value) {}; };

			   /// <value>A <see cref="T:System.Double" /> value...</value>
			   /// <remarks><c>P:Mono.DocTest.Widget.Y</c>.</remarks>
			   //todo: no internal(protected internal)
	protected: property double Y {
		double get() { return 0; };
		void set(double value) {};
	};


			   /// <param name="i">TODO</param>
			   /// <remarks><c>P:Mono.DocTest.Widget.Item(System.Int32)</c>.</remarks>
			   /// <value>A <see cref="T:System.Int32" /> instance.</value>

	public:
		[DocAttribute("Item property")]
		property int default[int]{
			int get(int index) { return 0; };

		[Doc("Item property set accessor")]
		void set(int index, int value) {};
		};

		public:
			[DocAttribute("Item property")]
			property long indexedProperty[long]{
				long get(long index) { return 0; };

			[Doc("Item property set accessor")]
			void set(long index, long value) {};
			};

		/// <param name="s">Some <see cref="T:System.String" />.</param>
		/// <param name="i">I love <see cref="T:System.Int32" />s.</param>
		/// <remarks><c>P:Mono.DocTest.Widget.Item(System.String,System.Int32)</c>.</remarks>
		/// <value>A <see cref="T:System.Int32" /> instance.</value>
	public:
		property int default[System::String ^, int]
		{ int get(System::String ^ s, int i) { return 0; }
		void set(System::String ^ s, int i, int value) {};
		};

		/// <remarks><c>E:Mono.DocTest.Widget.AnEvent</c>.</remarks>
	public:
		[Doc("Del event")]
		event Del^ AnEvent {
			[Doc("Del add accessor")]
			void add(Del^ name) {};
			[Doc("Del remove accessor")]
			void remove(Del^ name) {};
			void raise(int i) {};
		};

		/// <remarks><c>E:Mono.DocTest.Widget.AnotherEvent</c>.</remarks>
	protected: event Del^ AnotherEvent;


			   /// <param name="x">Another <see cref="T:Mono.DocTest.Widget" />.</param>
			   /// <remarks><c>M:Mono.DocTest.Widget.op_UnaryPlus(Mono.DocTest.Widget)</c>.</remarks>
			   /// <returns>A <see cref="T:Mono.DocTest.Widget" /> instance.</returns>
	public: static Widget^ operator + (Widget x) { return nullptr; }

			/// <remarks><c>M:Mono.DocTest.Widget.op_Division</c>.</remarks>
			/// <returns>A <see cref="T:Mono.DocTest.Widget" /> instance.</returns>
			//todo": added 1 to compile
	public: static Widget^  op_Division1 = nullptr;

			/// <param name="x1">Yet Another <see cref="T:Mono.DocTest.Widget" />.</param>
			/// <param name="x2">Yay, <see cref="T:Mono.DocTest.Widget" />s.</param>
			/// <remarks><c>M:Mono.DocTest.Widget.op_Addition(Mono.DocTest.Widget,Mono.DocTest.Widget)</c>.</remarks>
			/// <returns>A <see cref="T:Mono.DocTest.Widget" /> instance (2).</returns>
	public: static Widget^ operator+ (Widget x1, Widget x2) { return nullptr; }

			/// <param name="x"><see cref="T:Mono.DocTest.Widget" />s are fun!.</param>
			/// <remarks><c>M:Mono.DocTest.Widget.op_Explicit(Mono.DocTest.Widget)~System.Int32</c>.</remarks>
			/// <returns>A <see cref="T:System.Int32" /> instance.</returns>
	public: static explicit operator int(Widget^ x) { return 0; }

			/// <param name="x"><c>foo</c>; <see cref="T:Mono.DocTest.Widget" />.</param>
			/// <remarks><c>M:Mono.DocTest.Widget.op_Implicit(Mono.DocTest.Widget)~System.Int64</c>.</remarks>
			/// <returns>A <see cref="T:System.Int64" /> instance.</returns>
			//todo: no implicit(default behavior)
	public: static operator long(Widget x) { return 0; }

			/// <remarks><c>M:Mono.DocTest.Widget.Default(System.Int32,System.Int32)</c></remarks>c
			//todo: no default value
	public: void Default(
		[System::Runtime::InteropServices::Optional]
			/*[System::Runtime::InteropServices::DefaultParameterValueAttribute(1)]*/int a,
				[System::Runtime::InteropServices::Optional]
			/*[System::Runtime::InteropServices::DefaultParameterValueAttribute(2)]*/int b) {};

			/// <remarks><c>M:Mono.DocTest.Widget.Default(System.String,System.Char)</c></remarks>
			//todo: no default value
	public: void Default(/*[System::Runtime::InteropServices::DefaultParameterValueAttribute("a")]*/string a, /*[System::Runtime::InteropServices::DefaultParameterValueAttribute('b')]*/char b) {};

			//TODO: no dynamics - use Object instead/ + no + operator
			/// <remarks><c>M:Mono.DocTest.Widget.Dynamic0(System.Object,System.Object)</c></remarks>
	public: Object^ Dynamic0(Object^ a, Object^ b) { return gcnew Object(); }



			//TODO: no dynamics - use Object instead
			/// <remarks><c>M:Mono.DocTest.Widget.Dynamic1(System.Collections.Generic.Dictionary{System.Object,System.Object})</c></remarks>
	public: Generic1::Dictionary<Object^, System::String^> ^ Dynamic1(Generic1::Dictionary<Object^, System::String^>^ value) { return value; };

			//TODO: no dynamics - use Object instead
			/// <remarks><c>M:Mono.DocTest.Widget.Dynamic2(System.Func{System.String,System.Object})</c></remarks>
	public: Func<String^, Object^>^ Dynamic2(Func<String^, Object^>^ value) { return value; };

			//TODO: no dynamics - use Object instead
			/// <remarks><c>M:Mono.DocTest.Widget.Dynamic3(System.Func{System.Func{System.String,System.Object},System.Func{System.Object,System.String}})</c></remarks>
	public: Func<Func<String^, Object^>^, Func< Object^, String^>^>^ Dynamic3(
		Func<Func<String^, Object^>^, Func< Object^, String^>^>^ value) {
		return value;
	};

			//TODO: no dynamics - use Object instead
			/// <remarks><c>P:Mono.DocTest.Widget.DynamicP</c></remarks>
			/*public: property Func<Func<String^, Object^, String^>^, Func<Object^, Func<Object^>, String^>^> ^DynamicP{
			Func<Func<String^, Object^, String^>^, Func<Object^, Func<Object^>, String^>^> get(){ return nullptr; };
			};*/

			//TODO: no dynamics - use Object instead
			/// <remarks><c>F:Mono.DocTest.Widget.DynamicF</c></remarks>
	public: Func<Func<String^, Object^, String^>^, Func<Object^, Func<Object^>^, String^>^> ^DynamicF;

			//TODO: no dynamics - use Object instead + use delegate as pure Func cannot be used
			/// <remarks><c>E:Mono.DocTest.Widget.DynamicE1</c></remarks>

	public: [Obsolete("why not")] event Func<Object^>^ DynamicE1;

			//TODO: no dynamics - use Object instead 
			/// <remarks><c>E:Mono.DocTest.Widget.DynamicE2</c></remarks>
	public: event Func<Object^>^ DynamicE2 {
		[Doc("Del add accessor")]
		void add(Func<Object^>^ name) {};
		[Doc("Del remove accessor")]
		void remove(Func<Object^>^ name) {};
		Object^ raise() { return gcnew Object(); };
	};


	};

	/// <remarks><c>T:Mono.DocTest.UseLists</c>.</remarks>
	public ref class UseLists
	{
		/// <param name="list">A <see cref="T:Mono.DocTest.Generic.MyList{System.Int32}" />.</param>
		/// <remarks><c>M:Mono.DocTest.UseLists.Process(Mono.DocTest.MyList{System.Int32})</c>.</remarks>

	public: void Process(Mono_DocTest_Generic::MyList<int> ^ list) {};

			/// <param name="value">A <c>T</c>.</param>
			/// <typeparam name="T">Something</typeparam>
			/// <remarks><c>M:Mono.DocTest.UseLists.GetValues``1(``0)</c>.</remarks>
			/// <returns>A <see cref="T:Mono.DocTest.Generic.MyList`1" /> instance.</returns>

	public:
		generic<typename T>
			where T : value class
				Mono_DocTest_Generic::MyList<T>^ GetValues(T value) { return nullptr; };

			/// <param name="list">Another <see cref="T:Mono.DocTest.Generic.MyList{System.Int32}" />.</param>
			/// <remarks>
			///  <para><c>M:Mono.DocTest.UseLists.Process(System.Collections.Generic.List{System.Int32})</c>.</para>
			/// <para><see cref="M:System.Collections.Generic.List{System.Int32}.Remove(`0)" /></para>
			/// </remarks>
			/// <exception name="Whatever">text!</exception>
			/// <exception invalid="foo">text!</exception>
	public: void Process(Generic1::List<int> list)
	{
		// Bug: only creation is looked for, so this generates an <exception/>
		// node:
		gcnew Exception();

		// Bug? We only look at "static" types, so we can't follow
		// delegates/interface calls:

		//todo:uncomment
		/*Func<int, int>^ a = x = > {throw gcnew InvalidOperationException(); };
		a(1);*/

		// Multi-dimensional arrays have "phantom" methods that Cecil can't
		// resolve, as they're provided by the runtime.  These should be
		// ignored.
		cli::array<int, 2>^ array = gcnew cli::array<int, 2>(1, 1);
		array[0, 0] = 42;
	};

			/// <param name="list">A <see cref="T:Mono.DocTest.Generic.MyList{System.Predicate{System.Int32}}" />.</param>
			/// <remarks><c>M:Mono.DocTest.UseLists.Process(System.Collections.Generic.List{System.Predicate{System.Int32}})</c>.</remarks>
	public: void Process(Generic1::List<Predicate<int>^>^ list)
	{
		if (list == nullptr)
			throw gcnew ArgumentNullException("list");
		Process<int>(list);
	};

			/// <param name="list">A <see cref="T:Mono.DocTest.Generic.MyList{System.Predicate{``0}}" />.</param>
			/// <typeparam name="T">Something Else</typeparam>
			/// <remarks><c>M:Mono.DocTest.UseLists.Process``1(System.Collections.Generic.List{System.Predicate{``0}})</c>.</remarks>
	public:
		generic<typename T>
			void Process(Generic1::List<Predicate<T>^>^ list)
			{
				if (list->Contains(nullptr))
					throw gcnew ArgumentException("predicate null");
			};

			/// <param name="helper">A <see cref="T:Mono.DocTest.Generic.MyList{``0}.Helper{``1,``2}" />.</param>
			/// <typeparam name="T"><c>T</c></typeparam>
			/// <typeparam name="U"><c>U</c></typeparam>
			/// <typeparam name="V"><c>V</c></typeparam>
			/// <remarks><c>M:Mono.DocTest.UseLists.UseHelper``3(Mono.DocTest.Generic.MyList{``0}.Helper{``1,``2})</c>.</remarks>
	public:
		generic<typename T, typename U, typename V>
			void UseHelper(Mono_DocTest_Generic::MyList<T>::Helper<U, V>^ helper) {};
	};
};
