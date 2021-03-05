using Mono.Cecil;
using Mono.Documentation.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Documentation.Updater
{
    /// <summary>Formats attribute values. Should return true if it is able to format the value.</summary>
    public class AttributeValueFormatter
    {
        private readonly string[] numbersTypeFullNames =
        { 
            "System.Int32",
            "System.Int64" 
        };

        public virtual bool TryFormatValue (object v, ResolvedTypeInfo type, out string returnvalue)
        {
            TypeReference valueType = type.Reference;
            if (v == null)
            {
                returnvalue = "null";
                return true;
            }

            if (IsTypeType(valueType, v))
            {
                returnvalue = ConvertToType(valueType, v);
                return true;
            }

            if (IsStringType(valueType, v))
            {
                returnvalue = ConvertToString(valueType, v);
                return true;
            }

            if (valueType.FullName == "System.Char")
            {
                returnvalue = "'" + FilterSpecialChars (v.ToString ()) + "'";
                return true;
            }

            if (IsBoolType(valueType, v))
            {
                returnvalue = ConvertToBool(valueType, v);
                return true;
            }

            if (IsNumbersType(valueType, v))
            {
                returnvalue = ConvertToNumbers(valueType, v);
                return true;
            }

            if (IsEnumType(type, v))
            {
                returnvalue = ConvertToEnum(type, v);
                if (returnvalue == null)
                {
                    return false;
                }

                return true;
            }

            returnvalue = ConvertUnhandleType(type.Reference, v);
            return true;
        }

        private bool IsEnumType(ResolvedTypeInfo resolvedTypeInfo, object argumentValue)
        {
            if (resolvedTypeInfo.Definition?.IsEnum == true)
            {
                return true;
            }

            if (argumentValue is CustomAttributeArgument attributeArgument)
            {
                if (IsReferenceFromExternalAssembly(attributeArgument.Type))
                {
                    // When a type is reference by other assembly we need to get its type and identify the type whether or not is a enum type.
                    if (attributeArgument.Type.Scope is AssemblyNameReference assemblyNameReference)
                    {
                        return ConvertTypeReferenceToType(attributeArgument.Type).IsEnum;
                    }
                }
                else
                {
                    if (attributeArgument.Type is TypeDefinition argumentTypeDefinition)
                    {
                        return argumentTypeDefinition.IsEnum;
                    }
                }
            }

            return false;
        }

        private bool IsTypeType(TypeReference argumentTypeReference, object argumentValue)
        {
            return IsType(argumentTypeReference, argumentValue, "System.Type");
        }

        private bool IsStringType(TypeReference argumentTypeReference, object argumentValue)
        {
            return IsType(argumentTypeReference, argumentValue, "System.String");
        }

        private bool IsBoolType(TypeReference argumentTypeReference, object argumentValue)
        {
            return IsType(argumentTypeReference, argumentValue, "System.Boolean");
        }

        private bool IsNumbersType(TypeReference argumentTypeReference, object argumentValue)
        {
            return IsType(argumentTypeReference, argumentValue, numbersTypeFullNames);
        }

        private bool IsType(TypeReference argumentTypeReference, object argumentValue, params string[] typesFullName)
        {
            if (typesFullName.Contains(argumentTypeReference.FullName))
            {
                return true;
            }

            if (argumentValue is CustomAttributeArgument attributeArgument)
            {
                if (typesFullName.Contains(attributeArgument.Type.FullName))
                {
                    return true;
                }
            }

            return false;
        }

        private string ConvertToType(TypeReference argumentTypeReference, object argumentValue)
        {
            var valueResult = string.Empty;
            if (argumentValue is TypeReference argumentValueType)
            {
                valueResult = NativeTypeManager.GetTranslatedName(argumentValueType);   // TODO: drop NS handling
            }
            else
            {
                valueResult = GetArgumentValue(argumentTypeReference, argumentValue, "System.Type").ToString();
            }

            return "typeof(" + valueResult + ")";
        }

        private string ConvertToString(TypeReference argumentTypeReference, object argumentValue)
        {
            var valueResult = GetArgumentValue(argumentTypeReference, argumentValue, "System.String");
            if (valueResult == null)
            {
                return "null";
            }

            return "\"" + FilterSpecialChars(valueResult.ToString()) + "\"";
        }

        private string ConvertToBool(TypeReference argumentTypeReference, object argumentValue)
        {
            return GetArgumentValue(argumentTypeReference, argumentValue, "System.Boolean").ToString();
        }

        private string ConvertToNumbers(TypeReference argumentTypeReference, object argumentValue)
        {
            return GetArgumentValue(argumentTypeReference, argumentValue, numbersTypeFullNames).ToString();
        }

        private string ConvertUnhandleType(TypeReference argumentTypeReference, object argumentValue)
        {
            if (argumentValue is CustomAttributeArgument attributeArgument)
            {
                return attributeArgument.Value.ToString();
            }

            return argumentValue.ToString();
        }

        private string ConvertToEnum(ResolvedTypeInfo resolvedTypeInfo, object argumentValue)
        {
            if (resolvedTypeInfo.Definition?.IsEnum == true)
            {
                return ConvertEnumTypeValueFullName(resolvedTypeInfo.Definition, argumentValue);
            }

            if (argumentValue is CustomAttributeArgument attributeArgument)
            {
                if (IsReferenceFromExternalAssembly(attributeArgument.Type))
                {
                    // When a type is reference by other assembly we need to get its type and identify the type whether or not is a enum type.
                    if (attributeArgument.Type.Scope is AssemblyNameReference assemblyNameReference)
                    {
                        var argumentType = ConvertTypeReferenceToType(attributeArgument.Type);
                        if (argumentType.IsEnum)
                        {
                            return $"{argumentType.FullName}.{Enum.Parse(argumentType, attributeArgument.Value.ToString())}";
                        }
                    }
                }
                else
                {
                    if (attributeArgument.Type is TypeDefinition argumentTypeDefinition)
                    {
                        if (argumentTypeDefinition.IsEnum)
                        {
                            return ConvertEnumTypeValueFullName(argumentTypeDefinition, attributeArgument.Value);
                        }
                    }
                }
            }

            return null;
        }

        private string ConvertEnumTypeValueFullName(TypeDefinition typeDefinition, object argumentValue)
        {
            var typeFullName = MDocUpdater.GetDocTypeFullName(typeDefinition);
            var enumConstantNames = GetEnumerationValues(typeDefinition);
            var enumValueName = ToInt64(argumentValue);
            if (enumConstantNames.ContainsKey(enumValueName))
            {
                return typeFullName + "." + enumConstantNames[enumValueName];
            }

            return null;
        }

        private bool IsReferenceFromExternalAssembly(TypeReference typeReference)
        {
            return !(typeReference is TypeDefinition) && typeReference.Scope is AssemblyNameReference assemblyNameReference;
        }

        private Type ConvertTypeReferenceToType(TypeReference typeReference)
        {
            AssemblyLoader assemblyLoader = new AssemblyLoader();

            return assemblyLoader.ConvertToType(typeReference);
        }

        private object GetArgumentValue(TypeReference argumentTypeReference, object argumentValue, params string[] argumentsTypeFullName)
        {
            if (argumentsTypeFullName.Contains(argumentTypeReference.FullName))
            {
                return argumentValue;
            }

            CustomAttributeArgument attributeArgument = (CustomAttributeArgument)argumentValue;
            return attributeArgument.Value;
        }

        internal static IDictionary<long, string> GetEnumerationValues(TypeDefinition type)
        {
            var values = new Dictionary<long, string>();
            foreach (var f in
                    (from f in type.Fields
                     where !(f.IsRuntimeSpecialName || f.IsSpecialName)
                     select f))
            {
                values[ToInt64(f.Constant)] = f.Name;
            }
            return values;
        }

        internal static long ToInt64(object value)
        {
            if (value is ulong)
                return (long)(ulong)value;
            return Convert.ToInt64(value);
        }

        public static string FilterSpecialChars(string value)
        {
            return value.Replace("\0", "\\0")
                .Replace("\t", "\\t")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\f", "\\f")
                .Replace("\b", "\\b");
        }
    }
}