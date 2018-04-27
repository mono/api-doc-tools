using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace mdoc.Test
{
    public class BasicTests
    {
        protected Dictionary<string, ModuleDefinition> moduleCash = new Dictionary<string, ModuleDefinition>();
        protected Dictionary<string, TypeDefinition> typesCash = new Dictionary<string, TypeDefinition>();

        protected TypeDefinition GetType(string filepath, string classname)
        {
            if (typesCash.ContainsKey(classname))
                return typesCash[classname];


            if (!moduleCash.ContainsKey(filepath))
            {
                var readModule = ModuleDefinition.ReadModule(filepath);
                moduleCash.Add(filepath, readModule);
            }

            var module = moduleCash[filepath];
            var types = module.GetTypes();
            var testclass = types
                .SingleOrDefault(t => t.FullName == classname);
            if (testclass == null)
            {
                throw new Exception($"Test was unable to find type {classname}");
            }

            var typeDef = testclass.Resolve();
            typesCash.Add(classname, typeDef);
            return typeDef;
        }

        protected virtual TypeDefinition GetType(Type type)
        {
            var moduleName = type.Module.FullyQualifiedName;
            return GetType(moduleName, type.FullName);
        }
    }
}