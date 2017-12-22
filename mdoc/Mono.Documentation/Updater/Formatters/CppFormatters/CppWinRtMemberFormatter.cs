using System.Text;
using Mono.Cecil;
using Mono.Documentation.Updater.CppFormatters;

namespace Mono.Documentation.Updater.Formatters.CppFormatters
{
   public class CppWinRtMemberFormatter : CppWinRtFullMemberFormatter
    {
        protected override StringBuilder AppendNamespace(StringBuilder buf, TypeReference type)
        {
            return buf;
        }
    }
}
