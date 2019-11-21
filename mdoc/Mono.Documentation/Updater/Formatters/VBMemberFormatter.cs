using System.Text;
using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    public class VBMemberFormatter : VBFullMemberFormatter
    {
        public VBMemberFormatter() : this(null) {}
        public VBMemberFormatter(TypeMap map) : base(map) { }

        protected override StringBuilder AppendNamespace(StringBuilder buf, TypeReference type)
        {
            return buf;
        }
    }
}