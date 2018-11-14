using Mono.Cecil;
using Mono.Documentation.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Documentation.Updater.Formatters
{
    class CSharpAttributeDefinitionFormatter : AttributeDefinitionFormatter
    {
        public override bool TryFormatValue(object v, ResolvedTypeInfo type, out string returnvalue)
        {
            return base.TryFormatValue(v, type, out returnvalue);
        }

        public override bool TryFormatAttribute(CustomAttribute attribute, string prefix, out string formattedValue)
        {
            return base.TryFormatAttribute(attribute, prefix, out formattedValue);
        }

    }
}
