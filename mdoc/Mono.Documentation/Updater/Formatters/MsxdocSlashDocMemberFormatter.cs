using System.Text;
using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    class MsxdocSlashDocMemberFormatter : SlashDocMemberFormatter
    {
        public MsxdocSlashDocMemberFormatter(TypeMap map) : base(map) { }

        protected override StringBuilder AppendRefTypeName(StringBuilder buf, ByReferenceType type, IAttributeParserContext context)
        {
            return _AppendTypeName(buf, type.ElementType, context).Append(RefTypeModifier);
        }
    }
}
