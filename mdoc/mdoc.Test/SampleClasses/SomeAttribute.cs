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

        [AttributeDataType(TypeType = typeof(SomeNestedTypes.NestedClass))]
        public void PropertyTypeTypeWithNestedType()
        {
        }

        [AttributeDataType(TypeType = typeof(ICollection<>))]
        public void PropertyTypeTypeWithUnboundCollection()
        {
        }

        [AttributeDataType(TypeType = typeof(ICollection<int>))]
        public void PropertyTypeTypeWithCollectionOfInt()
        {
        }

        [AttributeDataType(TypeType = typeof(IDictionary<,>))]
        public void PropertyTypeTypeWithUnboundDictionary()
        {
        }

        [AttributeDataType(TypeType = typeof(IDictionary<int, int>))]
        public void PropertyTypeTypeWithDictionaryOfInt()
        {
        }

        [AttributeDataType(TypeType = typeof(SomeGenericClass<>))]
        public void PropertyTypeTypeWithUnboundCustomGenericType()
        {
        }

        [AttributeDataType(TypeType = typeof(SomeGenericClass<int>))]
        public void PropertyTypeTypeWithCustomGenericTypeOfInt()
        {
        }

        [AttributeDataType(TypeType = typeof(SomeNestedTypes.NestedGenericType<>))]
        public void PropertyTypeTypeWithUnboundNestedGenericType()
        {
        }

        [AttributeDataType(TypeType = typeof(SomeNestedTypes.NestedGenericType<int>))]
        public void PropertyTypeTypeWithNestedGenericTypeOfInt()
        {
        }


        [AttributeDataType(TypeType = typeof(SomeNestedTypes.NestedGenericType<>.InnerNestedGenericType<>))]
        public void PropertyTypeTypeWithUnboundInnerNestedGenericType()
        {
        }

        [AttributeDataType(TypeType = typeof(SomeNestedTypes.NestedGenericType<string>.InnerNestedGenericType<int>))]
        public void PropertyTypeTypeWithInnerNestedGenericTypeOfInt()
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

        [AttributeDataType(NestedEnumType = SomeNestedTypes.NestedEnum.Read)]
        public void PropertyNestedEnumType()
        {
        }

        [AttributeDataType(NestedEnumType = (SomeNestedTypes.NestedEnum)int.MaxValue)]
        public void PropertyNestedEnumTypeWithUnknownValue()
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

        [AttributeDataType(DuplicateFlagsEnumType = SomeFlagsEnum.Read | SomeFlagsEnum.Write | SomeFlagsEnum.Open)]
        public void PropertyDuplicateFlagsEnumTypeWithCombinationValue()
        {
        }

        [AttributeDataType(NestedFlagsEnumType = SomeNestedTypes.NestedFlagsEnum.Class | SomeNestedTypes.NestedFlagsEnum.Enum)]
        public void PropertyNestedFlagsEnumType()
        {
        }

        [AttributeDataType(NestedFlagsEnumType = (SomeNestedTypes.NestedFlagsEnum)0)]
        public void PropertyNestedFlagsEnumTypeWithUndefineValueZero()
        {
        }

        [AttributeDataType(NotApplyAttributeFlagsEnumType = NotApplyAttributeValidFlagsEnum.Class | NotApplyAttributeValidFlagsEnum.Enum)]
        public void PropertyFlagsEnumTypeWithNotApplyAttributeValidTypeAndCombinationValue()
        {
        }

        [AttributeDataType(NotApplyAttributeFlagsEnumType = NotApplyAttributeValidFlagsEnum.Class)]
        public void PropertyFlagsEnumTypeWithNotApplyAttributeValidTypeAndSingleValue()
        {
        }

        [AttributeDataType(NotApplyAttributeInvalidFlagsEnumType = (NotApplyAttributeInvalidFlagsEnum)5)]
        public void PropertyFlagsEnumTypeWithNotApplyAttributeInvalidTypeAndUnknownCombinationValue()
        {
        }

        [AttributeDataType(ApplePlatformFlagsEnumType = ObjCRuntime.Platform.Mac_10_8 | ObjCRuntime.Platform.Mac_Arch64)]
        public void PropertyFlagsEnumTypeWithApplePlatformType()
        {
        }

        [AttributeDataType(ApplePlatformFlagsEnumType = ObjCRuntime.Platform.None)]
        public void PropertyFlagsEnumTypeWithApplePlatformAndNoneValue()
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

        [AttributeDataType(ObjectType = typeof(SomeNestedTypes.NestedClass))]
        public void PropertyObjectWithNestedTypeType()
        {
        }

        [AttributeDataType(ObjectType = typeof(ICollection<>))]
        public void PropertyObjectWithUnboundCollectionType()
        {
        }

        [AttributeDataType(ObjectType = typeof(ICollection<int>))]
        public void PropertyObjectWithCollectionTypeOfInt()
        {
        }

        [AttributeDataType(ObjectType = typeof(IDictionary<,>))]
        public void PropertyObjectWithUnboundDictionaryType()
        {
        }

        [AttributeDataType(ObjectType = typeof(IDictionary<int, int>))]
        public void PropertyObjectWithDictionaryTypeOfInt()
        {
        }

        [AttributeDataType(ObjectType = typeof(SomeGenericClass<>))]
        public void PropertyObjectWithUnboundCustomGenericType()
        {
        }

        [AttributeDataType(ObjectType = typeof(SomeGenericClass<int>))]
        public void PropertyObjectWithCustomGenericTypeOfInt()
        {
        }

        [AttributeDataType(ObjectType = typeof(SomeNestedTypes.NestedGenericType<>))]
        public void PropertyObjectWithUnboundNestedGenericType()
        {
        }

        [AttributeDataType(ObjectType = typeof(SomeNestedTypes.NestedGenericType<int>))]
        public void PropertyObjectWithNestedGenericTypeOfInt()
        {
        }

        [AttributeDataType(ObjectType = typeof(SomeNestedTypes.NestedGenericType<>.InnerNestedGenericType<>))]
        public void PropertyObjectWithUnboundInnerNestedGenericType()
        {
        }

        [AttributeDataType(ObjectType = typeof(SomeNestedTypes.NestedGenericType<string>.InnerNestedGenericType<int>))]
        public void PropertyObjectWithInnerNestedGenericTypeOfInt()
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

        [AttributeDataType(ObjectType = SomeNestedTypes.NestedEnum.Read)]
        public void PropertyObjectWithNestedEnumType()
        {
        }

        [AttributeDataType(ObjectType = (SomeNestedTypes.NestedEnum)int.MaxValue)]
        public void PropertyObjectWithNestedEnumTypeAndUnknownValue()
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

        [AttributeDataType(ObjectType = SomeFlagsEnum.Read | SomeFlagsEnum.Write | SomeFlagsEnum.Open)]
        public void PropertyObjectWithDuplicateFlagsEnumTypeAndCombinationValue()
        {
        }

        [AttributeDataType(ObjectType = SomeNestedTypes.NestedFlagsEnum.Class | SomeNestedTypes.NestedFlagsEnum.Enum)]
        public void PropertyObjectWithNestedFlagsEnumType()
        {
        }

        [AttributeDataType(ObjectType = (SomeNestedTypes.NestedFlagsEnum)0)]
        public void PropertyObjectWithNestedFlagsEnumTypeAndUndefineValueZero()
        {
        }

        [AttributeDataType(ObjectType = NotApplyAttributeValidFlagsEnum.Class | NotApplyAttributeValidFlagsEnum.Enum)]
        public void PropertyObjectWithNotApplyAttributeValidFlagsEnumTypeAndCombinationValue()
        {
        }

        [AttributeDataType(ObjectType = NotApplyAttributeValidFlagsEnum.Class)]
        public void PropertyObjectWithNotApplyAttributeValidFlagsEnumTypeAndSingleValue()
        {
        }

        [AttributeDataType(ObjectType = (NotApplyAttributeInvalidFlagsEnum)5)]
        public void PropertyObjectWithNotApplyAttributeInvalidFlagsEnumTypeAndUnknownCombinationValue()
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
    }
}
