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

        public GenericType<string?, int?, bool?> GenericReturnValueWithNullableParameters1()
        {
            return new GenericType<string?, int?, bool?>();
        }

        public GenericType<string?, int, bool> GenericReturnValueWithNullableParameters2()
        {
            return new GenericType<string?, int, bool>();
        }

        public GenericType<string, int, bool> GenericReturnValueWithNullableParameters3()
        {
            return new GenericType<string, int, bool>();
        }

        public GenericType<string?, string> GenericReturnValueWithNullableParameters4()
        {
            return new GenericType<string?, string>();
        }

        public GenericType<int?,int> GenericReturnValueWithNullableParameters5()
        {
            return new GenericType<int?,int>();
        }

        public GenericType<bool?, bool> GenericReturnValueWithNullableParameters6()
        {
            return new GenericType<bool?, bool>();
        }
    }
}
