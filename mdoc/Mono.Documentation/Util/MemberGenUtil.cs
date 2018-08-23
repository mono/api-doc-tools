using Mono.Cecil;
using Mono.Documentation;
using Mono.Documentation.Util;
using System.Linq;

namespace mdoc.Mono.Documentation.Util
{
    public static class MemberGenUtil
    {
        public static bool IsUserDefined(this MethodDefinition method)
            => method.CustomAttributes.Any(a => a.GetDeclaringType() != Consts.CompilerGeneratedAttribute ||
                                                a.GetDeclaringType() != Consts.CompilationMappingAttribute);

        public static bool IsUserDefined(this PropertyDefinition prop)
            => prop.CustomAttributes.Any(a => a.GetDeclaringType() != Consts.CompilerGeneratedAttribute ||
                                              a.GetDeclaringType() != Consts.CompilationMappingAttribute);
    }
}
