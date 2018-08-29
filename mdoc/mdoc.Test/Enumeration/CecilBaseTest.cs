using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace mdoc.Test
{
    public abstract class CecilBaseTest
    {
        ModuleDefinition module;

        protected TypeDefinition GetTypeDef<T> ()
        {
            var type = typeof (T);
            if (module == null)
                module = ModuleDefinition.ReadModule (type.Module.FullyQualifiedName);

            var typeref = module.GetAllTypes ().Single (t => t.Name == type.Name);
            return typeref;
        }
    }
}
