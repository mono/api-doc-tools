using Mono.Cecil;
using System.IO;

namespace mdoc.Test
{
    public class ExternalAssemblyResolver : DefaultAssemblyResolver
    {
        private const string ExternalTestAssemblyDirectory = "../../../../external/Test";

        public ExternalAssemblyResolver()
        {
            AddExternalSearchDirectory();
        }

        private void AddExternalSearchDirectory()
        {
            AddSearchDirectory(GetExternalAssemblyPath(ExternalTestAssemblyDirectory));
        }

        private string GetExternalAssemblyPath(string externalTestAssemblyDirectory)
        {
            return Path.Combine(Path.GetDirectoryName(this.GetType().Module.Assembly.Location), externalTestAssemblyDirectory);
        }
    }
}
