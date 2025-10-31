using System;

namespace mdoc.Test.SampleClasses
{
    public class TupleNamesTestClass<T1, T2> : IComparable<ValueTuple<T1, T2>>
    {
        public (int a, int b) TuplePropertyType { get; }

        public (int a, int b, int c) TupleField;

        public (int a, int, int b) TupleMethod((int, int) t1, (int b, int c, int d) t2, ValueTuple<int, int> t3) => (t1.Item1, t2.b, t3.Item2);

        public ((int a, long b) c, int d) RecursiveTupleMethod((((int a, long) b, string c) d, (int e, (float f, float g) h) i, int j) t) => (t.d.b, t.j);

        public int CompareTo((T1, T2) other)
        {
            throw new NotImplementedException();
        }
    }
}