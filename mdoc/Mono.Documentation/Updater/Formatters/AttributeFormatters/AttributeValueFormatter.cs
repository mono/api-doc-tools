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
        public virtual bool TryFormatValue (object argumentValue, ResolvedTypeInfo resolvedType, out string returnvalue)
        {
            // The types of positional and named parameters for an attribute class are limited to the attribute parameter types, which are:
            // https://github.com/dotnet/csharplang/blob/main/spec/attributes.md#attribute-parameter-types
            if (argumentValue == null)
            {
                returnvalue = "null";
                return true;
            }

            TypeReference valueType = resolvedType.Reference;
            if (IsTypeType(valueType, argumentValue))
            {
                returnvalue = ConvertToType(valueType, argumentValue);
                return true;
            }

            if (IsStringType(valueType, argumentValue))
            {
                returnvalue = ConvertToString(valueType, argumentValue);
                return true;
            }

            if (IsCharType(valueType, argumentValue))
            {
                returnvalue = ConvertToChar(valueType, argumentValue);
                return true;
            }

            if (IsBoolType(valueType, argumentValue))
            {
                returnvalue = ConvertToBool(valueType, argumentValue);
                return true;
            }

            if (IsEnumType(valueType, argumentValue))
            {
                returnvalue = ConvertToEnum(valueType, argumentValue);
                if (returnvalue == null)
                {
                    return false;
                }

                return true;
            }

            returnvalue = ConvertUnhandlerTypeToString(valueType, argumentValue);
            return true;
        }

        private bool IsEnumType(TypeReference argumentTypeReference, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return IsEnumType(attributeArgument.Type, attributeArgument.Value);
            }

            try
            {
                return ConvertToTypeDefinition(argumentTypeReference).IsEnum;
            }
            catch (DllNotFoundException) { }

            return false;
        }

        private bool IsTypeType(TypeReference argumentTypeReference, object argumentValue)
        {
            return IsType("System.Type", argumentTypeReference, argumentValue);
        }

        private bool IsStringType(TypeReference argumentTypeReference, object argumentValue)
        {
            return IsType("System.String", argumentTypeReference, argumentValue);
        }

        private bool IsCharType(TypeReference argumentTypeReference, object argumentValue)
        {
            return IsType("System.Char", argumentTypeReference, argumentValue);
        }

        private bool IsBoolType(TypeReference argumentTypeReference, object argumentValue)
        {
            return IsType("System.Boolean", argumentTypeReference, argumentValue);
        }

        private bool IsType(string typeFullName, TypeReference argumentTypeReference, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return IsType(typeFullName, attributeArgument.Type, attributeArgument.Value);
            }

            return argumentTypeReference.FullName.Equals(typeFullName);
        }

        /// <summary>
        /// When a property type of an attribute is an object type you can assign any type to it,
        /// so we need to convert the object type to a concrete object type.
        /// </summary>
        /// <param name="argumentValue">The value of the argument.</param>
        private bool IsAssignToObject(object argumentValue)
        {
            return argumentValue is CustomAttributeArgument;
        }

        private bool IsFlagsEnum(TypeReference argumentTypeReference, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return IsFlagsEnum(attributeArgument.Type, attributeArgument.Value);
            }

            try
            {
                var argumentTypeDefinition = ConvertToTypeDefinition(argumentTypeReference);

                return argumentTypeDefinition.CustomAttributes.Any(a => a.AttributeType.FullName == "System.FlagsAttribute");
            }
            catch (DllNotFoundException) { }

            return false;
        }

        private bool IsApplePlatformEnum(TypeReference argumentTypeReference, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return IsApplePlatformEnum(attributeArgument.Type, attributeArgument.Value);
            }

            return MDocUpdater.GetDocTypeFullName(argumentTypeReference).Contains("ObjCRuntime.Platform");
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
                valueResult = GetArgumentValue("System.Type", argumentTypeReference, argumentValue).ToString();
            }

            return $"typeof({valueResult})";
        }

        private string ConvertToString(TypeReference argumentTypeReference, object argumentValue)
        {
            var valueResult = GetArgumentValue("System.String", argumentTypeReference, argumentValue);
            if (valueResult == null)
            {
                return "null";
            }

            return string.Format("\"{0}\"", FilterSpecialChars(valueResult.ToString()));
        }

        private string ConvertToBool(TypeReference argumentTypeReference, object argumentValue)
        {
            return GetArgumentValue("System.Boolean", argumentTypeReference, argumentValue).ToString().ToLower();
        }

        private string ConvertToChar(TypeReference argumentTypeReference, object argumentValue)
        {
            var valueResult = GetArgumentValue("System.Char", argumentTypeReference, argumentValue).ToString();

            return string.Format("'{0}'", FilterSpecialChars(valueResult));
        }

        private string ConvertUnhandlerTypeToString(TypeReference argumentTypeReference, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return ConvertUnhandlerTypeToString(attributeArgument.Type, attributeArgument.Value);
            }

            return argumentValue.ToString();
        }

        private string ConvertToEnum(TypeReference argumentTypeReference, object argumentValue)
        {
            if (IsFlagsEnum(argumentTypeReference, argumentValue))
            {
                if (IsApplePlatformEnum(argumentTypeReference, argumentValue))
                {
                    return ConvertToApplePlatformEnum(argumentValue);
                }

                return ConvertToFlagsEnum(argumentTypeReference, argumentValue);
            }
            else
            {
                var enumValue = ConvertToNormalEnum(argumentTypeReference, argumentValue);
                if (enumValue == null)
                {
                    // When assigning a value of flags type to an object type of a property
                    // but the flags type is not defined in .NET platform assemblies or current assembly(is a reference from an external assembly)
                    // which the flags type will have not a FlagsAttribute annotation.
                    enumValue = ConvertToFlagsEnum(argumentTypeReference, argumentValue);
                }

                return enumValue;
            }
        }

        private string ConvertToNormalEnum(TypeReference argumentTypeReference, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return ConvertToNormalEnum(attributeArgument.Type, attributeArgument.Value);
            }

            try
            {
                var argumentTypeDefinition = ConvertToTypeDefinition(argumentTypeReference);

                return ConvertEnumTypeValueToFullName(argumentTypeDefinition, argumentValue);
            }
            catch (DllNotFoundException)
            {
                return "NotFoundEnumType";
            }
        }

        private string ConvertEnumTypeValueToFullName(TypeDefinition typeDefinition, object argumentValue)
        {
            var typeFullName = MDocUpdater.GetDocTypeFullName(typeDefinition);
            var enumConstantNames = GetEnumerationValues(typeDefinition);
            var enumValue = ToInt64(argumentValue);

            if (enumConstantNames.ContainsKey(enumValue))
            {
                return typeFullName + "." + enumConstantNames[enumValue];
            }

            return null;
        }

        private string ConvertToFlagsEnum(TypeReference argumentTypeReference, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return ConvertToFlagsEnum(attributeArgument.Type, attributeArgument.Value);
            }

            try
            {
                var argumentTypeDefinition = ConvertToTypeDefinition(argumentTypeReference);
                var typeFullName = MDocUpdater.GetDocTypeFullName(argumentTypeDefinition);
                var enumConstantNames = GetEnumerationValues(argumentTypeDefinition);
                var enumValue = ToInt64(argumentValue);

                if (enumConstantNames.ContainsKey(enumValue))
                {
                    return typeFullName + "." + enumConstantNames[enumValue];
                }
                else
                {
                    var flagsEnumNames =
                        (from i in enumConstantNames.Keys
                         where (enumValue & i) == i && i != 0
                         select typeFullName + "." + enumConstantNames[i])
                        .DefaultIfEmpty(enumValue.ToString())
                        .OrderBy(val => val) // to maintain a consistent list across frameworks/versions
                        .ToArray();

                    if (flagsEnumNames.Length > 0)
                    {
                        return string.Join(" | ", flagsEnumNames);
                    }
                }

                return null;
            }
            catch (DllNotFoundException)
            {
                return "NotFoundFlagsEnumType";
            }
        }

        private TypeDefinition ConvertToTypeDefinition(TypeReference typeReference)
        {
            typeReference = typeReference.Resolve() ?? typeReference;
            if (IsReferenceFromExternalAssembly(typeReference))
            {
                throw new DllNotFoundException();

                // When a type is reference from other assemblies and the assembly are don't exist in the currently analyzing assembly's folder or not is .NET platform assemblies.
                // We could not get its TypeDefinition or Type which will block our conversion of the value of enum type to an actual full name of the enum type.
                AssemblyLoader assemblyLoader = new AssemblyLoader();

                return assemblyLoader.ConvertToTypeDefinition(typeReference);
            }

            return typeReference as TypeDefinition;
        }

        private string ConvertToApplePlatformEnum(object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return ConvertToApplePlatformEnum(attributeArgument.Value);
            }

            return FormatApplePlatformEnum(ToInt64(argumentValue));
        }

        private string FormatApplePlatformEnum(long enumValue)
        {
            int iosarch, iosmajor, iosminor, iossubminor;
            int macarch, macmajor, macminor, macsubminor;
            GetEncodingiOS(enumValue, out iosarch, out iosmajor, out iosminor, out iossubminor);
            GetEncodingMac((ulong)enumValue, out macarch, out macmajor, out macminor, out macsubminor);

            if (iosmajor == 0 & iosminor == 0 && iossubminor == 0)
            {
                return FormatApplePlatformEnumValue("Mac", macarch, macmajor, macminor, macsubminor);
            }

            if (macmajor == 0 & macminor == 0 && macsubminor == 0)
            {
                return FormatApplePlatformEnumValue("iOS", iosarch, iosmajor, iosminor, iossubminor);
            }

            return string.Format("(Platform){0}", enumValue);
        }

        private string FormatApplePlatformEnumValue(string plat, int arch, int major, int minor, int subminor)
        {
            var archstring = string.Empty;
            switch (arch)
            {
                case 1:
                    archstring = "32";
                    break;
                case 2:
                    archstring = "64";
                    break;
            }

            return string.Format("Platform.{4}_{0}_{1}{2} | Platform.{4}_Arch{3}",
                major,
                minor,
                subminor == 0 ? "" : "_" + subminor.ToString(),
                archstring,
                plat
            );
        }

        private void GetEncodingiOS(long entireLong, out int archindex, out int major, out int minor, out int subminor)
        {
            long lowerBits = entireLong & 0xffffffff;
            int lowerBitsAsInt = (int)lowerBits;
            GetEncodingApplePlatform(lowerBitsAsInt, out archindex, out major, out minor, out subminor);
        }

        private void GetEncodingMac(ulong entireLong, out int archindex, out int major, out int minor, out int subminor)
        {
            ulong higherBits = entireLong & 0xffffffff00000000;
            int higherBitsAsInt = (int)((higherBits) >> 32);
            GetEncodingApplePlatform(higherBitsAsInt, out archindex, out major, out minor, out subminor);
        }

        private void GetEncodingApplePlatform(Int32 encodedBits, out int archindex, out int major, out int minor, out int subminor)
        {
            // format is AAJJNNSS
            archindex = (int)((encodedBits & 0xFF000000) >> 24);
            major = (int)((encodedBits & 0x00FF0000) >> 16);
            minor = (int)((encodedBits & 0x0000FF00) >> 8);
            subminor = (int)((encodedBits & 0x000000FF) >> 0);
        }

        private bool IsReferenceFromExternalAssembly(TypeReference typeReference)
        {
            return !(typeReference is TypeDefinition) && typeReference.Scope is AssemblyNameReference assemblyNameReference;
        }

        private object GetArgumentValue(string argumentTypeFullName, TypeReference argumentTypeReference, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return GetArgumentValue(argumentTypeFullName, attributeArgument.Type, attributeArgument.Value);
            }

            if (argumentTypeReference.FullName.Equals(argumentTypeFullName))
            {
                return argumentValue;
            }

            throw new ArgumentException($"The argument type does not match {argumentTypeFullName}.");
        }

        protected static IDictionary<long, string> GetEnumerationValues(TypeDefinition type)
        {
            var enumValues = from f in type.Fields
                             where !(f.IsRuntimeSpecialName || f.IsSpecialName)
                             select f;

            var values = new Dictionary<long, string>();
            foreach (var item in enumValues)
            {
                values[ToInt64(item.Constant)] = item.Name;
            }

            return values;
        }

        protected static long ToInt64(object value)
        {
            if (value is ulong)
                return (long)(ulong)value;

            return Convert.ToInt64(value);
        }

        protected static string FilterSpecialChars(string value)
        {
            return value
                .Replace("\0", "\\0")
                .Replace("\t", "\\t")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\f", "\\f")
                .Replace("\b", "\\b");
        }
    }
}