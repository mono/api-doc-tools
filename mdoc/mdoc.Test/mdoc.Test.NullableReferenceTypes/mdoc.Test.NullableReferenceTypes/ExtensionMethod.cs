namespace mdoc.Test.NullableReferenceTypes
{
    public static class ExtensionMethod
    {
        public static void NullableAndNonNullableValueType(this int? type, int? i1, int i2, int? i3)
        {
        }

        public static void NullableAndNonNullableNullableReferenceType(this string? type, string? s1, string s2, string? s3)
        {
        }

        public static void NullableAndNonNullableNullableReferenceTypeAndValueType(this string? type, string? s1, int? i1, int i2, string s2, string? s3)
        {
        }
    }
}
