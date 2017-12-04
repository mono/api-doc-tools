using System;

namespace TestInterfaceImplementation
{
    public class Class1 : Interface1
    {
        public void Method1()
        {
        }

        Interface1 Interface1.Method2()
        {
            throw new NotImplementedException();
        }

        public int Property1 { get; }
        int Interface1.Property2 { get; set; }

        public event EventHandler Event1;
        event EventHandler<EventArgs> Interface1.Event2
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public int this[int index]
        {
            get { throw new NotImplementedException(); }
        }

        int Interface1.this[float index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
