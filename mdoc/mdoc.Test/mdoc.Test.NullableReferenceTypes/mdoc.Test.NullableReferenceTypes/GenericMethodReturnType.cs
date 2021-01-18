namespace mdoc.Test.NullableReferenceTypes
{
    public class GenericMethodReturnType
    {
        public T GenericType<T>()
        {
            return default;
        }

        public T GenericReferenceType<T>() where T : class
        {
            return default;
        }

        public T? GenericNullableReferenceType<T>() where T : class
        {
            return default;
        }

        public T GenericValueType<T>() where T : struct
        {
            return default;
        }

        public T? GenericNullableValueType<T>() where T : struct
        {
            return default;
        }
    }
}
