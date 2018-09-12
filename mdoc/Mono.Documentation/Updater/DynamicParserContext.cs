using System.Collections.ObjectModel;
using System.Linq;

using Mono.Cecil;

using Mono.Documentation.Util;

namespace Mono.Documentation.Updater
{
    public class DynamicParserContext
    {
        public ReadOnlyCollection<bool> TransformFlags;
        public int TransformIndex;

        public DynamicParserContext (ICustomAttributeProvider provider)
        {
            CustomAttribute da;
            if (provider.HasCustomAttributes &&
                    (da = (provider.CustomAttributes.SafeCast<CustomAttribute> ()
                        .SingleOrDefault (ca => ca.GetDeclaringType () == "System.Runtime.CompilerServices.DynamicAttribute"))) != null)
            {
                CustomAttributeArgument[] values = da.ConstructorArguments.Count == 0
                    ? new CustomAttributeArgument[0]
                    : (CustomAttributeArgument[])da.ConstructorArguments[0].Value;

                TransformFlags = new ReadOnlyCollection<bool> (values.Select (t => (bool)t.Value).ToArray ());
            }
        }
    }
}