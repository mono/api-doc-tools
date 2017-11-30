using System.Text;
using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    public class FSharpMemberFormatter : FSharpFullMemberFormatter
    {
        protected override StringBuilder AppendNamespace(StringBuilder buf, TypeReference type)
        {
            return buf;
        }
    }
}