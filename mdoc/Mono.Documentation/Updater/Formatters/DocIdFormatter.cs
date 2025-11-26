
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Documentation.Util;

namespace Mono.Documentation.Updater
{
    class DocIdFormatter : MemberFormatter
    {
        public override string Language => Consts.DocId;

        private SlashDocMemberFormatter slashDocMemberFormatter;

        public DocIdFormatter(TypeMap map) : base(map)
        {
            slashDocMemberFormatter = new SlashDocMemberFormatter(map);
        }

        public override string GetDeclaration(TypeReference tref)
        {
            return DocCommentId.GetDocCommentId(tref.Resolve());
        }
        public override string GetDeclaration(MemberReference mreference)
        {
            string ret = mreference is AttachedEventReference || mreference is AttachedPropertyReference
                ? slashDocMemberFormatter.GetDeclaration(mreference)
                : DocCommentId.GetDocCommentId(mreference.Resolve());
            return ret.Replace("|System.Runtime.InteropServices.InAttribute", string.Empty);
        }
    }
}