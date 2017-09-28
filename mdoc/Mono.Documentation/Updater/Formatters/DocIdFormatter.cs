
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Mono.Documentation.Updater
{
    class DocIdFormatter : MemberFormatter
    {
        public override string Language
        {
            get { return "DocId"; }
        }

        public override string GetDeclaration (TypeReference tref)
        {
            return DocCommentId.GetDocCommentId (tref.Resolve ());
        }
        public override string GetDeclaration (MemberReference mreference)
        {
            return DocCommentId.GetDocCommentId (mreference.Resolve ());
        }
    }
}