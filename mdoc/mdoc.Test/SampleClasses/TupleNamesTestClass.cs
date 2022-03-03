namespace mdoc.Test.SampleClasses
{
    public class TupleNamesTestClass
    {
        public (int a, int b) TuplePropertyType { get; }

        public (int a, int b, int c) TupleField;

        public (int a, int) TupleMethod((int, int) t1, (int b, int c, int d) t2) => (t1.Item1, t2.b);

        public ((int a, int b) c, int d) RecursiveTupleMethod((((int a, int) b, int c) d, (int e, (int f, int g) h) i, int j) t) => (t.d.b, t.j);
    }
}