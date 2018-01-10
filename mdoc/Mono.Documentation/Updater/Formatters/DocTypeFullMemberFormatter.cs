namespace Mono.Documentation.Updater
{
    class DocTypeFullMemberFormatter : MemberFormatter
    {
        public static readonly MemberFormatter Default = new DocTypeFullMemberFormatter ();

        protected override string NestedTypeSeparator
        {
            get { return "+"; }
        }
    }
}