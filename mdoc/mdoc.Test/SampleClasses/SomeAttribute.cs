using Mono.Cecil;
using System;

namespace mdoc.Test.SampleClasses
{
    public class SomeAttribute
    {
        [TestDataType(ObjectType = null)]
        public void ObjectPropertyWithNull()
        {
        }

        [TestDataType(TypeType = null)]
        public void TypeTypePropertyWithNull()
        {
        }

        [TestDataType(TypeType = typeof(TypeReference))]
        public void TypeTypeProperty()
        {
        }

        [TestDataType(ObjectType = typeof(TypeReference))]
        public void ObjectPropertyWithTypeType()
        {
        }

        [TestDataType(BoolType = true)]
        public void BoolTypeProperty()
        {
        }

        [TestDataType(ObjectType = true)]
        public void ObjectPropertyWithBoolType()
        {
        }

        [TestDataType(SByteType = SByte.MinValue)]
        public void SByteTypeProperty()
        {
        }

        [TestDataType(ObjectType = SByte.MinValue)]
        public void ObjectPropertyWithSByteType()
        {
        }

        [TestDataType(ByteType = Byte.MaxValue)]
        public void ByteTypeProperty()
        {
        }

        [TestDataType(ObjectType = Byte.MaxValue)]
        public void ObjectPropertyWithByteType()
        {
        }

        [TestDataType(Int16Type = Int16.MinValue)]
        public void Int16TypeProperty()
        {
        }

        [TestDataType(ObjectType = Int16.MinValue)]
        public void ObjectPropertyWithInt16Type()
        {
        }

        [TestDataType(UInt16Type = UInt16.MaxValue)]
        public void UInt16TypeProperty()
        {
        }

        [TestDataType(ObjectType = UInt16.MaxValue)]
        public void ObjectPropertyWithUInt16Type()
        {
        }

        [TestDataType(Int32Type = Int32.MinValue)]
        public void Int32TypeProperty()
        {
        }

        [TestDataType(ObjectType = Int32.MinValue)]
        public void ObjectPropertyWithInt32Type()
        {
        }

        [TestDataType(UInt32Type = UInt32.MaxValue)]
        public void UInt32TypeProperty()
        {
        }

        [TestDataType(ObjectType = UInt32.MaxValue)]
        public void ObjectPropertyWithUInt32Type()
        {
        }

        [TestDataType(Int64Type = Int64.MinValue)]
        public void Int64TypeProperty()
        {
        }

        [TestDataType(ObjectType = Int64.MinValue)]
        public void ObjectPropertyWithInt64Type()
        {
        }

        [TestDataType(UInt64Type = UInt64.MaxValue)]
        public void UInt64TypeProperty()
        {
        }

        [TestDataType(ObjectType = UInt64.MaxValue)]
        public void ObjectPropertyWithUInt64Type()
        {
        }

        [TestDataType(SingleType = Single.MinValue)]
        public void SingleTypeProperty()
        {
        }

        [TestDataType(ObjectType = Single.MinValue)]
        public void ObjectPropertyWithSingleType()
        {
        }

        [TestDataType(DoubleType = Double.MinValue)]
        public void DoubleTypeProperty()
        {
        }

        [TestDataType(ObjectType = Double.MinValue)]
        public void ObjectPropertyWithDoubleType()
        {
        }

        [TestDataType(CharType = 'C')]
        public void CharTypeProperty()
        {
        }

        [TestDataType(ObjectType = 'C')]
        public void ObjectPropertyWithCharType()
        {
        }

        [TestDataType(StringType = "This is a string argument.")]
        public void StringTypeProperty()
        {
        }

        [TestDataType(StringType = null)]
        public void StringTypePropertyWithNull()
        {
        }

        [TestDataType(ObjectType = "This is a string argument.")]
        public void ObjectPropertyWithStringType()
        {
        }

        [TestDataType(ArrayOfIntType = new[] { 0, 0, 7 })]
        public void ArrayOfIntTypeProperty()
        {
        }

        [TestDataType(ArrayOfIntType = null)]
        public void ArrayOfIntTypePropertyWithNull()
        {
        }

        [TestDataType(ObjectType = new[] { 0, 0, 7 })]
        public void ObjectPropertyWithArrayOfIntType()
        {
        }

        [TestDataType(FlagsEnumType = AttributeTargets.Class | AttributeTargets.Enum)]
        public void FlagsEnumTypeProperty()
        {
        }

        [TestDataType(ObjectType = AttributeTargets.Class | AttributeTargets.Enum)]
        public void ObjectPropertyWithFlagsEnumType()
        {
        }

        [TestDataType(InternalEnumType = SomeEnum.TestEnumElement2)]
        public void InternalEnumTypeProperty()
        {
        }

        [TestDataType(ObjectType = SomeEnum.TestEnumElement2)]
        public void ObjectPropertyWithInternalEnumType()
        {
        }

        [TestDataType(DotNetPlatformEnumType = ConsoleColor.Red)]
        public void DotNetPlatformEnumTypeProperty()
        {
        }

        [TestDataType(ObjectType = ConsoleColor.Red)]
        public void ObjectPropertyWithDotNetPlatformEnumType()
        {
        }
    }
}
