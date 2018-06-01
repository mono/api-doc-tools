#pragma once
#pragma once

namespace Generic1 = System::Collections::Generic;


class TestClass
{
public:
	TestClass();
	~TestClass();
};

public enum class Color {
	/// <summary>Insert Red summary here</summary>
	/// <remarks><c>F:Mono.DocTest.Color.Red</c>.</remarks>
	Red,
	/// <summary>Insert Blue summary here</summary>
	/// <remarks><c>F:Mono.DocTest.Color.Blue</c>.</remarks>
	Blue,
	/// <summary>Insert Green summary here</summary>
	/// <remarks><c>F:Mono.DocTest.Color.Green</c>.</remarks>
	Green,

	AnotherGreen = Green,
};

/// <typeparam name="T">T generic param</typeparam>
/// <remarks><c>T:Mono.DocTest.IFoo`1</c>.</remarks>
generic<typename T>
	public interface class IFoo {
		/// <typeparam name="U">U generic param</typeparam>
		/// <remarks><c>T:Mono.DocTest.IFoo`1.Method``1(`0,``0)</c>.</remarks>
		generic<typename U>
			T Method(T t, U u);
	};

	namespace Mono_DocTest_Generic {
		using namespace System;
		using namespace System::Collections;

		generic<typename T>
			where T: value class
				public interface class IFooNew {
				/// <typeparam name="U">U generic param</typeparam>
				/// <remarks><c>T:Mono.DocTest.IFoo`1.Method``1(`0,``0)</c>.</remarks>
				generic<typename U>
					T Method(T t, U u);
			};

		
		/// <summary>extension methods!</summary>
		/// <remarks><c>T:Mono.DocTest.Generic.Extensions</c></remarks>
		//todo: do need attribute on class??
		[System::Runtime::CompilerServices::Extension]
		public ref class Extensions abstract sealed {
			/// <summary><c>System.Object</c> extension method</summary>
			/// <remarks><c>M:Mono.DocTest.Generic.Extensions.ToEnumerable``1</c></remarks>
		public:
			generic <typename T>
				[System::Runtime::CompilerServices::Extension]
				static Generic1::IEnumerable<T> ^ ToEnumerable(T self)
				{
					//todo: no yield
					//yield return self;
					return gcnew Generic1::List<T>();
				};

				/// <summary><see cref="T:System.Collections.Generic.IEnumerable`1" /> extension method</summary>
				/// <remarks><c>M:Mono.DocTest.Generic.Extensions.ForEach``1</c></remarks>
		public:
			generic <typename T>
				[System::Runtime::CompilerServices::Extension]
				static void ForEach(Generic1::IEnumerable<T> ^ self, Action<T> ^ a)
				{
				};

				/// <summary><see cref="T:Mono.DocTest.Generic.IFoo`1" /> extension method</summary>
				/// <remarks><c>M:Mono.DocTest.Generic.Extensions.Bar``1</c></remarks>

		public:
			generic <typename T>
				[System::Runtime::CompilerServices::Extension]
				static void Bar(IFoo<T>^ self, String ^ s)
				{
				};

				/// <summary>
				///   <see cref="T:System.Collections.Generic.IEnumerable{System.Int32}" /> 
				///   extension method.
				/// </summary>
				/// <remarks><c>M:Mono.DocTest.Generic.Extensions.ToDouble</c></remarks>
		public:
			[System::Runtime::CompilerServices::Extension]
			static Generic1::IEnumerable<double>^ ToDouble(Generic1::IEnumerable<int>^ list)
			{
				return nullptr;
			};

			/// <summary>
			///   <see cref="T:Mono.DocTest.Generic.IFoo`1" /> extension method.
			/// </summary>
			/// <remarks><c>M:Mono.DocTest.Generic.Extensions.ToDouble</c></remarks>
			public:
			generic <typename T>
				where T : IFoo<T>
					[System::Runtime::CompilerServices::Extension]
				static double ToDouble(T val)
				{
					// the target type is T:...IFoo<T>, NOT T:System.Object.
					return 0.0;
				};
		};

		/// <typeparam name="U">Insert <c>text</c> here.</typeparam>
		/// <remarks><c>T:Mono.DocTest.Generic.GenericBase`1</c>.</remarks>
        generic <typename U>
            public ref class GenericBase {
                /// <param name="genericParameter">Something</param>
                /// <typeparam name="S">Insert more <c>text</c> here.</typeparam>
                /// <remarks><c>M:Mono.DocTest.GenericBase`1.BaseMethod``1(``0)</c>.</remarks>
                /// <returns>The default value.</returns>

				//todo: done for default keyword
			private: U member;
					 //todo: done for default keyword
			public: GenericBase() : member()
			{
			};
					//todo: done for default keyword
					GenericBase(GenericBase^ c)
					{
						member = c->member;
					};

			public:
				generic <typename S>
					U BaseMethod(/*[Doc("S")]*/S genericParameter) {
						return member;
					};

					U BaseMethod2(GenericBase<U> genericParameter) {
						return member;
					};

					/// <remarks><c>F:Mono.DocTest.GenericBase`1.StaticField1</c></remarks>
			public:
				static initonly GenericBase<U> ^ StaticField1 = gcnew GenericBase<U>();

				/// <remarks><c>F:Mono.DocTest.GenericBase`1.ConstField1</c></remarks>
			public: const int ConstInt = 1;

			public: const long ConstLong = 2;

			public: const Decimal  ConstDecimal;

			public: const short ConstShort = 4;

			public: const UInt16 ConstUint16 = 2 ;

			public: const UInt32 ConstUint32 = 3;

			public: const UInt64 ConstUint64 = 4;

			public: const float ConstFloat = 2.4;

			public: const bool ConstBool = true;

			public: const char ConstChar = 't';

			public: const Object^ ConstObject;

			public: const String^ ConstString;



			
					/// <param name="list">Insert description here</param>
					/// <remarks><c>M:Mono.DocTest.GenericBase`1.op_Explicit(Mono.DocTest.GenericBase{`0})~`0</c></remarks>
					/// <returns>The default value for <typeparamref name="U"/>.</returns>
					//public: static explicit operator U(GenericBase<U> list) { /*return 0;*/ return nullptr; };

					/// <remarks>T:Mono.DocTest.Generic.GenericBase`1.FooEventArgs</remarks>
			public: ref class FooEventArgs : EventArgs {
			};

					/// <remarks>E:Mono.DocTest.Generic.GenericBase`1.MyEvent</remarks>
			public: event EventHandler<FooEventArgs ^ > ^ MyEvent;

					/// <remarks>E:Mono.DocTest.Generic.GenericBase`1.ItemChanged</remarks>
					//todo: uncomment
					//public: event Action<Mono_DocTest_Generic::MyList<U>^, Mono_DocTest_Generic::MyList<U>::Helper<U, U>^> ItemChanged;

					/// <remarks>T:Mono.DocTest.Generic.GenericBase`1.NestedCollection</remarks>
			public: ref class NestedCollection {
				//todo: no internal
				/// <remarks>T:Mono.DocTest.Generic.GenericBase`1.NestedCollection.Enumerator</remarks>
			protected: value struct Enumerator {
			};
			};
			};

			/// <typeparam name="T">I'm Dying Here!</typeparam>
			/// <remarks><c>T:Mono.DocTest.Generic.MyList`1</c>.</remarks>
			//todo: on generic par-r [Mono.DocTest.Doc("Type Parameter!")] 
            generic <typename T>
                public ref class MyList : GenericBase<T>, Generic1::IEnumerable<cli::array <int> ^ >
				{
                    /// <typeparam name="U">Seriously!</typeparam>
                    /// <typeparam name="V">Too <c>many</c> docs!</typeparam>
                    /// <remarks><c>T:Mono.DocTest.MyList`1.Helper`2</c>.</remarks>

				public:
					generic<typename U, typename V>
						ref class Helper {
							/// <param name="a">Ako</param>
							/// <param name="b">bko</param>
							/// <param name="c">cko</param>
							/// <remarks><c>M:Mono.DocTest.MyList`1.Helper`2.UseT(`0,`1,`2)</c>.</remarks>
						public: void UseT(T a, U b, V c) { };
						};

						/// <param name="t">tko</param>
						/// <remarks><c>M:Mono.DocTest.MyList`1.Test(`0)</c>.</remarks>
				public: void Test(T t) { auto a = gcnew MyList::Helper<int, String^>(); };

						/// <param name="t">Class generic type</param>
						/// <param name="u">Method generic type</param>
						/// <typeparam name="U">Method generic parameter</typeparam>
						/// <remarks><c>M:Mono.DocTest.MyList`1.Method``1(`0,``0)</c>.</remarks>

				public:
					generic<typename U>
						void Method(T t, U u) {};

						// mcs "crashes" (CS1569) on this method; exclude it for now.
						// <remarks><c>M:Mono.DocTest.MyList`1.RefMethod``1(`0@,``0@)</c>.</remarks>
				public:
					generic<typename U>
						void RefMethod(T% t, U% u) {};

						/// <param name="helper">A <see cref="T:Mono.DocTest.Generic.MyList`1.Helper`2" />.</param>
						/// <typeparam name="U">Argh!</typeparam>
						/// <typeparam name="V">Foo Argh!</typeparam>
						/// <remarks><c>M:Mono.DocTest.Generic.MyList`1.UseHelper``2(Mono.DocTest.Generic.MyList{``0}.Helper{``1,``2})</c>.</remarks>
				public:
					generic<typename U, typename V>
						void UseHelper(Helper helper) {};

						/// <remarks><c>M:Mono.DocTest.Generic.MyList`1.GetHelper``2</c>.</remarks>
						/// <returns><see langword="null" />.</returns>
				public:
					generic<typename U, typename V>
						Helper^ GetHelper() { return nullptr; };

						/// <remarks><c>M:Mono.DocTest.MyList`1.System#Collections#GetEnumerator</c>.</remarks>						
				public: virtual	IEnumerator^ GetEnumerator1() = IEnumerable::GetEnumerator{
					return nullptr;
				};


						/// <remarks><c>M:Mono.DocTest.MyList`1.GetEnumerator</c>.</remarks>
				public: virtual Generic1::IEnumerator<cli::array<int>^>^ GetEnumerator() = Generic1::IEnumerable<cli::array<int>^>::GetEnumerator{
					return nullptr;
				};
				};

				/// <typeparam name="T">T generic param</typeparam>
				/// <remarks><c>T:Mono.DocTest.IFoo`1</c>.</remarks>
				generic <typename T>
					public interface class IFoo {
						/// <typeparam name="U">U generic param</typeparam>
						/// <remarks><c>T:Mono.DocTest.IFoo`1.Method``1(`0,``0)</c>.</remarks>
						generic<typename U>
							T Method(T t, U u);
					};

					generic <typename T>
						where T: gcnew()
						public ref class GenericConstraintClass {				
						};



					/// <typeparam name="A">Ako generic param</typeparam>
					/// <typeparam name="B">Bko generic param</typeparam>
					/// <remarks><c>T:Mono.DocTest.MyList`2</c>.</remarks>								
					generic <typename A, typename B>
						//where A : class, IList<B>, gcnew()
						//where B : class, A
						public ref class MyList1 : Generic1::ICollection<A>,
							Generic1::IEnumerable<A>, Generic1::IEnumerator<A>,
							IFoo<A>
							, GenericBase<Generic1::Dictionary<A,B>^ >/*<>*/
						{

							~MyList1() {};
							// IEnumerator

							// shown?
							//todo: uncomment
							//property Object^ IEnumerator::Current { Object^ get() { return nullptr; } }

							/// <remarks><c>M:Mono.DocTest.MyList`2.MoveNext</c>.</remarks>
							/// <returns><see cref="T:System.Boolean" /></returns>
						public:
							virtual bool MoveNext() { return false; };

							/// <remarks><c>M:Mono.DocTest.MyList`2.Reset</c>.</remarks>
						public:
							virtual void Reset() {};

							virtual property Object^ Current3 {
								Object^ get() = IEnumerator::Current::get{ return nullptr; }
							};

							// IEnumerator<T>
							/// <remarks><c>P:Mono.DocTest.MyList`2.Current</c>.</remarks>
							/// <value>The current value.</value>
						public:
							property A Current1 {
								A get() { return Current2; /*default(A);*/ }
							};
							/// <remarks><c>P:Mono.DocTest.MyList`2.Current</c>.</remarks>
							/// <value>The current value.</value>							
							virtual property A Current2 {
								A get() sealed = Generic1::IEnumerator<A>::Current::get{ return Current1; /*default(A);*/ };// { return default(A); }
							};

							/// <remarks><c>M:Mono.DocTest.MyList`2.System#Collections#GetEnumerator</c>.</remarks>
						public: virtual	System::Collections::IEnumerator^ GetEnumerator1() = System::Collections::IEnumerable::GetEnumerator{
							return nullptr;
						};


								// IEnumerable<T>
								/// <remarks><c>M:Mono.DocTest.MyList`2.System#Collections#Generic#IEnumerable{A}#GetEnumerator</c>.</remarks>
								/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator{`0}" />.</returns>
								virtual	Generic1::IEnumerator<A>^ GetEnumerator() = Generic1::IEnumerable<A>::GetEnumerator{ return nullptr; };


						public: Generic1::List<A>::Enumerator^ GetEnumerator3() { return gcnew Generic1::List<A>::Enumerator(); };

								// ICollection<T>
								/// <remarks><c>P:Mono.DocTest.MyList`2.Count</c>.</remarks>
								/// <value>A <see cref="T:System.Int32" />.</value>
						public:
							virtual property int Count {
								int get() { return 0; }
							};

							/// <remarks><c>P:Mono.DocTest.MyList`2.System#Collections#Generic#ICollection{A}#IsReadOnly</c>.</remarks>
							/// <value>A <see cref="T:System.Boolean" />.</value>
						public:
							virtual property bool IsReadOnly {
								bool get() { return false; }
							};
							/// <param name="item">The item to add.</param>
							/// <remarks><c>M:Mono.DocTest.MyList`2.System#Collections#Generic#ICollection{A}#Add(`0)</c>.</remarks>
							virtual void Add(A item) = Generic1::ICollection<A>::Add{};
							/// <remarks><c>M:Mono.DocTest.MyList`2.System#Collections#Generic#ICollection{A}#Clear</c>.</remarks>
							virtual void Clear() = Generic1::ICollection<A>::Clear{};
							/// <param name="item">The item to check for</param>
							/// <remarks><c>M:Mono.DocTest.MyList`2.System#Collections#Generic#ICollection{A}.Contains(`0)</c>.</remarks>
							/// <returns>A <see cref="T:System.Boolean" /> instance (<see langword="false" />).</returns>
							virtual bool Contains(A item) = Generic1::ICollection<A>::Contains{ return false; };
							/// <param name="array">Where to copy elements to</param>
							/// <param name="arrayIndex">Where to start copyingto</param>
							/// <remarks><c>M:Mono.DocTest.MyList`2.CopyTo(`0[],System.Int32)</c>.</remarks>
						public: virtual void CopyTo(cli::array<A>^ arrayPar, int arrayIndex) = Generic1::ICollection<A>::CopyTo{};
								/// <param name="item">the item to remove</param>
								/// <remarks><c>M:Mono.DocTest.MyList`2.System#Collections#Generic#ICollection{A}#Remove(`0)</c>.</remarks>
								/// <returns>Whether the item was removed.</returns>
								virtual bool Remove(A item) = Generic1::ICollection<A>::Remove{ return false; };

								/// <remarks>M:Mono.DocTest.Generic.MyList`2.Foo</remarks>						
						public:
							generic<typename AA, typename BB>
								where AA : Generic1::IEnumerable<A>
									where BB : Generic1::IEnumerable<B>
										Generic1::KeyValuePair<AA, BB>^ Foo()
									{
										return gcnew Generic1::KeyValuePair<AA, BB>();
									};

									// IFoo members
									/// <typeparam name="U">U generic param on MyList`2</typeparam>
									/// <remarks><c>M:Mono.DocTest.Generic.MyList`2.Mono#DocTest#Generic#IFoo{A}#Method``1(`0,``0)</c>.</remarks>
									generic<typename U>
										virtual A Method(A a, U u) = IFoo<A>::Method
										{
											return Current2; /*default(A);*/
										};

						};




	}
