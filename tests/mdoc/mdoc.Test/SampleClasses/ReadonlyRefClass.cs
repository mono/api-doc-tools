using System.Runtime.InteropServices;

namespace mdoc.Test.SampleClasses
{
    public class ReadonlyRefClass
    {
        int i;
        public ref int Ref() => ref i;

        public ref readonly int ReadonlyRef() => ref i;

        public ref int RefProperty { get { return ref i; } }

        public ref readonly int this[int index] => throw null;

        public void RefInAndOutMethod(ref int a, in int b, out int c) => throw null;

        public void InAttributeMethod([In] ref int a, [In] in int b, [Out] out int c) => throw null;
    }

    public class GenericRefClass<T>
    {
        public ref T Ref() => throw null;

        public ref readonly T ReadonlyRef() => throw null;

        public ref T RefProperty => throw null;

        public ref readonly T this[int index] => throw null;

        public void RefInAndOutMethod(ref T a, in T b, out T c) => throw null;

        public void InAttributeMethod([In] ref T a, [In] in T b, [Out] out T c) => throw null;
    }
}
