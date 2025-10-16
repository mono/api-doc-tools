using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mdoc.Test.NullableReferenceTypes
{
    public class CommonType
    {
        public dynamic DynamicType()
        {
            return default;
        }

        public dynamic? NullableDynamicType()
        {
            return default;
        }

        public DayOfWeek Enumeration()
        {
            return 0;
        }

        public DayOfWeek? NullableEnumeration()
        {
            return 0;
        }

        public int ValueType()
        {
            return 0;
        }

        public int? NullableValueType()
        {
            return default;
        }

        public int[] ArrayOfValueType()
        {
            return default;
        }

        public int?[] ArrayOfValueTypeNullable()
        {
            return default;
        }

        public int[]? NullableArrayOfValueType()
        {
            return default;
        }

        public int?[]? NullableArrayOfNullableValueType()
        {
            return default;
        }

        public int[][] DimensionalArrayOfValueType()
        {
            return default;
        }

        public int?[][] DimensionalArrayOfNullableValueType()
        {
            return default;
        }

        public int?[]?[] DimensionalArrayOfNullableValueTypeOfNullableRow()
        {
            return default;
        }

        public int?[]?[]? NullableDimensionalArrayOfNullableValueTypeOfNullableRow()
        {
            return default;
        }

        public int?[][]? NullableDimensionalArrayOfNullableValueType()
        {
            return default;
        }

        public int[][]? NullableDimensionalArrayOfValueType()
        {
            return default;
        }

        public int[][]?[][]? NullableFourDimensionalArrayOfValueTypeOfMiddleNullableArray()
        {
            return default;
        }

        public int?[][]?[][] FourDimensionalArrayOfNullableValueTypeOfMiddleNullableArray()
        {
            return default;
        }

        public Tuple<int, int> TupleOfValueType()
        {
            return default;
        }

        public Tuple<int, int>? NullableTupleOfValueType()
        {
            return default;
        }

        public Tuple<int?, int?> TupleOfNullableValueType()
        {
            return default;
        }

        public Tuple<int?, int?>? NullableTupleOfNullableValueType()
        {
            return default;
        }

        public ValueTuple<int, int> ValueTupleOfValueType()
        {
            return default;
        }

        public ValueTuple<int, int>? NullableValueTupleOfValueType()
        {
            return default;
        }
        public ValueTuple<int?, int?> ValueTupleOfNullableValueType()
        {
            return default;
        }
        public ValueTuple<int?, int?>? NullableValueTupleOfNullableValueType()
        {
            return default;
        }

        public ICollection<int> InterfaceOfValueType()
        {
            return default;
        }

        public ICollection<int>? NullableInterfaceOfValueType()
        {
            return default;
        }

        public ICollection<int?>? NullableInterfaceOfNullableValueType()
        {
            return default;
        }

        public Action<int> ActionOfValueType()
        {
            return default;
        }

        public Action<int?> ActionOfNullableValueType()
        {
            return default;
        }

        public Action<int>? NullableActionOfValueType()
        {
            return default;
        }

        public Action<int?>? NullableActionOfNullableValueType()
        {
            return default;
        }

        public Dictionary<int, int> DictionaryOfValueType()
        {
            return default;
        }

        public Dictionary<int, int>? NullableDictionaryOfValueType()
        {
            return default;
        }

        public Dictionary<int?, int?> DictionaryOfNullableValueType()
        {
            return default;
        }

        public Dictionary<int?, int?>? NullableDictionaryOfNullableValueType()
        {
            return default;
        }

        public Dictionary<int, int?> DictionaryOfNullableValueTypeValue()
        {
            return default;
        }

        public Dictionary<int, int?>? NullableDictionaryOfNullableValueTypeValue()
        {
            return default;
        }

        public Dictionary<int?, int>? NullableDictionaryOfNullableValueTypeKey()
        {
            return default;
        }

        public Dictionary<int, Dictionary<int, int>> DictionaryOfValueTypeKeyAndDictionaryOfValueTypeValue()
        {
            return default;
        }

        public Dictionary<int, Dictionary<int, int>>? NullableDictionaryOfValueTypeKeyAndDictionaryOfValueTypeValue()
        {
            return default;
        }

        public Dictionary<int?, Dictionary<int, int>?>? NullableDictionaryOfNullableValueTypeKeyAndNullableDictionaryOfValueTypeValue()
        {
            return default;
        }
        public Dictionary<int?, Dictionary<int?, int?>?>? NullableDictionaryOfNullableValueTypeKeyAndNullableDictionaryOfNullableValueTypeValue()
        {
            return default;
        }

        public Dictionary<int, Tuple<int, int>> DictionaryOfValueTypeKeyAndTupleOfValueTypeValue()
        {
            return default;
        }

        public Dictionary<int, Tuple<int, int>>? NullableDictionaryOfValueTypeKeyAndTupleOfValueTypeValue()
        {
            return default;
        }

        public Dictionary<int?, Tuple<int, int>?>? NullableDictionaryOfNullableValueTypeKeyAndNullableTupleOfValueTypeValue()
        {
            return default;
        }
        public Dictionary<int?, Tuple<int?, int?>?>? NullableDictionaryOfNullableValueTypeKeyAndNullableTupleOfNullableValueTypeValue()
        {
            return default;
        }

        public Dictionary<Dictionary<int, int>, Dictionary<int, int>> DictionaryOfDictionaryOfValueType()
        {
            return default;
        }

        public Dictionary<Dictionary<int, int>?, Dictionary<int, int>?>? NullableDictionaryOfNullableDictionaryOfValueType()
        {
            return default;
        }

        public Dictionary<Dictionary<int?, int?>?, Dictionary<int?, int?>?>? NullableDictionaryOfNullableDictionaryOfNullableValueType()
        {
            return default;
        }

        public string ReferenceType()
        {
            return default;
        }

        public string? NullableReferenceType()
        {
            return default;
        }

        public string[] ArrayOfReferenceType()
        {
            return default;
        }

        public string?[] ArrayOfNullableReferenceType()
        {
            return default;
        }

        public string[]? NullableArrayOfReferenceType()
        {
            return default;
        }

        public string?[]? NullableArrayOfNullableReferenceType()
        {
            return default;
        }

        public string[][] DimensionalArrayOfReferenceType()
        {
            return default;
        }

        public string?[][] DimensionalArrayOfNullableReferenceType()
        {
            return default;
        }

        public string?[]?[] DimensionalArrayOfNullableReferenceTypeOfNullableRow()
        {
            return default;
        }

        public string?[]?[]? NullableDimensionalArrayOfNullableReferenceTypeOfNullableRow()
        {
            return default;
        }

        public string?[][]? NullableDimensionalArrayOfNullableReferenceType()
        {
            return default;
        }

        public string[][]? NullableDimensionalArrayOfReferenceType()
        {
            return default;
        }

        public string[][]?[][]? NullableFourDimensionalArrayOfReferenceTypeOfMiddleNullableArray()
        {
            return default;
        }

        public string?[][]?[][] FourDimensionalArrayOfNullableReferenceTypeOfMiddleNullableArray()
        {
            return default;
        }

        public Tuple<string, string> TupleOfReferenceType()
        {
            return default;
        }

        public Tuple<string, string>? NullableTupleOfReferenceType()
        {
            return default;
        }
        public Tuple<string?, string?> TupleOfNullableReferenceType()
        {
            return default;
        }

        public Tuple<string?, string?>? NullableTupleOfNullableReferenceType()
        {
            return default;
        }

        public ValueTuple<string, string> ValueTupleOfReferenceType()
        {
            return default;
        }

        public ValueTuple<string, string>? NullableValueTupleOfReferenceType()
        {
            return default;
        }

        public ValueTuple<string?, string?> ValueTupleOfNullableReferenceType()
        {
            return default;
        }

        public ValueTuple<string?, string?>? NullableValueTupleOfNullableReferenceType()
        {
            return default;
        }

        public ICollection<string> InterfaceOfReferenceType()
        {
            return default;
        }

        public ICollection<string>? NullableInterfaceOfReferenceType()
        {
            return default;
        }

        public ICollection<string?>? NullableInterfaceOfNullableReferenceType()
        {
            return default;
        }

        public ICollection<dynamic> InterfaceOfDynamicType()
        {
            return default;
        }

        public ICollection<dynamic>? NullableInterfaceOfDynamicType()
        {
            return default;
        }

        public ICollection<dynamic?>? NullableInterfaceOfNullableDynamicType()
        {
            return default;
        }

        public Action<string> ActionOfReferenceType()
        {
            return default;
        }

        public Action<string?> ActionOfNullableReferenceType()
        {
            return default;
        }

        public Action<string>? NullableActionOfReferenceType()
        {
            return default;
        }

        public Action<string?>? NullableActionOfNullableReferenceType()
        {
            return default;
        }

        public Dictionary<string, string> DictionaryOfReferenceType()
        {
            return default;
        }

        public Dictionary<string, string>? NullableDictionaryOfReferenceType()
        {
            return default;
        }

        public Dictionary<string?, string?> DictionaryOfNullableReferenceType()
        {
            return default;
        }

        public Dictionary<string?, string?>? NullableDictionaryOfNullableReferenceType()
        {
            return default;
        }

        public Dictionary<string, string?> DictionaryOfNullableReferenceTypeValue()
        {
            return default;
        }

        public Dictionary<string, string?>? NullableDictionaryOfNullableReferenceTypeValue()
        {
            return default;
        }

        public Dictionary<string?, string>? NullableDictionaryOfNullableReferenceTypeKey()
        {
            return default;
        }

        public Dictionary<Dictionary<string, string>, Dictionary<string, string>> DictionaryOfDictionaryOfReferenceType()
        {
            return default;
        }

        public Dictionary<Dictionary<string, string>?, Dictionary<string, string>?>? NullableDictionaryOfNullableDictionaryOfReferenceType()
        {
            return default;
        }

        public Dictionary<Dictionary<string?, string?>?, Dictionary<string?, string?>?>? NullableDictionaryOfNullableDictionaryOfNullableReferenceType()
        {
            return default;
        }

        public Dictionary<string, Dictionary<string, string>> DictionaryOfReferenceTypeKeyAndDictionaryOfReferenceTypeValue()
        {
            return default;
        }

        public Dictionary<string, Dictionary<string, string>>? NullableDictionaryOfReferenceTypeKeyAndDictionaryOfReferenceTypeValue()
        {
            return default;
        }

        public Dictionary<string?, Dictionary<string, string>?>? NullableDictionaryOfNullableReferenceTypeKeyAndNullableDictionaryOfReferenceTypeValue()
        {
            return default;
        }

        public Dictionary<string?, Dictionary<string?, string?>?>? NullableDictionaryOfNullableReferenceTypeKeyAndNullableDictionaryOfNullableReferenceTypeValue()
        {
            return default;
        }

        public Dictionary<string, Tuple<string, string>> DictionaryOfReferenceTypeKeyAndTupleOfReferenceTypeValue()
        {
            return default;
        }

        public Dictionary<string, Tuple<string, string>>? NullableDictionaryOfReferenceTypeKeyAndTupleOfReferenceTypeValue()
        {
            return default;
        }

        public Dictionary<string?, Tuple<string, string>?>? NullableDictionaryOfNullableReferenceTypeKeyAndNullableTupleOfReferenceTypeValue()
        {
            return default;
        }
        public Dictionary<string?, Tuple<string?, string?>?>? NullableDictionaryOfNullableReferenceTypeKeyAndNullableTupleOfNullableReferenceTypeValue()
        {
            return default;
        }
    }
}
