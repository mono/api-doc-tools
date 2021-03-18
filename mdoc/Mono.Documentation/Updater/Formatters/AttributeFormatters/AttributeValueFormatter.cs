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
        // The types of positional and named parameters for an attribute class are limited to the attribute parameter types, which are:
        // https://github.com/dotnet/csharplang/blob/main/spec/attributes.md#attribute-parameter-types
        public string FormatValue (TypeReference argumentType, object argumentValue)
        {
            if (IsNull(argumentValue))
            {
                return "null";
            }

            if (IsArrayType(argumentType, argumentValue))
            {
                return ConvertToArrayType(argumentType, argumentValue);
            }

            if (IsTypeType(argumentType, argumentValue))
            {
                return ConvertToType(argumentType, argumentValue);
            }

            if (IsStringType(argumentType, argumentValue))
            {
                return ConvertToString(argumentType, argumentValue);
            }

            if (IsCharType(argumentType, argumentValue))
            {
                return ConvertToChar(argumentType, argumentValue);
            }

            if (IsBoolType(argumentType, argumentValue))
            {
                return ConvertToBool(argumentType, argumentValue);
            }

            if (IsEnumType(argumentType, argumentValue))
            {
                return ConvertToEnum(argumentType, argumentValue);
            }

            return ConvertUnhandlerTypeToString(argumentValue);
        }

        private bool IsNull(object argumentValue)
        {
            return argumentValue == null;
        }

        private bool IsEnumType(TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return IsEnumType(attributeArgument.Type, attributeArgument.Value);
            }

            return argumentType.Resolve().IsEnum;
        }

        private bool IsTypeType(TypeReference argumentType, object argumentValue)
        {
            return IsType("System.Type", argumentType, argumentValue);
        }

        private bool IsStringType(TypeReference argumentType, object argumentValue)
        {
            return IsType("System.String", argumentType, argumentValue);
        }

        private bool IsCharType(TypeReference argumentType, object argumentValue)
        {
            return IsType("System.Char", argumentType, argumentValue);
        }

        private bool IsBoolType(TypeReference argumentType, object argumentValue)
        {
            return IsType("System.Boolean", argumentType, argumentValue);
        }

        private bool IsType(string typeFullName, TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return IsType(typeFullName, attributeArgument.Type, attributeArgument.Value);
            }

            return argumentType.FullName.Equals(typeFullName);
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

        private bool IsArrayType(TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;

                return IsArrayType(attributeArgument.Type, attributeArgument.Value);
            }

            return argumentType is ArrayType && argumentValue is CustomAttributeArgument[];
        }

        private string ConvertToArrayType(TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;

                return ConvertToArrayType(attributeArgument.Type, attributeArgument.Value);
            }

            var arrayType = argumentType as ArrayType;
            var arrayArguments = argumentValue as CustomAttributeArgument[];
            var arrayTypeFullName = $"new {arrayType.FullName}{(arrayType.FullName.EndsWith("[]") ? "" : "[]")}";
            var arrayArgumentValues = arrayArguments.Select(i => FormatValue(i.Type, i.Value));

            return $"{arrayTypeFullName} {{ {string.Join(", ", arrayArgumentValues)} }}";
        }

        private bool IsFlagsEnum(TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return IsFlagsEnum(attributeArgument.Type, attributeArgument.Value);
            }

            var argumentTypeDefinition = argumentType.Resolve();
            var isApplyFlagsAttributeEnumType = argumentTypeDefinition.CustomAttributes.Any(a => a.AttributeType.FullName == "System.FlagsAttribute");
            var isNotApplyAttributeFlagsEnumType = IsNotApplyAttributeFlagsEnumType(argumentTypeDefinition, argumentValue);

            return isApplyFlagsAttributeEnumType || isNotApplyAttributeFlagsEnumType;
        }

        /// <summary>
        /// We have a few legacy flags enum type not apply FlagsAttribute to it.
        /// For example, Microsoft.JScript.JSFunctionAttributeEnum in .NET Framework 1.1 but the issue has been fixed in the newer version.
        /// </summary>
        private bool IsNotApplyAttributeFlagsEnumType(TypeDefinition argumentType, object argumentValue)
        {
            (var typeFullName, var enumConstants, var enumValue) = GetEnumTypeData(argumentType, argumentValue);
            if (enumConstants.ContainsKey(enumValue))
            {
                // Not is a combinations of values.
                return false;
            }

            var flagsEnumValues = enumConstants.Keys.ToList();
            flagsEnumValues.Remove(0);  // Remove invalid flags enum value zero.

            // The following example is a invalid and valid flags enum type.
            // None = 0, Read = 1, Write = 2, ReadWrite = 3 maybe is a flags enum type but sometimes it is not.
            // Read = 1, Write = 2, Open = 4, Close = 8 actually is a flags enum type.
            var minFlagsEnumValueCount = 4;
            if (flagsEnumValues.Count() >= minFlagsEnumValueCount)
            {
                long allEnumLogicalOrValue = 0;
                foreach (var item in flagsEnumValues)
                {
                    allEnumLogicalOrValue = allEnumLogicalOrValue | item;
                }

                var isFlagsEnumType = !flagsEnumValues.Any(i => (i & allEnumLogicalOrValue) != i);
                var isCombinationValue = flagsEnumValues.Count(i => (i & enumValue) != i) > 1;

                return isFlagsEnumType && isCombinationValue;
            }

            return false;
        }

        private bool IsApplePlatformEnum(TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return IsApplePlatformEnum(attributeArgument.Type, attributeArgument.Value);
            }

            return MDocUpdater.GetDocTypeFullName(argumentType).Contains("ObjCRuntime.Platform");
        }

        private string ConvertToType(TypeReference argumentType, object argumentValue)
        {
            var typeFullName = string.Empty;
            if (argumentValue is TypeReference argumentValueType)
            {
                typeFullName = NativeTypeManager.GetTranslatedName(argumentValueType);   // TODO: drop NS handling
            }
            else
            {
                typeFullName = GetArgumentValue("System.Type", argumentType, argumentValue).ToString();
            }

            return $"typeof({typeFullName})";
        }

        private string ConvertToString(TypeReference argumentType, object argumentValue)
        {
            var valueResult = GetArgumentValue("System.String", argumentType, argumentValue);
            if (valueResult == null)
            {
                return "null";
            }

            return string.Format("\"{0}\"", FilterSpecialChars(valueResult.ToString()));
        }

        private string ConvertToBool(TypeReference argumentType, object argumentValue)
        {
            return GetArgumentValue("System.Boolean", argumentType, argumentValue).ToString().ToLower();
        }

        private string ConvertToChar(TypeReference argumentType, object argumentValue)
        {
            var valueResult = GetArgumentValue("System.Char", argumentType, argumentValue).ToString();

            return string.Format("'{0}'", FilterSpecialChars(valueResult));
        }

        private string ConvertUnhandlerTypeToString(object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return ConvertUnhandlerTypeToString(attributeArgument.Value);
            }

            return argumentValue.ToString();
        }

        private string ConvertToEnum(TypeReference argumentType, object argumentValue)
        {
            if (IsFlagsEnum(argumentType, argumentValue))
            {
                if (IsApplePlatformEnum(argumentType, argumentValue))
                {
                    return ConvertToApplePlatformEnum(argumentType, argumentValue);
                }

                return ConvertToFlagsEnum(argumentType, argumentValue);
            }

            return ConvertToNormalEnum(argumentType, argumentValue);
        }

        private string ConvertToNormalEnum(TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return ConvertToNormalEnum(attributeArgument.Type, attributeArgument.Value);
            }

            (var typeFullName, var enumConstants, var enumValue) = GetEnumTypeData(argumentType, argumentValue);
            if (enumConstants.ContainsKey(enumValue))
            {
                return typeFullName + "." + enumConstants[enumValue];
            }

            return ConvertToUnknownEnum(argumentType, argumentValue);
        }

        private string ConvertToUnknownEnum(TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return ConvertToUnknownEnum(attributeArgument.Type, attributeArgument.Value);
            }

            (var typeFullName, var enumConstants, var enumValue) = GetEnumTypeData(argumentType, argumentValue);

            return $"({typeFullName}) {enumValue}";
        }

        private string ConvertToFlagsEnum(TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return ConvertToFlagsEnum(attributeArgument.Type, attributeArgument.Value);
            }

            (var typeFullName, var enumConstants, var enumValue) = GetEnumTypeData(argumentType, argumentValue);
            if (enumConstants.ContainsKey(enumValue))
            {
                return typeFullName + "." + enumConstants[enumValue];
            }

            var flagsEnumNames =
                (from i in enumConstants.Keys
                 where (enumValue & i) == i && i != 0
                 select typeFullName + "." + enumConstants[i])
                .OrderBy(val => val) // to maintain a consistent list across frameworks/versions
                .ToArray();

            if (flagsEnumNames.Length > 0)
            {
                return string.Join(" | ", flagsEnumNames);
            }

            return ConvertToUnknownEnum(argumentType, argumentValue);
        }

        private string ConvertToApplePlatformEnum(TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return ConvertToApplePlatformEnum(attributeArgument.Type, attributeArgument.Value);
            }

            (var typeFullName, var enumConstants, var enumValue) = GetEnumTypeData(argumentType, argumentValue);
            if (enumConstants.ContainsKey(enumValue))
            {
                return typeFullName + "." + enumConstants[enumValue];
            }

            return FormatApplePlatformEnum(enumValue);
        }

        private (string typeFullName, IDictionary<long, string> enumConstants, long enumValue) GetEnumTypeData(TypeReference argumentType, object argumentValue)
        {
            var argumentTypeDefinition = argumentType.Resolve();
            var typeFullName = MDocUpdater.GetDocTypeFullName(argumentTypeDefinition);
            var enumConstants = GetEnumerationValues(argumentTypeDefinition);
            var enumValue = ToInt64(argumentValue);

            return (typeFullName, enumConstants, enumValue);
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

        private object GetArgumentValue(string argumentTypeFullName, TypeReference argumentType, object argumentValue)
        {
            if (IsAssignToObject(argumentValue))
            {
                var attributeArgument = (CustomAttributeArgument)argumentValue;
                return GetArgumentValue(argumentTypeFullName, attributeArgument.Type, attributeArgument.Value);
            }

            if (argumentType.FullName.Equals(argumentTypeFullName))
            {
                return argumentValue;
            }

            throw new ArgumentException($"The argument type does not match {argumentTypeFullName}.");
        }

        protected static IDictionary<long, string> GetEnumerationValues(TypeDefinition argumentType)
        {
            var enumValues = from f in argumentType.Fields
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