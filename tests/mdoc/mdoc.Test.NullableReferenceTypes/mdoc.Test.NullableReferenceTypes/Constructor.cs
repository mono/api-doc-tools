using System;
using System.Collections.Generic;

namespace mdoc.Test.NullableReferenceTypes.Constructor
{
    public class NonNullableAndNullableValueType
    {
        public NonNullableAndNullableValueType(int i1, int? i2, int i3)
        {
        }
    }

    public class NonNullableAndNullableReferenceType
    {
        public NonNullableAndNullableReferenceType(string s1, string? s2, string s3)
        {
        }
    }

    public class InterfaceOfValueType
    {
        public InterfaceOfValueType(ICollection<int> collection1, ICollection<int>? collection2, ICollection<int> collection3)
        {
        }
    }

    public class InterfaceOfReferenceType
    {
        public InterfaceOfReferenceType(ICollection<string> collection1, ICollection<string>? collection2, ICollection<string> collection3)
        {
        }
    }
}
