using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mono.Documentation.Updater
{
    public class DocTypeFullMemberFormatter : MemberFormatter
    {
        private static MemberFormatter defaultFormatter;
        public static MemberFormatter Default
        {
            get
            {
                if (defaultFormatter == null)
                    defaultFormatter = new DocTypeFullMemberFormatter(MDocUpdater.Instance.TypeMap);

                return defaultFormatter;
            }
        }

        public DocTypeFullMemberFormatter(TypeMap map) : base(map) { }

        protected override string NestedTypeSeparator
        {
            get { return "+"; }
        }

        protected override string GetTypeNullableSymbol(TypeReference type, bool? isNullableType)
        {
            return DocUtils.GetTypeNullableSymbol(type, isNullableType);
        }

        protected override string GetTypeName(TypeReference type, IAttributeParserContext context, bool appendGeneric = true, bool useTypeProjection = true, bool isTypeofOperator = false)
        {
            GenericInstanceType genType = type as GenericInstanceType;
            if (IsSpecialGenericNullableValueType(genType))
            {
                return AppendSpecialGenericNullableValueTypeName(new StringBuilder(), genType, context, appendGeneric, useTypeProjection).ToString();
            }

            return base.GetTypeName(type, context, appendGeneric, useTypeProjection, isTypeofOperator);
        }

        protected override bool IsSpecialGenericNullableValueType(GenericInstanceType genInst)
        {
            return genInst != null && genInst.HasGenericArguments && (genInst.Name.StartsWith("ValueTuple`") || genInst.Name.StartsWith("Nullable`"));
        }

        protected override StringBuilder AppendSpecialGenericNullableValueTypeName(StringBuilder buf, GenericInstanceType genInst, IAttributeParserContext context, bool appendGeneric = true, bool useTypeProjection = true)
        {
            if (genInst.Name.StartsWith("Nullable`") && genInst.HasGenericArguments)
            {
                var underlyingTypeName = GetTypeName(genInst.GenericArguments.First(), context, appendGeneric, useTypeProjection);
                buf.Append($"System.Nullable<{underlyingTypeName}>");

                return buf;
            }

            if (genInst.Name.StartsWith("ValueTuple`"))
            {
                buf.Append("System.ValueTuple<");
                var genArgList = new List<string>();
                foreach (var item in genInst.GenericArguments)
                {
                    var isNullableType = false;
                    if (!item.IsValueType)
                    {
                        isNullableType = context.IsNullable();
                    }

                    var underlyingTypeName = GetTypeName(item, context, appendGeneric, useTypeProjection) + GetTypeNullableSymbol(item, isNullableType);
                    genArgList.Add(underlyingTypeName);
                }
                buf.Append(string.Join(",", genArgList));
                buf.Append(">");

                return buf;
            }

            return buf;
        }
    }
}