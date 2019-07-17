using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Documentation;
using Mono.Documentation.Updater.Frameworks;

namespace mdoc.Test
{
    public abstract class CecilBaseTest
    {

        protected TypeDefinition GetTypeDef<T> ()
        {
            var type = typeof(T);
            var path = type.Module.Assembly.Location;
            var assemblyResolver = new MDocResolver();
            var dependencyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "..", "..", "..", "..", "external", "Windows");
            assemblyResolver.AddSearchDirectory(dependencyPath);
            var cachedResolver = new CachedResolver(assemblyResolver);
            if (!System.IO.Directory.Exists(dependencyPath))
                throw new System.Exception($"The path '{dependencyPath}' doesn't seem to exist ... did project files get moved around?");

            var resolver = new MDocMetadataResolver(cachedResolver);
            var assembly = AssemblyDefinition.ReadAssembly(path, new ReaderParameters { AssemblyResolver = cachedResolver, MetadataResolver = resolver });
            
            var typeref = assembly.MainModule.GetAllTypes ().FirstOrDefault (t => t.Name == type.Name);
            return typeref;
        }
    }
}
