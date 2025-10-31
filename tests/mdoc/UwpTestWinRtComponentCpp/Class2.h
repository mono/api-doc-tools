#pragma once
#include<Class1.h>

namespace Namespace2 {
	public ref class Class2 : public Windows::UI::Xaml::Application
	{
	private:
		Class2();

	private: ref class Class2Nested {};
	};

	public ref class Class3 sealed : public Windows::UI::Xaml::Application
	{
	private:
		Class3();

		//	public: double DoubleField;

	public: property long long LongProperty;
	public: property Platform::Array<Platform::Type ^ > ^ ArrayOfTypeProperty;

	protected: property Platform::Array<Platform::Type ^ > ^ ArrayOfTypePropertyProtected;

	//for public wnRT type -> *
	//	public: unsigned long long MethodWithReferenceParameter(Class2 ^ * refParam);
	//only for private
	private: unsigned long long MethodWithReferenceParameter(double & refParam);
	};

	public value class Class4 {
		//at least 1 public field
	public: Platform::String ^ StringField;

			//не может нон паблик дата мемберс
			//private: property Platform::String ^ StringField2;
	};

		generic<typename T>
		private interface class IFooNew {

		};
}

