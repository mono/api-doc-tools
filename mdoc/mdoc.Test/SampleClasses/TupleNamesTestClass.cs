using System;

namespace mdoc.Test.SampleClasses
{
    public class TupleNamesTestClass<T1, T2> : IComparable<ValueTuple<T1, T2>>
    {
        public (int a, int b) TuplePropertyType { get; }

        public (int a, int b, int c) TupleField;

        public (int a, int, int b) TupleMethod((int, int) t1, (int b, int c, int d) t2, ValueTuple<int, int> t3) => (t1.Item1, t2.b, t3.Item2);

        public ((int a, long b) c, int d) RecursiveTupleMethod((((int a, long) b, string c) d, (int e, (float f, float g) h) i, int j) t) => (t.d.b, t.j);

        public static (T1, T2, T3, T4, T5, T6, T7, T8) EightTupleMethod<T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
            => (item1, item2, item3, item4, item5, item6, item7, item8);

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17) SeventeenTupleMethod<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, T17 item17)
    => (item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, item16, item17);

        public int CompareTo((T1, T2) other)
        {
            throw new NotImplementedException();
        }
    }
}