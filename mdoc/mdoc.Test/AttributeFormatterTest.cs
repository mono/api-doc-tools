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
        [TestCase("PropertiyObjectWithBoolType", true)]
        [TestCase("PropertiyObjectWithNull", null)]
        [TestCase("PropertiyObjectWithInternalEnumType", SomeEnum.TestEnumElement2)]
        [TestCase("PropertiyObjectWithExternalEnumType", ConsoleColor.Red)]
        public void TestPropertiyObject(string methodName, params object[] argumentValue)
        {
            TestAttribute(methodName, argumentValue);

        }

        private void TestAttribute(string memberName, params object[] expectedValues)
        {
            TestAttribute(typeof(SomeAttribute), memberName, expectedValues);
        }

        private void TestAttribute(Type type, string memberName, params object[] expectedValues)
        {
            var methodDefinition = GetMethod(type, memberName);
            var methodAttribute = AttributeFormatter.GetCustomAttributes(methodDefinition).First();
            var attributeArguments = GetAttributeArguments(methodAttribute.Item1);

            Assert.AreEqual(expectedValues.Length, attributeArguments.Count());

            var expectedValueIndex = 0;
            AttributeValueFormatter formatter = new AttributeValueFormatter();
            foreach (var item in attributeArguments)
            {
                var argumentValue = ConvertArgumentValueToDisplayName(expectedValues[expectedValueIndex++]);
                var returnValue = string.Empty;
                var formatterResult = formatter.TryFormatValue(item.argumentValue, new ResolvedTypeInfo(item.argumentType), out returnValue);

                Assert.IsTrue(formatterResult);
                Assert.AreEqual(argumentValue, returnValue);
            }
        }


        private string ConvertArgumentValueToDisplayName(object argumentValue)
        {
            if (argumentValue == null)
            {
                return "null";
            }

            Type valueType = argumentValue.GetType();
            if (valueType.IsEnum)
            {
                return $"{valueType.FullName}.{argumentValue}";
            }

            return argumentValue.ToString();
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
                    select ValueTuple.Create(namedArg.Name, namedArg.Argument.Type, namedArg.Argument.Value))
                    .Concat(from namedArg in customAttribute.Properties
                            select ValueTuple.Create(namedArg.Name, namedArg.Argument.Type, namedArg.Argument.Value))
                    .OrderBy(v => v.Item1);
        }
    }
}
