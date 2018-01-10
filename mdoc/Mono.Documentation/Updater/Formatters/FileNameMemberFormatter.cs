using System.Text;

using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    class FileNameMemberFormatter : SlashDocMemberFormatter
    {
        protected override StringBuilder AppendNamespace (StringBuilder buf, TypeReference type)
        {
            return buf;
        }

        protected override string NestedTypeSeparator
        {
            get { return "+"; }
        }
    }
}