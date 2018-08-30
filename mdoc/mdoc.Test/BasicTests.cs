using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Mono.Cecil;
using Mono.Documentation.Framework;

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
                var fullpath = Path.Combine (Path.GetDirectoryName (this.GetType ().Module.Assembly.Location), filepath);
                var resolver = new DefaultAssemblyResolver ();
                var testAssemblyPath = Path.GetDirectoryName (this.GetType ().Module.Assembly.Location);
                resolver.AddSearchDirectory (testAssemblyPath);

                ReaderParameters p = new ReaderParameters ()
                {
                    AssemblyResolver = resolver
                };


                var readModule = ModuleDefinition.ReadModule(fullpath, p);
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

        protected static XDocument ReadXDocument(string xml)
        {
            using (TextReader stringReader = new StringReader(xml))
            {
                return XDocument.Load(stringReader);
            }
        }

        protected static string NormalizeXml(string xml)
        {
            return ReadXDocument(xml).ToString();
        }
      
        protected MethodDefinition GetMethod(TypeDefinition testclass, Func<MethodDefinition, bool> query)
        {
            var methods = testclass.Methods;
            var member = methods.FirstOrDefault(query)?.Resolve();
            if (member == null)
                throw new Exception("Did not find the member in the test class");
            return member;
        }

        protected MethodDefinition GetMethod(Type type, Func<MethodDefinition, bool> query)
        {
            return GetMethod(GetType(type), query);
        }

        protected MethodDefinition GetMethod(Type type, string name)
        {
            return GetMethod(GetType(type), i => i.Name == name);
        }
    }
}