using Mono.Cecil;
using System.Text;

namespace Mono.Documentation.Updater
{
    class DocTypeFullMemberFormatter : MemberFormatter
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

        protected override StringBuilder AppendParameter(StringBuilder buf, ParameterDefinition parameterDef)
        {
            return buf.Append(GetName(parameterDef.ParameterType, useTypeProjection: false, isTypeofOperator: false));
        }
    }
}