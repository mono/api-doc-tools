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
    public class AttributeFormatterTest : BasicTests
    {
        [TestCase("PropertyTypeType", "typeof(Mono.Cecil.TypeReference)")]
        [TestCase("PropertyTypeTypeWithNull", null)]
        [TestCase("PropertyTypeTypeWithNestedType", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes/NestedClass)")]
        [TestCase("PropertyBoolType", true)]
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
        [TestCase("PropertyCharType", 'C')]
        [TestCase("PropertyStringType", "\"This is a string argument.\"")]
        [TestCase("PropertyStringTypeWithEmptyString", "")]
        [TestCase("PropertyStringTypeWithNull", null)]
        [TestCase("PropertyEnumType", ConsoleColor.Red)]
        [TestCase("PropertyEnumTypeWithUnknownValue", "(System.ConsoleColor) 2147483647")]
        [TestCase("PropertyNestedEnumType", SomeNestedTypes.NestedEnum.Read)]
        [TestCase("PropertyNestedEnumTypeWithUnknownValue", "(mdoc.Test.SampleClasses.SomeNestedTypes+NestedEnum) 2147483647")]
        [TestCase("PropertyFlagsEnumType", "System.AttributeTargets.Class | System.AttributeTargets.Enum")]
        [TestCase("PropertyFlagsEnumTypeWithAllValue", "System.AttributeTargets.All")]
        [TestCase("PropertyFlagsEnumTypeWithUndefineValueZero", "(System.AttributeTargets) 0")]
        [TestCase("PropertyNestedFlagsEnumType", "mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum.Class | mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum.Enum")]
        [TestCase("PropertyNestedFlagsEnumTypeWithUndefineValueZero", "(mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum) 0")]
        [TestCase("PropertyFlagsEnumTypeWithNotApplyAttributeValidTypeAndCombinationValue", "mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Class | mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Enum")]
        [TestCase("PropertyFlagsEnumTypeWithNotApplyAttributeValidTypeAndSingleValue", "mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Class")]
        [TestCase("PropertyFlagsEnumTypeWithNotApplyAttributeInvalidTypeAndUnknownCombinationValue", "(mdoc.Test.SampleClasses.NotApplyAttributeInvalidFlagsEnum) 5")]
        [TestCase("PropertyFlagsEnumTypeWithApplePlatformType", "Platform.Mac_10_8 | Platform.Mac_Arch64")]
        [TestCase("PropertyFlagsEnumTypeWithApplePlatformAndNoneValue", "ObjCRuntime.Platform.None")]
        [TestCase("PropertyObjectWithNull", null)]
        [TestCase("PropertyObjectWithTypeType", "typeof(Mono.Cecil.TypeReference)")]
        [TestCase("PropertyObjectWithNestedTypeType", "typeof(mdoc.Test.SampleClasses.SomeNestedTypes/NestedClass)")]
        [TestCase("PropertyObjectWithBoolType", true)]
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
        [TestCase("PropertyObjectWithCharType", 'C')]
        [TestCase("PropertyObjectWithStringType", "\"This is a string argument.\"")]
        [TestCase("PropertyObjectWithStringTypeAndEmptyString", "")]
        [TestCase("PropertyObjectWithEnumType", ConsoleColor.Red)]
        [TestCase("PropertyObjectWithEnumTypeAndUnknownValue", "(System.ConsoleColor) 2147483647")]
        [TestCase("PropertyObjectWithNestedEnumType", SomeNestedTypes.NestedEnum.Read)]
        [TestCase("PropertyObjectWithNestedEnumTypeAndUnknownValue", "(mdoc.Test.SampleClasses.SomeNestedTypes+NestedEnum) 2147483647")]
        [TestCase("PropertyObjectWithFlagsEnumType", "System.AttributeTargets.Class | System.AttributeTargets.Enum")]
        [TestCase("PropertyObjectWithFlagsEnumTypeAndAllValue", "System.AttributeTargets.All")]
        [TestCase("PropertyObjectWithFlagsEnumTypeAndUndefineValueZero", "(System.AttributeTargets) 0")]
        [TestCase("PropertyObjectWithNestedFlagsEnumType", "mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum.Class | mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum.Enum")]
        [TestCase("PropertyObjectWithNestedFlagsEnumTypeAndUndefineValueZero", "(mdoc.Test.SampleClasses.SomeNestedTypes+NestedFlagsEnum) 0")]
        [TestCase("PropertyObjectWithNotApplyAttributeValidFlagsEnumTypeAndCombinationValue", "mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Class | mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Enum")]
        [TestCase("PropertyObjectWithNotApplyAttributeValidFlagsEnumTypeAndSingleValue", "mdoc.Test.SampleClasses.NotApplyAttributeValidFlagsEnum.Class")]
        [TestCase("PropertyObjectWithNotApplyAttributeInvalidFlagsEnumTypeAndUnknownCombinationValue", "(mdoc.Test.SampleClasses.NotApplyAttributeInvalidFlagsEnum) 5")]
        [TestCase("PropertyObjectWithApplePlatformFlagsEnumType", "Platform.Mac_10_8 | Platform.Mac_Arch64")]
        [TestCase("PropertyObjectWithApplePlatformFlagsEnumTypeAndNoneValue", "ObjCRuntime.Platform.None")]
        public void TestAttributeValueFormatterWithDifferentTypes(string methodName, object argumentValue)
        {
            TestAttributeValueFormatter(methodName, argumentValue);
        }

        [TestCase("PropertyArrayOfIntType", "new System.Int32[] { 0, 0, 7 }")]
        [TestCase("PropertyArrayOfIntTypeWithNull", null)]
        [TestCase("PropertyObjectWithArrayOfIntType", "new System.Int32[] { 0, 0, 7 }")]
        public void TestAttributeFormatterWithArrayType(string methodName, object argumentValue)
        {
            TestAttributeValueFormatterWithArrayType(typeof(SomeAttribute), methodName, argumentValue);
        }

        private void TestAttributeValueFormatter(string memberName, object expectedValues)
        {
            TestAttributeValueFormatter(typeof(SomeAttribute), memberName, expectedValues);
        }

        private void TestAttributeValueFormatter(Type type, string memberName, object expectedValue)
        {
            var (argumentType, argumentValue) = GetAttributeArguments(type, memberName);
            var expectedArgumentValue = ConvertToDisplayValue(expectedValue);
            var returnValue = string.Empty;

            var attributeFormatter = new AttributeValueFormatter();
            var formatterResult = attributeFormatter.TryFormatValue(argumentValue, argumentType, out returnValue);

            Assert.IsTrue(formatterResult);
            Assert.AreEqual(expectedArgumentValue, returnValue);
        }

        private void TestAttributeValueFormatterWithArrayType(Type type, string memberName, object expectedValue)
        {
            var (argumentType, argumentValue) = GetAttributeArguments(type, memberName);
            var expectedArgumentValue = ConvertToDisplayValue(expectedValue);

            var attributeFormatter = new AttributeFormatter();
            var returnValue = attributeFormatter.MakeAttributesValueString(argumentValue, argumentType);

            Assert.AreEqual(expectedArgumentValue, returnValue);
        }

        private string ConvertToDisplayValue(object argumentValue)
        {
            if (IsNull(argumentValue))
            {
                return "null";
            }

            if (IsEmptyString(argumentValue.ToString()))
            {
                return "\"\"";
            }

            if (IsChar(argumentValue))
            {
                return string.Format("'{0}'", FilterSpecialChars(argumentValue.ToString()));
            }

            if (IsBool(argumentValue))
            {
                return argumentValue.ToString().ToLower();
            }

            if (IsEnum(argumentValue))
            {
                return $"{argumentValue.GetType().FullName}.{argumentValue}";
            }
            
            return argumentValue.ToString();
        }

        private bool IsNull(object argumentValue)
        {
            return argumentValue == null;
        }

        private bool IsEmptyString(object argumentValue)
        {
            return string.Empty.Equals(argumentValue);
        }

        private bool IsType(object argumentValue, string typeFullName)
        {
            return argumentValue.GetType().FullName == typeFullName;
        }

        private bool IsChar(object argumentValue)
        {
            return IsType(argumentValue, "System.Char");
        }

        private bool IsBool(object argumentValue)
        {
            return IsType(argumentValue, "System.Boolean");
        }

        private bool IsEnum(object argumentValue)
        {
            return argumentValue.GetType().IsEnum;
        }

        public static string FilterSpecialChars(string value)
        {
            return value
                .Replace("\0", "\\0")
                .Replace("\t", "\\t")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\f", "\\f")
                .Replace("\b", "\\b");
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
    }
}
