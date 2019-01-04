namespace mdoc.Test.SampleClasses
{
    public class ReadonlyRefClass
    {
        int i;

        public ref int Ref() => ref i;
        public ref readonly int ReadonlyRef() => ref i;
    }
}
