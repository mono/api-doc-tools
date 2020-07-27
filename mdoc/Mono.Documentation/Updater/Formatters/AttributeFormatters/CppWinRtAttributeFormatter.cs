using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Documentation.Util;

namespace Mono.Documentation.Updater.Formatters
{
    class CppWinRtAttributeFormatter : AttributeFormatter
    {
        public override string PrefixBrackets { get; } = "/// [";
        public override string SurfixBrackets { get; } = "]";
        public override string Language => Consts.CppWinRt;

        public override bool TryGetAttributeString(CustomAttribute attribute, out string rval, string prefix = null, bool withBrackets = true)
        {
            if (attribute == null)
            {
                if (string.IsNullOrEmpty(prefix))
                {
                    rval = null;
                    return false;
                }
                rval = withBrackets ? PrefixBrackets + prefix + SurfixBrackets : prefix;
                return true;
            }

            if (IsIgnoredAttribute(attribute))
            {
                rval = null;
                return false;
            }

            TypeDefinition attrType = attribute.AttributeType as TypeDefinition;
            if (attrType != null && !DocUtils.IsPublic(attrType)
                || (FormatterManager.SlashdocFormatter.GetName(attribute.AttributeType) == null)
                || Array.IndexOf(IgnorableAttributes, attribute.AttributeType.FullName) >= 0)
            {
                rval = null;
                return false;
            }

            var fields = new List<string>();

            for (int i = 0; i < attribute.ConstructorArguments.Count; ++i)
            {
                CustomAttributeArgument argument = attribute.ConstructorArguments[i];
                string attributesValue = MakeAttributesValueString(argument.Value, argument.Type);
                fields.Add(attributesValue.StartsWith("typeof(") ? attributesValue.Substring(7, attributesValue.Length - 8) : attributesValue);
            }
            var namedArgs =
                (from namedArg in attribute.Fields
                 select new { Type = namedArg.Argument.Type, Name = namedArg.Name, Value = namedArg.Argument.Value })
                .Concat(
                        (from namedArg in attribute.Properties
                         select new { Type = namedArg.Argument.Type, Name = namedArg.Name, Value = namedArg.Argument.Value }))
                .OrderBy(v => v.Name);
            foreach (var d in namedArgs) {
                string namedArgument = MakeNamedArgumentString(d.Name, MakeAttributesValueString(d.Value, d.Type));
                fields.Add(namedArgument.StartsWith("typeof(") ? namedArgument.Substring(7, namedArgument.Length - 8) : namedArgument);
            }

            string a2 = String.Join(", ", fields.ToArray());
            if (a2 != "") a2 = "(" + a2 + ")";

            string name = attribute.GetDeclaringType();
            if (name.EndsWith("Attribute")) name = name.Substring(0, name.Length - "Attribute".Length);
            rval = withBrackets ? PrefixBrackets + prefix + name + a2 + SurfixBrackets
                : prefix + name + a2;
            return true;
        }
    }
}
