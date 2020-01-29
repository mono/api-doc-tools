namespace Mono.Documentation.Updater
{
    class ILNativeTypeMemberFormatter : ILFullMemberFormatter
    {
        public ILNativeTypeMemberFormatter(TypeMap map) : base(map) { }

        protected static string _GetBuiltinType (string t)
        {
            return null;
        }
    }
}