using System;
using System.Collections.Generic;

namespace mdoc.Test.SampleClasses
{
    public class NativeIntClass
    {
        public (nint, nuint) Method1(nint a, nuint b, IntPtr c, UIntPtr d)
        {
            return (a + c, b + d);
        }

        public (nint, nuint) Method2(List<nint> a, Dictionary<int, nuint> b)
        {
            return (a[0], b[0]);
        }

        public (nint, nuint) Method3((nint, nuint) a, (nuint, IntPtr) b, (UIntPtr, string) c)
        {
            return (a.Item1 + b.Item2, b.Item1 + c.Item1);
        }

        public (((nint a, IntPtr) b, UIntPtr c) d, (nint e, (nuint f, IntPtr g) h) i) Method4() => throw null;
    }

    public class GenericNativeIntClass<nint>
    {

    }
}