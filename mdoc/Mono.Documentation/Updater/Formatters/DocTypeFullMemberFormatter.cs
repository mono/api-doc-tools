using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    public class DocTypeFullMemberFormatter : MemberFormatter
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

        protected override string GetTypeNullableSymbol(TypeReference type, bool? isNullableType)
        {
            return DocUtils.GetTypeNullableSymbol(type, isNullableType);
        }
    }
}