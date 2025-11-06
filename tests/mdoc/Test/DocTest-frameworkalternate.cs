using System;

namespace Monodoc.Test 
{
    public class FirstAttribute : Attribute {}
    public class SecondAttribute : Attribute {}
    public class MyClass
    {
        #if FXONE
        [First]
        public void Meth(int a, string b, int c) {}
        #endif

        #if FXTWO
        [First, Second]
        public void Meth(int a, string d, int c) {}
        #endif
    }
}