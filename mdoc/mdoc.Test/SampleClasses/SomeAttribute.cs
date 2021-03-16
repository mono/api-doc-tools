using Mono.Cecil;
using System;

namespace mdoc.Test.SampleClasses
{
    public class SomeAttribute
    {
        [AttributeDataType(TypeType = null)]
        public void PropertyTypeTypeWithNull()
        {
        }

        [AttributeDataType(TypeType = typeof(TypeReference))]
        public void PropertyTypeType()
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

        [AttributeDataType(ArrayOfIntType = new[] { 0, 0, 7 })]
        public void PropertyArrayOfIntType()
        {
        }

        [AttributeDataType(ArrayOfIntType = null)]
        public void PropertyArrayOfIntTypeWithNull()
        {
        }

        [AttributeDataType(FlagsEnumType = AttributeTargets.Class | AttributeTargets.Enum)]
        public void PropertyFlagsEnumType()
        {
        }

        [AttributeDataType(FlagsEnumType = AttributeTargets.All)]
        public void PropertyFlagsEnumTypeWithAll()
        {
        }

        [AttributeDataType(NotApplyAttributeFlagsEnumType = NotApplyAttributeFlagsEnum.Class | NotApplyAttributeFlagsEnum.Enum)]
        public void PropertyNotApplyAttributeFlagsEnumTypeWithCombinationValue()
        {
        }

        [AttributeDataType(NotApplyAttributeFlagsEnumType = NotApplyAttributeFlagsEnum.Class)]
        public void PropertyNotApplyAttributeFlagsEnumTypeWithSingleValue()
        {
        }

        [AttributeDataType(NotApplyAttributeInvalidFlagsEnumType = NotApplyAttributeInvalidFlagsEnum.Read | NotApplyAttributeInvalidFlagsEnum.Open)]
        public void PropertyNotApplyAttributeInvalidFlagsEnumTypeWithCombinationValue()
        {
        }

        [AttributeDataType(ApplePlatformFlagsEnumType = ObjCRuntime.Platform.Mac_10_8 | ObjCRuntime.Platform.Mac_Arch64)]
        public void PropertyApplePlatformFlagsEnumType()
        {
        }

        [AttributeDataType(ApplePlatformFlagsEnumType = ObjCRuntime.Platform.None)]
        public void PropertyApplePlatformFlagsEnumTypeWithNone()
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

        [AttributeDataType(ObjectType = null)]
        public void PropertyObjectWithNull()
        {
        }

        [AttributeDataType(ObjectType = typeof(TypeReference))]
        public void PropertyObjectWithTypeType()
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

        [AttributeDataType(ObjectType = new[] { 0, 0, 7 })]
        public void PropertyObjectWithArrayOfIntType()
        {
        }

        [AttributeDataType(ObjectType = AttributeTargets.Class | AttributeTargets.Enum)]
        public void PropertyObjectWithFlagsEnumType()
        {
        }

        [AttributeDataType(ObjectType = AttributeTargets.All)]
        public void PropertyObjectWithAllFlagsEnumType()
        {
        }

        [AttributeDataType(ObjectType = NotApplyAttributeFlagsEnum.Class | NotApplyAttributeFlagsEnum.Enum)]
        public void PropertyObjectWithNotApplyAttributeFlagsEnumTypeAndCombinationValue()
        {
        }

        [AttributeDataType(ObjectType = NotApplyAttributeFlagsEnum.Class)]
        public void PropertyObjectWithNotApplyAttributeFlagsEnumTypeAndSingleValue()
        {
        }

        [AttributeDataType(ObjectType = NotApplyAttributeInvalidFlagsEnum.Read | NotApplyAttributeInvalidFlagsEnum.Open)]
        public void PropertyObjectWithNotApplyAttributeInvalidFlagsEnumTypeAndCombinationValue()
        {
        }

        [AttributeDataType(ObjectType = ObjCRuntime.Platform.Mac_10_8 | ObjCRuntime.Platform.Mac_Arch64)]
        public void PropertyObjectWithApplePlatformFlagsEnumType()
        {
        }

        [AttributeDataType(ObjectType = ObjCRuntime.Platform.None)]
        public void PropertyObjectWithApplePlatformFlagsEnumTypeAndNoneValue()
        {
        }

        [AttributeDataType(ObjectType = ConsoleColor.Red)]
        public void PropertyObjectWithEnumType()
        {
        }

        [AttributeDataType(ObjectType = (ConsoleColor)int.MaxValue)]
        public void PropertyObjectWithUnknowEnumValue()
        {
        }
    }
}
