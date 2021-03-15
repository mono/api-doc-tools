using System;

namespace mdoc.Test.SampleClasses
{
    public class TestDataTypeAttribute : Attribute
    {
        public object ObjectType { get; set; }

        public Type TypeType { get; set; }

        public bool BoolType { get; set; }

        public SByte SByteType { get; set; }

        public Byte ByteType { get; set; }

        public Int16 Int16Type { get; set; }

        public UInt16 UInt16Type { get; set; }

        public Int32 Int32Type { get; set; }

        public Int32? NullableOfInt32Type { get; set; }

        public UInt32 UInt32Type { get; set; }

        public Int64 Int64Type { get; set; }

        public UInt64 UInt64Type { get; set; }

        public Single SingleType { get; set; }

        public Double DoubleType { get; set; }

        public char CharType { get; set; }

        public string StringType { get; set; }

        public int[] ArrayOfIntType { get; set; }

        public AttributeTargets FlagsEnumType { get; set; }

        public ConsoleColor DotNetPlatformEnumType { get; set; }

        public SomeEnum InternalEnumType { get; set; }
    }
}
