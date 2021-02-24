using System;
using System.Collections.Generic;

namespace mdoc.Test.NullableReferenceTypes
{
    public class MethodParameter
    {
        public void ParamsArrayOfNullableValueType(int i, params int?[] array)
        {
        }

        public void ParamsArrayOfNullableReferenceType(string s, params object?[] array)
        {
        }

        public void NullableAndNonNullableValueType(int i1, int? i2, int i3)
        {
        }

        public void NullableAndNonNullableReferenceType(string s1, string? s2, string s3)
        {
        }

        public void NullableGenericValueTypeOfValueType(ReadOnlySpan<int> s1, ReadOnlySpan<int?> s2, ReadOnlySpan<int> s3)
        {
        }

        public void NullableGenericValueTypeOfReferenceType(ReadOnlySpan<string> s1, ReadOnlySpan<string?> s2, ReadOnlySpan<string> s3)
        {
        }

        public void NullableAndNonNullableInterfaceOfValueType(ICollection<int> collection1, ICollection<int>? collection2, ICollection<int> collection3)
        {
        }

        public void NullableAndNonNullableInterfaceOfReferenceType(ICollection<string> collection1, ICollection<string>? collection2, ICollection<string> collection3)
        {
        }

        public void NonNullableValueTypeWithOutModifier(string? value, out bool result)
        {
            result = false;
        }

        public void NonNullableValueTypeWithRefModifier(string? value, ref bool result)
        {
        }
    }
}