using System.Linq;

using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    /// <summary>Flags enum formatter that assumes powers of two values.</summary>
    /// <remarks>As described here: https://msdn.microsoft.com/en-us/library/vstudio/ms229062(v=vs.100).aspx</remarks>
    class StandardFlagsEnumFormatter : AttributeValueFormatter
    {
        public override bool TryFormatValue (object v, ResolvedTypeInfo type, out string returnvalue)
        {
            TypeReference valueType = type.Reference;
            TypeDefinition valueDef = type.Definition;
            if (valueDef.CustomAttributes.Any (ca => ca.AttributeType.FullName == "System.FlagsAttribute"))
            {

                string typename = MDocUpdater.GetDocTypeFullName (valueType);
                var values = MDocUpdater.GetEnumerationValues (valueDef);
                long c = MDocUpdater.ToInt64 (v);
                returnvalue = string.Join (" | ",
                    (from i in values.Keys
                     where (c & i) == i && i != 0
                     select typename + "." + values[i])
                    .DefaultIfEmpty (c.ToString ()).ToArray ());

                return true;
            }

            returnvalue = null;
            return false;
        }
    }
}