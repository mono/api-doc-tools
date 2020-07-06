using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

using Mono.Documentation.Util;

namespace Mono.Documentation.Updater
{
    /// <summary>Formats attribute values. Should return true if it is able to format the value.</summary>
    class AttributeValueFormatter
    {
        public virtual bool TryFormatValue (object v, ResolvedTypeInfo type, out string returnvalue)
        {
            TypeReference valueType = type.Reference;
            if (v == null)
            {
                returnvalue = "null";
                return true;
            }
            if (valueType.FullName == "System.Type")
            {
                var vTypeRef = v as TypeReference;
                if (vTypeRef != null)
                    returnvalue = "typeof(" + NativeTypeManager.GetTranslatedName (vTypeRef) + ")"; // TODO: drop NS handling
                else
                    returnvalue = "typeof(" + v.ToString () + ")";

                return true;
            }
            if (valueType.FullName == "System.String")
            {
                returnvalue = "\"" + FilterSpecialChars (v.ToString ()) + "\"";
                return true;
            }
            if (valueType.FullName == "System.Char")
            {
                returnvalue = "'" + FilterSpecialChars (v.ToString ()) + "'";
                return true;
            }
            if (v is Boolean)
            {
                returnvalue = (bool)v ? "true" : "false";
                return true;
            }

            TypeDefinition valueDef = type.Definition;
            if (valueDef == null || !valueDef.IsEnum)
            {
                returnvalue = v.ToString ();
                return true;
            }

            string typename = MDocUpdater.GetDocTypeFullName (valueType);
            var values = GetEnumerationValues (valueDef);
            long c = ToInt64 (v);
            if (values.ContainsKey (c))
            {
                returnvalue = typename + "." + values[c];
                return true;
            }

            returnvalue = null;
            return false;
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