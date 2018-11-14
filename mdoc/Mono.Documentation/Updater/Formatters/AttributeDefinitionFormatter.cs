using Mono.Cecil;
using Mono.Documentation.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Documentation.Updater.Formatters
{
    class AttributeDefinitionFormatter  : AttributeValueFormatter
    {
        SlashDocCSharpMemberFormatter slashdocFormatter = new SlashDocCSharpMemberFormatter();

        private static string[] IgnorableAttributes = {
		    // Security related attributes
		    "System.Reflection.AssemblyKeyFileAttribute",
            "System.Reflection.AssemblyDelaySignAttribute",
		    // Present in @RefType
		    "System.Runtime.InteropServices.OutAttribute",
		    // For naming the indexer to use when not using indexers
		    "System.Reflection.DefaultMemberAttribute",
		    // for decimal constants
		    "System.Runtime.CompilerServices.DecimalConstantAttribute",
		    // compiler generated code
		    Consts.CompilerGeneratedAttribute,
		    // more compiler generated code, e.g. iterator methods
		    "System.Diagnostics.DebuggerHiddenAttribute",
            "System.Runtime.CompilerServices.FixedBufferAttribute",
            "System.Runtime.CompilerServices.UnsafeValueTypeAttribute",
            "System.Runtime.CompilerServices.AsyncStateMachineAttribute",
		    // extension methods
		    "System.Runtime.CompilerServices.ExtensionAttribute",
		    // Used to differentiate 'object' from C#4 'dynamic'
		    "System.Runtime.CompilerServices.DynamicAttribute",
		    // F# compiler attribute
		    "Microsoft.FSharp.Core.CompilationMapping",
        };

        // TODO: most list of ignored attributes, and method IsIgnored to static
        public static bool IsIgnoredAttribute(CustomAttribute customAttribute)
        {
            // An Obsolete attribute with a known string is added to all ref-like structs
            // https://github.com/dotnet/csharplang/blob/master/proposals/csharp-7.2/span-safety.md#metadata-representation-or-ref-like-structs
            return customAttribute.AttributeType.FullName == typeof(ObsoleteAttribute).FullName
                && customAttribute.HasConstructorArguments
                && customAttribute.ConstructorArguments.First().Value.ToString() == Consts.RefTypeObsoleteString;
        }

        public virtual bool TryFormatAttribute(CustomAttribute attribute, string prefix, out string formattedValue)
        {
            formattedValue = null;
            TypeDefinition attrType = attribute.AttributeType as TypeDefinition;
            if (attrType != null && !DocUtils.IsPublic(attrType))
                return false;
            if (slashdocFormatter.GetName(attribute.AttributeType) == null)
                return false;

            if (Array.IndexOf(IgnorableAttributes, attribute.AttributeType.FullName) >= 0)
                return false;

            List<string> fields = new List<string>();

            for (int i = 0; i < attribute.ConstructorArguments.Count; ++i)
            {
                CustomAttributeArgument argument = attribute.ConstructorArguments[i];
                fields.Add(MDocUpdater.MakeAttributesValueString(
                        argument.Value,
                        argument.Type));
            }
            var namedArgs =
                (from namedArg in attribute.Fields
                 select new { Type = namedArg.Argument.Type, Name = namedArg.Name, Value = namedArg.Argument.Value })
                .Concat(
                        (from namedArg in attribute.Properties
                         select new { Type = namedArg.Argument.Type, Name = namedArg.Name, Value = namedArg.Argument.Value }))
                .OrderBy(v => v.Name);
            foreach (var d in namedArgs)
                fields.Add(string.Format("{0}={1}", d.Name,
                        MDocUpdater.MakeAttributesValueString(d.Value, d.Type)));

            string a2 = String.Join(", ", fields.ToArray());
            if (a2 != "") a2 = "(" + a2 + ")";

            string name = attribute.GetDeclaringType();
            if (name.EndsWith("Attribute")) name = name.Substring(0, name.Length - "Attribute".Length);
            formattedValue = prefix + name + a2;
            return true;
        }
    }
}
