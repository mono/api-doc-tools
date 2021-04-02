using mdoc.Test.SampleClasses;
using Mono.Cecil;
using Mono.Documentation.Updater;
using Mono.Documentation.Updater.Formatters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mdoc.Test
{
    [TestFixture()]
    public class AttributeValueFormatterTest : BasicTests
    {
        [TestCase("PropertyTypeType", "typeof(Mono.Cecil.TypeReference)")]
        [TestCase("PropertyTypeTypeWithNull", "null")]
        [TestCase("PropertyTypeTypeWithNestedType", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedClass)")]
        [TestCase("PropertyTypeTypeWithUnboundCollection", "typeof(System.Collections.Generic.ICollection<>)")]
        [TestCase("PropertyTypeTypeWithCollectionOfInt", "typeof(System.Collections.Generic.ICollection<System.Int32>)")]
        [TestCase("PropertyTypeTypeWithUnboundDictionary", "typeof(System.Collections.Generic.IDictionary<,>)")]
        [TestCase("PropertyTypeTypeWithDictionaryOfInt", "typeof(System.Collections.Generic.IDictionary<System.Int32,System.Int32>)")]
        [TestCase("PropertyTypeTypeWithUnboundCustomGenericType", "typeof(mdoc.Test.SampleClasses.SomeGenericClass<>)")]
        [TestCase("PropertyTypeTypeWithCustomGenericTypeOfInt", "typeof(mdoc.Test.SampleClasses.SomeGenericClass<System.Int32>)")]
        [TestCase("PropertyTypeTypeWithUnboundNestedGenericType", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedGenericType<>)")]
        [TestCase("PropertyTypeTypeWithNestedGenericTypeOfInt", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedGenericType<System.Int32>)")]
        [TestCase("PropertyTypeTypeWithUnboundInnerNestedGenericType", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedGenericType<>+InnerNestedGenericType<>)")]
        [TestCase("PropertyTypeTypeWithInnerNestedGenericTypeOfInt", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedGenericType<System.String>+InnerNestedGenericType<System.Int32>)")]
        [TestCase("PropertyBoolType", "true")]
        [TestCase("PropertySByteType", SByte.MinValue)]
        [TestCase("PropertyByteType", Byte.MaxValue)]
        [TestCase("PropertyInt16Type", Int16.MinValue)]
        [TestCase("PropertyUInt16Type", UInt16.MaxValue)]
        [TestCase("PropertyInt32Type", Int32.MinValue)]
        [TestCase("PropertyUInt32Type", UInt32.MaxValue)]
        [TestCase("PropertyInt64Type", Int64.MinValue)]
        [TestCase("PropertyUInt64Type", UInt64.MaxValue)]
        [TestCase("PropertySingleType", Single.MinValue)]
        [TestCase("PropertyDoubleType", Double.MinValue)]
        [TestCase("PropertyCharType", "'C'")]
        [TestCase("PropertyStringType", "\"This is a string argument.\"")]
        [TestCase("PropertyStringTypeWithEmptyString", "\"\"")]
        [TestCase("PropertyStringTypeWithNull", "null")]
        [TestCase("PropertyEnumType", "System.ConsoleColor.Red")]
        [TestCase("PropertyEnumTypeWithUnknownValue", "(System.ConsoleColor) 2147483647")]
        [TestCase("PropertyNestedEnumType", "mdoc.Test.SampleClasses.SomeNestedTypes+NestedEnum.Read")]
        [TestCase("PropertyNestedEnumTypeWithUnknownValue", "(mdoc.Test.SampleClasses.SomeNestedTypes+NestedEnum) 2147483647")]
        [TestCase("PropertyFlagsEnumType", "System.AttributeTargets.Class | System.AttributeTargets.Enum")]
        [TestCase("PropertyFlagsEnumTypeWithAllValue", "System.AttributeTargets.All")]
        [TestCase("PropertyFlagsEnumTypeWithUndefineValueZero", "(System.AttributeTargets) 0")]
        [TestCase("PropertyDuplicateFlagsEnumTypeWithCombinationValue", "mdoc.Test.SampleClasses.SomeFlagsEnum.Open | mdoc.Test.SampleClasses.SomeFlagsEnum.ReadWrite")]
        [TestCase("PropertyNestedFlagsEnumType", "mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum.Class | mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum.Enum")]
        [TestCase("PropertyNestedFlagsEnumTypeWithUndefineValueZero", "(mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum) 0")]
        [TestCase("PropertyFlagsEnumTypeWithNotApplyAttributeValidTypeAndCombinationValue", "mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Class | mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Enum")]
        [TestCase("PropertyFlagsEnumTypeWithNotApplyAttributeValidTypeAndSingleValue", "mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Class")]
        [TestCase("PropertyFlagsEnumTypeWithNotApplyAttributeInvalidTypeAndUnknownCombinationValue", "(mdoc.Test.SampleClasses.NotApplyAttributeInvalidFlagsEnum) 5")]
        [TestCase("PropertyFlagsEnumTypeWithApplePlatformType", "Platform.Mac_10_8 | Platform.Mac_Arch64")]
        [TestCase("PropertyFlagsEnumTypeWithApplePlatformAndNoneValue", "ObjCRuntime.Platform.None")]
        [TestCase("PropertyArrayOfIntType", "new System.Int32[] { 0, 0, 7 }")]
        [TestCase("PropertyArrayOfIntTypeWithNull", "null")]
        [TestCase("PropertyObjectWithNull", "null")]
        [TestCase("PropertyObjectWithTypeType", "typeof(Mono.Cecil.TypeReference)")]
        [TestCase("PropertyObjectWithNestedTypeType", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedClass)")]
        [TestCase("PropertyObjectWithUnboundCollectionType", "typeof(System.Collections.Generic.ICollection<>)")]
        [TestCase("PropertyObjectWithCollectionTypeOfInt", "typeof(System.Collections.Generic.ICollection<System.Int32>)")]
        [TestCase("PropertyObjectWithUnboundDictionaryType", "typeof(System.Collections.Generic.IDictionary<,>)")]
        [TestCase("PropertyObjectWithDictionaryTypeOfInt", "typeof(System.Collections.Generic.IDictionary<System.Int32,System.Int32>)")]
        [TestCase("PropertyObjectWithUnboundCustomGenericType", "typeof(mdoc.Test.SampleClasses.SomeGenericClass<>)")]
        [TestCase("PropertyObjectWithCustomGenericTypeOfInt", "typeof(mdoc.Test.SampleClasses.SomeGenericClass<System.Int32>)")]
        [TestCase("PropertyObjectWithUnboundNestedGenericType", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedGenericType<>)")]
        [TestCase("PropertyObjectWithNestedGenericTypeOfInt", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedGenericType<System.Int32>)")]
        [TestCase("PropertyObjectWithUnboundInnerNestedGenericType", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedGenericType<>+InnerNestedGenericType<>)")]
        [TestCase("PropertyObjectWithInnerNestedGenericTypeOfInt", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes+NestedGenericType<System.String>+InnerNestedGenericType<System.Int32>)")]
        [TestCase("PropertyObjectWithBoolType", "true")]
        [TestCase("PropertyObjectWithSByteType", SByte.MinValue)]
        [TestCase("PropertyObjectWithByteType", Byte.MaxValue)]
        [TestCase("PropertyObjectWithInt16Type", Int16.MinValue)]
        [TestCase("PropertyObjectWithUInt16Type", UInt16.MaxValue)]
        [TestCase("PropertyObjectWithInt32Type", Int32.MinValue)]
        [TestCase("PropertyObjectWithUInt32Type", UInt32.MaxValue)]
        [TestCase("PropertyObjectWithInt64Type", Int64.MinValue)]
        [TestCase("PropertyObjectWithUInt64Type", UInt64.MaxValue)]
        [TestCase("PropertyObjectWithSingleType", Single.MinValue)]
        [TestCase("PropertyObjectWithDoubleType", Double.MinValue)]
        [TestCase("PropertyObjectWithCharType", "'C'")]
        [TestCase("PropertyObjectWithStringType", "\"This is a string argument.\"")]
        [TestCase("PropertyObjectWithStringTypeAndEmptyString", "\"\"")]
        [TestCase("PropertyObjectWithEnumType", "System.ConsoleColor.Red")]
        [TestCase("PropertyObjectWithEnumTypeAndUnknownValue", "(System.ConsoleColor) 2147483647")]
        [TestCase("PropertyObjectWithNestedEnumType", "mdoc.Test.SampleClasses.SomeNestedTypes+NestedEnum.Read")]
        [TestCase("PropertyObjectWithNestedEnumTypeAndUnknownValue", "(mdoc.Test.SampleClasses.SomeNestedTypes+NestedEnum) 2147483647")]
        [TestCase("PropertyObjectWithFlagsEnumType", "System.AttributeTargets.Class | System.AttributeTargets.Enum")]
        [TestCase("PropertyObjectWithFlagsEnumTypeAndAllValue", "System.AttributeTargets.All")]
        [TestCase("PropertyObjectWithFlagsEnumTypeAndUndefineValueZero", "(System.AttributeTargets) 0")]
        [TestCase("PropertyObjectWithDuplicateFlagsEnumTypeAndCombinationValue", "mdoc.Test.SampleClasses.SomeFlagsEnum.Open | mdoc.Test.SampleClasses.SomeFlagsEnum.ReadWrite")]
        [TestCase("PropertyObjectWithNestedFlagsEnumType", "mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum.Class | mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum.Enum")]
        [TestCase("PropertyObjectWithNestedFlagsEnumTypeAndUndefineValueZero", "(mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum) 0")]
        [TestCase("PropertyObjectWithNotApplyAttributeValidFlagsEnumTypeAndCombinationValue", "mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Class | mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Enum")]
        [TestCase("PropertyObjectWithNotApplyAttributeValidFlagsEnumTypeAndSingleValue", "mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Class")]
        [TestCase("PropertyObjectWithNotApplyAttributeInvalidFlagsEnumTypeAndUnknownCombinationValue", "(mdoc.Test.SampleClasses.NotApplyAttributeInvalidFlagsEnum) 5")]
        [TestCase("PropertyObjectWithApplePlatformFlagsEnumType", "Platform.Mac_10_8 | Platform.Mac_Arch64")]
        [TestCase("PropertyObjectWithApplePlatformFlagsEnumTypeAndNoneValue", "ObjCRuntime.Platform.None")]
        [TestCase("PropertyObjectWithArrayOfIntType", "new System.Int32[] { 0, 0, 7 }")]
        public void TestFormatValueWithDifferentTypes(string methodName, object argumentValue)
        {
            TestAttributeValueFormatter(typeof(SomeAttribute), methodName, argumentValue);
        }

        [TestCase("GetEnumerator", "typeof(mdoc.Test.SampleClasses.SomeIteratorStateMachine<,>+<GetEnumerator>d__0)", typeof(SomeIteratorStateMachine<,>))]
        [TestCase("GetEnumerator", "typeof(mdoc.Test.SampleClasses.SomeIteratorStateMachine<,>+SomeNestedIteratorStateMachine<,>+<GetEnumerator>d__0)", typeof(SomeIteratorStateMachine<,>.SomeNestedIteratorStateMachine<,>))]
        [TestCase("WithParameterType", "typeof(mdoc.Test.SampleClasses.SomeIteratorStateMachine<,>+<WithParameterType>d__2<,>)", typeof(SomeIteratorStateMachine<,>))]
        [TestCase("WithParameterType", "typeof(mdoc.Test.SampleClasses.SomeIteratorStateMachine<,>+SomeNestedIteratorStateMachine<,>+<WithParameterType>d__2<,>)", typeof(SomeIteratorStateMachine<,>.SomeNestedIteratorStateMachine<,>))]
        public void TestFormartValueWithIteratorStateMachineAttribute(string methodName, object argumentValue, Type type)
        {
            TestAttributeValueFormatter(type, methodName, argumentValue);
        }

        private void TestAttributeValueFormatter(Type type, string memberName, object expectedValue)
        {
            var (argumentType, argumentValue) = GetAttributeArguments(type, memberName);

            var attributeFormatter = new AttributeValueFormatter();
            var formatValue = attributeFormatter.Format(argumentType, argumentValue);

            Assert.AreEqual(expectedValue.ToString(), formatValue);
        }

        private (TypeReference argumentType, object argumentValue) GetAttributeArguments(Type type, string memberName)
        {
            var methodDefinition = GetMethod(type, memberName);
            var methodAttribute = AttributeFormatter.GetCustomAttributes(methodDefinition).First();
            CustomAttribute customAttribute = methodAttribute.Item1;

            var customAttributeList = new List<(TypeReference, object)>();
            for (int i = 0; i < customAttribute.ConstructorArguments.Count; ++i)
            {
                customAttributeList.Add((customAttribute.ConstructorArguments[i].Type, customAttribute.ConstructorArguments[i].Value));
            }

            foreach (var item in GetAttributeArgumentsFromFieldsAndProperties(customAttribute))
            {
                customAttributeList.Add((item.argumentType, item.argumentValue));
            }

            return customAttributeList.First();
        }

        private IEnumerable<(string argumentName, TypeReference argumentType, object argumentValue)> GetAttributeArgumentsFromFieldsAndProperties(CustomAttribute customAttribute)
        {
            return (from namedArg in customAttribute.Fields
                    select (namedArg.Name, namedArg.Argument.Type, namedArg.Argument.Value))
                    .Concat(from namedArg in customAttribute.Properties
                            select (namedArg.Name, namedArg.Argument.Type, namedArg.Argument.Value))
                    .OrderBy(v => v.Name);
        }

        protected override TypeDefinition GetType(Type type)
        {
            var moduleName = type.Module.FullyQualifiedName;
            return GetType(moduleName, ConvertNestedTypeFullName(type.FullName));
        }

        private string ConvertNestedTypeFullName(string fullName)
        {
            // Simply of implementation for help nested class convert the full name from .NET style to mono.cecil style.
            return fullName.Replace("+", "/");
        }
    }
}
