﻿namespace mdoc.Test.SampleClasses
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
        ~SomeGenericClass()
        {

        }
        public void ZdoWithNullParams(out string a, object b = null, TestClass c = null) { a = "default"; }
    }
}