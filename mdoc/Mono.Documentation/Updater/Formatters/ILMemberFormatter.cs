using System.Text;

using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    public class ILMemberFormatter : ILFullMemberFormatter
    {
        public ILMemberFormatter() : this(null) {}
        public ILMemberFormatter(TypeMap map) : base(map) { }

        protected override StringBuilder AppendNamespace (StringBuilder buf, TypeReference type)
        {
            return buf;
        }
    }
}