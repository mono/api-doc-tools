using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mdoc.Mono.Documentation.Updater.Formatters
{
    // Nullable Reference Types https://github.com/dotnet/roslyn/blob/master/docs/features/nullable-reference-types.md
    // Nullable Metadata https://github.com/dotnet/roslyn/blob/master/docs/features/nullable-metadata.md
    public static class CSharpTypeNameFormatter
    {
        private const string NullableAttribute = "System.Runtime.CompilerServices.NullableAttribute";
        private const string NullableContextAttribute = "System.Runtime.CompilerServices.NullableContextAttribute";

        private const int MaxArgumentCount = 100;

        private const byte ObliviousNullableAttribute = 0;
        private const byte NotAnnotatedNullableAttribute = 1;
        private const byte AnnotatedNullableAttribute = 2;

        public static bool? IsNullableReferenceType(ParameterDefinition parameterDefinition)
        {
            var nullableAttribute = FindCustomAttribute(parameterDefinition, NullableAttribute);
            if (nullableAttribute == null)
            {
                if (parameterDefinition.Method is MethodDefinition methodDefinition)
                {
                    nullableAttribute = FindCustomAttribute(methodDefinition, NullableContextAttribute);
                    if (nullableAttribute == null)
                    {
                        var typeDefinition = methodDefinition.DeclaringType;
                        nullableAttribute = FindCustomAttribute(typeDefinition, NullableContextAttribute);
                    }
                }
            }

            return GetTypeNullability(nullableAttribute).First();
        }

        public static IEnumerable<bool?> IsNullableReferenceType(ICollection<ICustomAttributeProvider> customAttributeProviders)
        {
            CustomAttribute nullableAttribute = null;
            string currentAttribute = NullableAttribute;
            foreach (var item in customAttributeProviders)
            {
                nullableAttribute = FindCustomAttribute(item, currentAttribute);
                if (nullableAttribute != null)
                {
                    break;
                }

                currentAttribute = NullableContextAttribute;
            }

            return GetTypeNullability(nullableAttribute);
        }

        private static CustomAttribute FindCustomAttribute(ICustomAttributeProvider customAttributeProvider, string customAttributeName)
        {
            if (customAttributeProvider.HasCustomAttributes)
            {
                return customAttributeProvider.CustomAttributes.SingleOrDefault(a => a.AttributeType.FullName.Equals(customAttributeName));
            }

            return null;
        }

        private static IEnumerable<bool?> GetTypeNullability(CustomAttribute typeCustomAttribute)
        {
            if (typeCustomAttribute != null)
            {
                var nullableAttributeValue = typeCustomAttribute.ConstructorArguments[0].Value;
                if (nullableAttributeValue is CustomAttributeArgument[] nullableAttributeArguments)
                {
                    return nullableAttributeArguments.Select(a => IsAnnotatedNullableAttribute((byte)a.Value));
                }

                return Enumerable.Repeat<bool?>(IsAnnotatedNullableAttribute((byte)nullableAttributeValue), MaxArgumentCount);
            }

            return Enumerable.Repeat<bool?>(null, MaxArgumentCount);
        }

        private static bool? IsAnnotatedNullableAttribute(byte value)
        {
            switch (value)
            {
                case AnnotatedNullableAttribute:
                    return true;

                case NotAnnotatedNullableAttribute:
                    return false;

                case ObliviousNullableAttribute:
                    return null;
            }

            throw new ArgumentOutOfRangeException("value", value, $"Nullable attribute value:'{value}' is not a valid type argument.");
        }
    }
}
