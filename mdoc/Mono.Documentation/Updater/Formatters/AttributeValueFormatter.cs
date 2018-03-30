using System;

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
                returnvalue = "\"" + MDocUpdater.FilterSpecialChars (v.ToString ()) + "\"";
                return true;
            }
            if (valueType.FullName == "System.Char")
            {
                returnvalue = "'" + MDocUpdater.FilterSpecialChars (v.ToString ()) + "'";
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
            var values = MDocUpdater.GetEnumerationValues (valueDef);
            long c = MDocUpdater.ToInt64 (v);
            if (values.ContainsKey (c))
            {
                returnvalue = typename + "." + values[c];
                return true;
            }

            returnvalue = null;
            return false;
        }
    }
}