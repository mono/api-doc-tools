namespace mdoc.Test.SampleClasses
{
    public class SomeGenericClass<T>
    {
        public void SomeMethod<T>(T t)
        {
            
        }
        public void SomeMethod2(T t)
        {
            
        }
        public void SomeMethod3(int t)
        {
            
        }
        public void SomeMethod4(out string a, in int i, T t, object b = null)
        {
            a = "default";
        }
        ~SomeGenericClass()
        {

        }
    }
}