#pragma once
#include <collection.h>
#include <ppl.h>
#include <amp.h>
#include <amp_math.h>
using namespace Windows::Foundation::Collections;

namespace UwpTestWinRtComponentCpp
{
	//short, long
	public delegate void PrimeFoundHandler(int result);
	public delegate void PrimeFoundHandlerWithSpecificType(/*Windows::Foundation::Collections::*/IMap<double, float> ^ result);

    public ref class Class1 sealed
    {
    public:
        Class1();
	public:
		// Synchronous method.
		Windows::Foundation::Collections::IVector<double> ^ ComputeResult(double input);

		// Asynchronous methods
		Windows::Foundation::IAsyncOperationWithProgress<Windows::Foundation::Collections::IVector<int>^, double>^
			GetPrimesOrdered(int first, int last);
		Windows::Foundation::IAsyncActionWithProgress<double>^ GetPrimesUnordered(int first, int last);

		// Event whose type is a delegate "class"
		event PrimeFoundHandler^ primeFoundEvent;

	private:
		bool is_prime(int n);
		Windows::UI::Core::CoreDispatcher ^ m_dispatcher;
    };
		
	public delegate void SomethingHappenedEventHandler(Class1 ^ sender, Platform::String ^ s);

	public ref class CustomAttribute1 sealed : Platform::Metadata::Attribute {
		public: bool Field1;
		public: Windows::Foundation::HResult Field2;
		//public: Platform::CallbackContext Field3 = CallbackContext.Any;
			
		private: delegate void SomethingHappenedEventHandler();	
	};

	public enum class Color1 {
		Red, Blue
	};
}

namespace Namespace222 {


	using namespace std::chrono;

	using namespace Windows::ApplicationModel::Core;
	using namespace Windows::Foundation;
	using namespace Windows::UI::Core;
	using namespace Windows::UI::Composition;
	using namespace Windows::Media::Core;
	using namespace Windows::Media::Playback;
	using namespace Windows::System;
	using namespace Windows::Storage;
	using namespace Windows::Storage::Pickers;


	public ref class App sealed: IFrameworkView
	{
	 ref class NestedClass1 {};
	
	public: virtual void Initialize(CoreApplicationView ^ applicationView)
	{
	};

			void virtual Load(Platform::String ^ entryPoint)
			{
			};

			void virtual Uninitialize()
			{
			};

			void virtual Run()
			{

			};

			void virtual SetWindow(CoreWindow ^ window) {};	
			void SetWindow1(CoreWindow ^ window) {};			

	public: property CoreWindow ^ m_activated;
	public: property CompositionTarget ^ m_target;
	};
}