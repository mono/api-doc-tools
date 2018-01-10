using System.Text;
using Mono.Cecil;

namespace Mono.Documentation.Updater.Formatters.CppFormatters
{
    public class CppCxMemberFormatter : CppCxFullMemberFormatter
    {
        protected override StringBuilder AppendNamespace (StringBuilder buf, TypeReference type)
        {
            return buf;
        }
    }
}