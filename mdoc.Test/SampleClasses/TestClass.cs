using System;
namespace mdoc.Test.SampleClasses
{
    public class TestClass
    {
        public TestClass () { }
        // private constructor
        TestClass (TestPrivateClass arg) { }

        // Unary Operators
		public static TestClass operator + (TestClass c1) { return new TestClass (); }
        public static TestClass operator - (TestClass c1) { return new TestClass (); }
        public static TestClass operator ! (TestClass c1) { return new TestClass (); }
        public static TestClass operator ~ (TestClass c1) { return new TestClass (); }
        public static TestClass operator ++ (TestClass c1) { return new TestClass (); }
        public static TestClass operator -- (TestClass c1) { return new TestClass (); }

        // Binary Operators
        public static TestClass operator + (TestClass c1, TestClass c2) { return new TestClass (); }
		public static TestClass operator - (TestClass c1, TestClass c2) {return new TestClass (); }
		public static TestClass operator / (TestClass c1, TestClass c2) { return new TestClass (); } 
        public static TestClass operator * (TestClass c1, TestClass c2) { return new TestClass (); }
		public static TestClass operator % (TestClass c1, TestClass c2) { return new TestClass (); }
        public static TestClass operator & (TestClass c1, TestClass c2) { return new TestClass (); }
        public static TestClass operator | (TestClass c1, TestClass c2) { return new TestClass (); }
        public static TestClass operator ^ (TestClass c1, TestClass c2) { return new TestClass (); }
        public static TestClass operator << (TestClass c1, int c2) { return new TestClass (); }
        public static TestClass operator >> (TestClass c1, int c2) { return new TestClass (); }

        // Comparison Operators
        public static bool operator true (TestClass c1) { return false; }
		public static bool operator false (TestClass c1) { return false; }
		public static bool operator == (TestClass c1, TestClass c2) { return true; }
		public static bool operator != (TestClass c1, TestClass c2) { return true; }
        public static bool operator < (TestClass c1, TestClass c2) { return true; }
        public static bool operator > (TestClass c1, TestClass c2) { return true; }
        public static bool operator <= (TestClass c1, TestClass c2) { return true; }
        public static bool operator >= (TestClass c1, TestClass c2) { return true; }
	}
}
