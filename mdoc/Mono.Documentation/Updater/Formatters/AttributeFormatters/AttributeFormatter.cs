using Mono.Cecil;
using Mono.Documentation.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mono.Documentation.Updater.Formatters
{
    public class AttributeFormatter
    {
        public IEnumerable<string> GetCustomAttributes(MemberReference mi)
        {
            IEnumerable<string> attrs = Enumerable.Empty<string>();

            ICustomAttributeProvider p = mi as ICustomAttributeProvider;
            if (p != null)
                attrs = attrs.Concat(GetCustomAttributes(p.CustomAttributes, ""));

            TypeDefinition typeDefinition = mi as TypeDefinition;
            if (typeDefinition != null && typeDefinition.IsSerializable)
            {
                attrs = attrs.Concat(new[] { "System.Serializable" });
            }

            PropertyDefinition pd = mi as PropertyDefinition;
            if (pd != null)
            {
                if (pd.GetMethod != null)
                    attrs = attrs.Concat(GetCustomAttributes(pd.GetMethod.CustomAttributes, "get: "));
                if (pd.SetMethod != null)
                    attrs = attrs.Concat(GetCustomAttributes(pd.SetMethod.CustomAttributes, "set: "));
            }

            EventDefinition ed = mi as EventDefinition;
            if (ed != null)
            {
                if (ed.AddMethod != null)
                    attrs = attrs.Concat(GetCustomAttributes(ed.AddMethod.CustomAttributes, "add: "));
                if (ed.RemoveMethod != null)
                    attrs = attrs.Concat(GetCustomAttributes(ed.RemoveMethod.CustomAttributes, "remove: "));
            }

            return attrs;
        }

        public IEnumerable<string> GetCustomAttributes(IList<CustomAttribute> attributes, string prefix)
        {
            foreach (CustomAttribute attribute in attributes.OrderBy(ca => ca.AttributeType.FullName).Where(i => !IsIgnoredAttribute(i)))
            {
                TypeDefinition attrType = attribute.AttributeType as TypeDefinition;
                if (attrType != null && !DocUtils.IsPublic(attrType))
                    continue;
                if (FormatterManager.SlashdocFormatter.GetName(attribute.AttributeType) == null)
                    continue;

                if (Array.IndexOf(IgnorableAttributes, attribute.AttributeType.FullName) >= 0)
                    continue;

                var fields = new List<string>();

                for (int i = 0; i < attribute.ConstructorArguments.Count; ++i)
                {
                    CustomAttributeArgument argument = attribute.ConstructorArguments[i];
                    fields.Add(MakeAttributesValueString(
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
                            MakeAttributesValueString(d.Value, d.Type)));

                string a2 = String.Join(", ", fields.ToArray());
                if (a2 != "") a2 = "(" + a2 + ")";

                string name = attribute.GetDeclaringType();
                if (name.EndsWith("Attribute")) name = name.Substring(0, name.Length - "Attribute".Length);
                yield return prefix + name + a2;
            }
        }

        public static string MakeAttributesValueString(object v, TypeReference valueType)
        {
            var formatters = new[] {
                new AttributeValueFormatter (),
                new ApplePlatformEnumFormatter (),
                new StandardFlagsEnumFormatter (),
                new DefaultAttributeValueFormatter (),
            };

            ResolvedTypeInfo type = new ResolvedTypeInfo(valueType);

            if (valueType is ArrayType && v is CustomAttributeArgument[])
            {
                ArrayType atype = valueType as ArrayType;
                CustomAttributeArgument[] args = v as CustomAttributeArgument[];
                var returnvalue = $"new {atype.FullName}{(atype.FullName.EndsWith("[]") ? "" : "[]")} {{ { string.Join(", ", args.Select(a => MakeAttributesValueString(a.Value, a.Type)).ToArray()) } }}";
                return returnvalue;
            }

            foreach (var formatter in formatters)
            {
                string formattedValue;
                if (formatter.TryFormatValue(v, type, out formattedValue))
                {
                    return formattedValue;
                }
            }

            // this should never occur because the DefaultAttributeValueFormatter will always
            // successfully format the value ... but this is needed to satisfy the compiler :)
            throw new InvalidDataException(string.Format("Unable to format attribute value ({0})", v.ToString()));
        }

        private bool IsIgnoredAttribute(CustomAttribute customAttribute)
        {
            // An Obsolete attribute with a known string is added to all ref-like structs
            // https://github.com/dotnet/csharplang/blob/master/proposals/csharp-7.2/span-safety.md#metadata-representation-or-ref-like-structs
            return customAttribute.AttributeType.FullName == typeof(ObsoleteAttribute).FullName
                && customAttribute.HasConstructorArguments
                && customAttribute.ConstructorArguments.First().Value.ToString() == Consts.RefTypeObsoleteString;
        }

        // FIXME: get TypeReferences instead of string comparison?
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
    }
}
