namespace mdoc.Test.NullableReferenceTypes
{
    public class GenericPropertyReturnType<T, TClass, TStruct> where TClass : class where TStruct : struct
    {
        public T GenericType { get; }

        public TClass GenericReferenceType { get; }

        public TClass? GenericNullableReferenceType { get; }

        public TStruct GenericValueType { get; }

        public TStruct? GenericNullableValueType { get; }
    }
}
