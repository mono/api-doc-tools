using System.Collections.Generic;

namespace mdoc.Test.NullableReferenceTypes
{
    public class GenericFieldType<T>
    {
        public T GenericType;
    }

    public class GenericFieldTypeOfValueType<T> where T : struct
    {
        public T GenericType;
        public T? NullableGenericType;
        public ICollection<T> InterfaceOfGenericType;
        public ICollection<T?> InterfaceOfNullableGenericType;
        public ICollection<T>? NullableInterfaceOfGenericType;
        public ICollection<T?>? NullableInterfaceOfNullableGenericType;
        public Dictionary<Dictionary<T, string>, string> DictionaryOfDictionary;
        public Dictionary<Dictionary<T?, string>, string> DictionaryOfDictionaryOfNullableGenericTypeKey;
        public Dictionary<Dictionary<T?, string>, string>? NullableDictionaryOfDictionaryOfNullableGenericTypeKey;
    }

    public class GenericFieldTypeOfReferenceType<T> where T : class
    {
        public T GenericType;
        public T? NullableGenericType;
        public ICollection<T> InterfaceOfGenericType;
        public ICollection<T?> InterfaceOfNullableGenericType;
        public ICollection<T>? NullableInterfaceOfGenericType;
        public ICollection<T?>? NullableInterfaceOfNullableGenericType;
        public Dictionary<Dictionary<T, string>, string> DictionaryOfDictionary;
        public Dictionary<Dictionary<T?, string>, string> DictionaryOfDictionaryOfNullableGenericTypeKey;
        public Dictionary<Dictionary<T?, string>, string>? NullableDictionaryOfDictionaryOfNullableGenericTypeKey;
    }
}
