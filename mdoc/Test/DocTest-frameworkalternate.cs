namespace Monodoc.Test 
{
    public class MyClass
    {
        #if FXONE
        public void Meth(int a, string b, int c) {}
        #endif

        #if FXTWO
        public void Meth(int a, string d, int c) {}
        #endif
    }
}