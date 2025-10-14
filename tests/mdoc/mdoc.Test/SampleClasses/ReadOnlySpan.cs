
namespace mdoc.Test.SampleClasses
{
    public struct ReadOnlySpan<T>
    {
        public T this[int index] => default(T);
    }
}
