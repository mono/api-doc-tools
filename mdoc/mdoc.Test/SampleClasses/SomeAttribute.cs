using Mono.Cecil;
using System;
using System.Collections.Generic;

namespace mdoc.Test.SampleClasses
{
    public class SomeAttribute
    {
        [AttributeDataType(TypeType = typeof(TypeReference))]
        public void PropertyTypeType()
        {
        }

        [AttributeDataType(TypeType = null)]
        public void PropertyTypeTypeWithNull()
        {
        }

        [AttributeDataType(TypeType = typeof(ICollection<>))]
        public void PropertyTypeTypeWithCollectionOfNone()
        {
        }

        [AttributeDataType(TypeType = typeof(ICollection<int>))]
        public void PropertyTypeTypeWithCollectionOfInt()
        {
        }

        [AttributeDataType(TypeType = typeof(IDictionary<,>))]
        public void PropertyTypeTypeWithDictionaryOfNone()
        {
        }

        [AttributeDataType(TypeType = typeof(IDictionary<int, int>))]
        public void PropertyTypeTypeWithDictionaryOfInt()
        {
        }

        [AttributeDataType(BoolType = true)]
        public void PropertyBoolType()
        {
        }

        [AttributeDataType(SByteType = SByte.MinValue)]
        public void PropertySByteType()
        {
        }

        [AttributeDataType(ByteType = Byte.MaxValue)]
        public void PropertyByteType()
        {
        }

        [AttributeDataType(Int16Type = Int16.MinValue)]
        public void PropertyInt16Type()
        {
        }

        [AttributeDataType(UInt16Type = UInt16.MaxValue)]
        public void PropertyUInt16Type()
        {
        }

        [AttributeDataType(Int32Type = Int32.MinValue)]
        public void PropertyInt32Type()
        {
        }

        [AttributeDataType(UInt32Type = UInt32.MaxValue)]
        public void PropertyUInt32Type()
        {
        }

        [AttributeDataType(Int64Type = Int64.MinValue)]
        public void PropertyInt64Type()
        {
        }

        [AttributeDataType(UInt64Type = UInt64.MaxValue)]
        public void PropertyUInt64Type()
        {
        }

        [AttributeDataType(SingleType = Single.MinValue)]
        public void PropertySingleType()
        {
        }

        [AttributeDataType(DoubleType = Double.MinValue)]
        public void PropertyDoubleType()
        {
        }

        [AttributeDataType(CharType = 'C')]
        public void PropertyCharType()
        {
        }

        [AttributeDataType(StringType = "This is a string argument.")]
        public void PropertyStringType()
        {
        }

        [AttributeDataType(StringType = null)]
        public void PropertyStringTypeWithNull()
        {
        }

        [AttributeDataType(StringType = "")]
        public void PropertyStringTypeWithEmptyString()
        {
        }

        [AttributeDataType(ArrayOfIntType = new[] { 0, 0, 7 })]
        public void PropertyArrayOfIntType()
        {
        }

        [AttributeDataType(ArrayOfIntType = null)]
        public void PropertyArrayOfIntTypeWithNull()
        {
        }

        [AttributeDataType(EnumType = ConsoleColor.Red)]
        public void PropertyEnumType()
        {
        }

        [AttributeDataType(EnumType = (ConsoleColor)int.MaxValue)]
        public void PropertyEnumTypeWithUnknownValue()
        {
        }

        [AttributeDataType(FlagsEnumType = AttributeTargets.Class | AttributeTargets.Enum)]
        public void PropertyFlagsEnumType()
        {
        }

        [AttributeDataType(FlagsEnumType = AttributeTargets.All)]
        public void PropertyFlagsEnumTypeWithAllValue()
        {
        }

        [AttributeDataType(FlagsEnumType = (AttributeTargets)0)]
        public void PropertyFlagsEnumTypeWithUndefineValueZero()
        {
        }

        [AttributeDataType(ObjectType = null)]
        public void PropertyObjectWithNull()
        {
        }

        [AttributeDataType(ObjectType = typeof(TypeReference))]
        public void PropertyObjectWithTypeType()
        {
        }


        [AttributeDataType(ObjectType = typeof(ICollection<>))]
        public void PropertyObjectWithCollectionTypeOfNone()
        {
        }

        [AttributeDataType(ObjectType = typeof(ICollection<int>))]
        public void PropertyObjectWithCollectionTypeOfInt()
        {
        }

        [AttributeDataType(ObjectType = typeof(IDictionary<,>))]
        public void PropertyObjectWithDictionaryTypeOfNone()
        {
        }

        [AttributeDataType(ObjectType = typeof(IDictionary<int, int>))]
        public void PropertyObjectWithDictionaryTypeOfInt()
        {
        }

        [AttributeDataType(ObjectType = true)]
        public void PropertyObjectWithBoolType()
        {
        }

        [AttributeDataType(ObjectType = SByte.MinValue)]
        public void PropertyObjectWithSByteType()
        {
        }

        [AttributeDataType(ObjectType = Byte.MaxValue)]
        public void PropertyObjectWithByteType()
        {
        }

        [AttributeDataType(ObjectType = Int16.MinValue)]
        public void PropertyObjectWithInt16Type()
        {
        }

        [AttributeDataType(ObjectType = UInt16.MaxValue)]
        public void PropertyObjectWithUInt16Type()
        {
        }

        [AttributeDataType(ObjectType = Int32.MinValue)]
        public void PropertyObjectWithInt32Type()
        {
        }

        [AttributeDataType(ObjectType = UInt32.MaxValue)]
        public void PropertyObjectWithUInt32Type()
        {
        }

        [AttributeDataType(ObjectType = Int64.MinValue)]
        public void PropertyObjectWithInt64Type()
        {
        }

        [AttributeDataType(ObjectType = UInt64.MaxValue)]
        public void PropertyObjectWithUInt64Type()
        {
        }

        [AttributeDataType(ObjectType = Single.MinValue)]
        public void PropertyObjectWithSingleType()
        {
        }

        [AttributeDataType(ObjectType = Double.MinValue)]
        public void PropertyObjectWithDoubleType()
        {
        }

        [AttributeDataType(ObjectType = 'C')]
        public void PropertyObjectWithCharType()
        {
        }

        [AttributeDataType(ObjectType = "This is a string argument.")]
        public void PropertyObjectWithStringType()
        {
        }

        [AttributeDataType(ObjectType = "")]
        public void PropertyObjectWithStringTypeAndEmptyString()
        {
        }

        [AttributeDataType(ObjectType = new[] { 0, 0, 7 })]
        public void PropertyObjectWithArrayOfIntType()
        {
        }

        [AttributeDataType(ObjectType = ConsoleColor.Red)]
        public void PropertyObjectWithEnumType()
        {
        }

        [AttributeDataType(ObjectType = (ConsoleColor)int.MaxValue)]
        public void PropertyObjectWithEnumTypeAndUnknownValue()
        {
        }

        [AttributeDataType(ObjectType = AttributeTargets.Class | AttributeTargets.Enum)]
        public void PropertyObjectWithFlagsEnumType()
        {
        }

        [AttributeDataType(ObjectType = AttributeTargets.All)]
        public void PropertyObjectWithFlagsEnumTypeAndAllValue()
        {
        }

        [AttributeDataType(ObjectType = (AttributeTargets)0)]
        public void PropertyObjectWithFlagsEnumTypeAndUndefineValueZero()
        {
        }
    }
}