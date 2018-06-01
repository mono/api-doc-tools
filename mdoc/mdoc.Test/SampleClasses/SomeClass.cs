using System;
using Windows.Foundation.Metadata;

namespace mdoc.Test.SampleClasses
{
    public class SomeClass
    {
        public SomeClass(int i)
        {
            
        }

        public SomeClass(int i, int j)
        {
            
        }

        public int Field;

        public int Property { get; set; }

        public static int StaticProperty { get; set; }

        public TestClass Property2 { get; set; }
        public static TestClass StaticProperty2 { get; set; }

        public TestClass Property3 { get; }

        public TestClass Property4
        {
            set
            {
                
            }
        }

        public async void AsyncMethod()
        {
            
        }

        public static async void StaticAsyncMethod()
        {
            
        }

        public void SomeMethodWithParameters(SomeClass someClass, int i)
        {
            
        }

        public void SomeMethod()
        {
            
        }

        public void SomeMethodWebHostHiddenParameter([WebHostHidden] int parameter)
        {
            
        }

        [return: WebHostHidden()]
        public int SomeMethodWebHostHiddenReturn(int parameter)
        {
            throw new NotImplementedException();
        }

        public static void SomeStaticMethod()
        {
            
        }

        public int SomeMethod2()
        {
            return 0;
        }

        public static int SomeStaticMethod2()
        {
            return 0;
        }

        public bool SomeMethodWithReturnBool()
        {
            return true;
        }

        public void op_NotOperator()
        {
        }

        public void get_Method()
        {
        }

        public event EventHandler<object> AppMemoryUsageIncreased;
        public static event EventHandler<object> StaticEvent;
        private static event EventHandler<object> PrivateEvent;
    }
}