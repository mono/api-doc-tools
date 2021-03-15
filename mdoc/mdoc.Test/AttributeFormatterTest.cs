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
        [TestCase("PropertyTypeTypeWithNull", null)]
        [TestCase("PropertyTypeType", "typeof(Mono.Cecil.TypeReference)")]
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
        [TestCase("PropertyStringTypeWithNull", null)]
        [TestCase("PropertyInternalEnumType", SomeEnum.TestEnumElement2)]
        [TestCase("PropertyDotNetPlatformEnumType", ConsoleColor.Red)]
        [TestCase("PropertyFlagsEnumType", "System.AttributeTargets.Class | System.AttributeTargets.Enum")]
        [TestCase("PropertyFlagsEnumTypeWithAll", "System.AttributeTargets.All")]
        [TestCase("PropertyObjectWithNull", null)]
        [TestCase("PropertyObjectWithTypeType", "typeof(Mono.Cecil.TypeReference)")]
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
        [TestCase("PropertyObjectWithInternalEnumType", SomeEnum.TestEnumElement2)]
        [TestCase("PropertyObjectWithDotNetPlatformEnumType", ConsoleColor.Red)]
        [TestCase("PropertyObjectWithFlagsEnumType", "System.AttributeTargets.Class | System.AttributeTargets.Enum")]
        [TestCase("PropertyObjectWithAllFlagsEnumType", "System.AttributeTargets.All")]
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
            var methodDefinition = GetMethod(type, memberName);
            var methodAttribute = AttributeFormatter.GetCustomAttributes(methodDefinition).First();
            var attributeArguments = GetAttributeArguments(methodAttribute.Item1).First();
            var expectedArgumentValue = ConvertToDisplayValue(expectedValue);
            var returnValue = string.Empty;

            var attributeFormatter = new AttributeValueFormatter();
            var formatterResult = attributeFormatter.TryFormatValue(attributeArguments.argumentValue, new ResolvedTypeInfo(attributeArguments.argumentType), out returnValue);

            Assert.IsTrue(formatterResult);
            Assert.AreEqual(expectedArgumentValue, returnValue);
        }

        private void TestAttributeValueFormatterWithArrayType(Type type, string memberName, object expectedValue)
        {
            var methodDefinition = GetMethod(type, memberName);
            var methodAttribute = AttributeFormatter.GetCustomAttributes(methodDefinition).First();
            var attributeArguments = GetAttributeArguments(methodAttribute.Item1).First();
            var expectedArgumentValue = ConvertToDisplayValue(expectedValue);

            var attributeFormatter = new AttributeFormatter();
            var returnValue = attributeFormatter.MakeAttributesValueString(attributeArguments.argumentValue, attributeArguments.argumentType);

            Assert.AreEqual(expectedArgumentValue, returnValue);
        }

        private string ConvertToDisplayValue(object argumentValue)
        {
            if (argumentValue == null)
            {
                return "null";
            }

            Type valueType = argumentValue.GetType();
            switch(valueType.FullName)
            {
                case "System.Char":
                    return string.Format("'{0}'", FilterSpecialChars(argumentValue.ToString()));

                case "System.Boolean":
                    return argumentValue.ToString().ToLower();

                default:
                    return valueType.IsEnum ? $"{valueType.FullName}.{argumentValue}" : argumentValue.ToString();
            }
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

        private IEnumerable<(TypeReference argumentType, object argumentValue)> GetAttributeArguments(CustomAttribute customAttribute)
        {
            for (int i = 0; i < customAttribute.ConstructorArguments.Count; ++i)
            {
                yield return (customAttribute.ConstructorArguments[i].Type, customAttribute.ConstructorArguments[i].Value);
            }

            foreach (var item in GetAttributeArgumentsFromFieldsAndProperties(customAttribute))
            {
                yield return (item.argumentType, item.argumentValue);
            }
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
