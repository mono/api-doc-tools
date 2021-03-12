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
        [TestCase("ObjectPropertyWithNull", null)]
        [TestCase("TypeTypePropertyWithNull", null)]
        [TestCase("TypeTypeProperty", "typeof(Mono.Cecil.TypeReference)")]
        [TestCase("ObjectPropertyWithTypeType", "typeof(Mono.Cecil.TypeReference)")]
        [TestCase("BoolTypeProperty", true)]
        [TestCase("ObjectPropertyWithBoolType", true)]
        [TestCase("SByteTypeProperty", SByte.MinValue)]
        [TestCase("ObjectPropertyWithSByteType", SByte.MinValue)]
        [TestCase("ByteTypeProperty", Byte.MaxValue)]
        [TestCase("ObjectPropertyWithByteType", Byte.MaxValue)]
        [TestCase("Int16TypeProperty", Int16.MinValue)]
        [TestCase("ObjectPropertyWithInt16Type", Int16.MinValue)]
        [TestCase("UInt16TypeProperty", UInt16.MaxValue)]
        [TestCase("ObjectPropertyWithUInt16Type", UInt16.MaxValue)]
        [TestCase("Int32TypeProperty", Int32.MinValue)]
        [TestCase("ObjectPropertyWithInt32Type", Int32.MinValue)]
        [TestCase("UInt32TypeProperty", UInt32.MaxValue)]
        [TestCase("ObjectPropertyWithUInt32Type", UInt32.MaxValue)]
        [TestCase("Int64TypeProperty", Int64.MinValue)]
        [TestCase("ObjectPropertyWithInt64Type", Int64.MinValue)]
        [TestCase("UInt64TypeProperty", UInt64.MaxValue)]
        [TestCase("ObjectPropertyWithUInt64Type", UInt64.MaxValue)]
        [TestCase("SingleTypeProperty", Single.MinValue)]
        [TestCase("ObjectPropertyWithSingleType", Single.MinValue)]
        [TestCase("DoubleTypeProperty", Double.MinValue)]
        [TestCase("ObjectPropertyWithDoubleType", Double.MinValue)]
        [TestCase("CharTypeProperty", 'C')]
        [TestCase("ObjectPropertyWithCharType", 'C')]
        [TestCase("StringTypeProperty", "\"This is a string argument.\"")]
        [TestCase("StringTypePropertyWithNull", null)]
        [TestCase("ObjectPropertyWithStringType", "\"This is a string argument.\"")]
        [TestCase("InternalEnumTypeProperty", SomeEnum.TestEnumElement2)]
        [TestCase("ObjectPropertyWithInternalEnumType", SomeEnum.TestEnumElement2)]
        [TestCase("DotNetPlatformEnumTypeProperty", ConsoleColor.Red)]
        [TestCase("ObjectPropertyWithDotNetPlatformEnumType", ConsoleColor.Red)]
        [TestCase("FlagsEnumTypeProperty", "System.AttributeTargets.Class | System.AttributeTargets.Enum")]
        [TestCase("ObjectPropertyWithFlagsEnumType", "System.AttributeTargets.Class | System.AttributeTargets.Enum")]
        public void TestAttributeValueFormatterWithDifferentTypes(string methodName, object argumentValue)
        {
            TestAttributeValueFormatter(methodName, argumentValue);
        }


        [TestCase("ArrayOfIntTypeProperty", "new System.Int32[] { 0, 0, 7 }")]
        [TestCase("ArrayOfIntTypePropertyWithNull", null)]
        [TestCase("ObjectPropertyWithArrayOfIntType", "new System.Int32[] { 0, 0, 7 }")]
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
