using System.Text;
using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    class MsxdocSlashDocMemberFormatter : SlashDocMemberFormatter
    {
        protected override StringBuilder AppendRefTypeName(StringBuilder buf, TypeReference type, DynamicParserContext context)
        {
            TypeSpecification spec = type as TypeSpecification;
            return _AppendTypeName(buf, spec != null ? spec.ElementType : type.GetElementType(), context).Append(RefTypeModifier);
        }
    }
}
