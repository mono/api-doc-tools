namespace mdoc.Test.SampleClasses
{
    public class RefIndexer
    {
        public ref int this[int i] => throw null;
    }

    public class RefReadonlyIndexer
    {
        public ref readonly int this[int i] => throw null;
    }

    public class GenericRefIndexer<T>
    {
        public ref T this[int i] => throw null;
    }

    public class GenericRefReadonlyIndexer<T>
    {
        public ref readonly T this[int i] => throw null;
    }
}
