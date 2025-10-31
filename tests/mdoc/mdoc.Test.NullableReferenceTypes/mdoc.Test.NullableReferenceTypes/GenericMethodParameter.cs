using System;

namespace mdoc.Test.NullableReferenceTypes
{
    public class GenericMethodParameter
    {
        public void GenericType<T>(T t)
        {
        }

        public void GenericReferenceType<T>(T t) where T : class
        {
        }

        public void GenericNullableReferenceType<T>(T? t) where T : class
        {
        }

        public void ActionOfGenericNullableReferenceType<T>(Action<T?> t) where T : class
        {
        }

        public void NullableActionOfGenericNullableReferenceType<T>(Action<T?>? t) where T : class
        {
        }

        public void FuncGenericNullableReferenceType<T>(Func<T?> t) where T : class
        {
        }

        public void NullableFuncGenericNullableReferenceType<T>(Func<T?>? t) where T : class
        {
        }

        public void GenericNonNullableAndNullableReferenceType<T1, T2>(T1 t1, T2? t2) where T2 : class
        {
        }

        public void GenericValueType<T>(T t) where T : struct
        {
        }

        public void GenericNullableValueType<T>(T? t) where T : struct
        {
        }

        public void ActionOfGenericNullableValueType<T>(Action<T?> action) where T : struct
        {
        }

        public void NullableActionOfGenericNullableValueType<T>(Action<T?>? action) where T : struct
        {
        }

        public void FuncGenericNullableValueType<T>(Func<T?> func) where T : struct
        {
        }

        public void NullableFuncGenericNullableValueType<T>(Func<T?>? func) where T : struct
        {
        }

        public void GenericNonNullableAndNullableValueType<T1, T2>(T1 t1, T2? t2) where T2 : struct
        {
        }

        public void GenericTypeWithNullableParameters1(GenericType<string?, int?, bool?> p)
        {
        }

        public void GenericTypeWithNullableParameters2(GenericType<string?, int, bool> p)
        {
        }

        public void GenericTypeWithNullableParameters3(GenericType<string, int, bool> p)
        {
        }

        public void GenericTypeWithNullableParameters4(GenericType<string?, string> p)
        {
        }

        public void GenericTypeWithNullableParameters5(GenericType<int?, int> p)
        {
        }

        public void GenericTypeWithNullableParameters6(GenericType<bool?, bool> p)
        {
        }
    }
}
