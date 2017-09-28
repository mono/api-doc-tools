
using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    class ResolvedTypeInfo
    {
        TypeDefinition typeDef;

        public ResolvedTypeInfo (TypeReference value)
        {
            Reference = value;
        }

        public TypeReference Reference { get; private set; }

        public TypeDefinition Definition
        {
            get
            {
                if (typeDef == null)
                {
                    typeDef = Reference.Resolve ();
                }
                return typeDef;
            }
        }
    }
}