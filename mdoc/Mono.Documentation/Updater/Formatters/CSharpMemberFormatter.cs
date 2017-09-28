using System.Text;

using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    public class CSharpMemberFormatter : CSharpFullMemberFormatter
    {
        protected override StringBuilder AppendNamespace (StringBuilder buf, TypeReference type)
        {
            return buf;
        }
    }
}