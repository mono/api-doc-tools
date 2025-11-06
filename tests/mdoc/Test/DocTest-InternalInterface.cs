using System;

namespace MyNamespace {
	internal interface MyInternalInterface {
		bool Foo {get;set;}
		string FooSet {set;}
		void FooMeth ();
		void BarMeth ();
		event EventHandler<string> InternalEvent;
	}
	public interface MyPublicInterface {
		event EventHandler<string> PublicEvent;
	}

	public class MyClass : MyInternalInterface, MyPublicInterface {
		[System.ComponentModel.DefaultValue ('\0')]
		public string Bar {get;set;}
		public void BarMeth () {} // part of the interface, but publicly implemented

		string MyInternalInterface.FooSet {set {}}
		bool MyInternalInterface.Foo {get;set;}
		void MyInternalInterface.FooMeth () {}

		event EventHandler<string> MyPublicInterface.PublicEvent {add{}remove{}}
		event EventHandler<string> MyInternalInterface.InternalEvent {add{}remove{}}
		public event EventHandler<int> InstanceEvent {add{}remove{}}
	}

    public static class ArrayX10 {
        public static bool IsAligned<T> (this T[] vect, int index) where T : struct 
        {
            return false;
        }
    }
}
