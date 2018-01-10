using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Documentation.Updater;
using NUnit.Framework;

namespace mdoc.Test
{
    public abstract class BasicFormatterTests<T> where T : MemberFormatter
    {
        protected Dictionary<string, ModuleDefinition> moduleCash = new Dictionary<string, ModuleDefinition>();
        protected Dictionary<string, TypeDefinition> typesCash = new Dictionary<string, TypeDefinition>();

        protected abstract T formatter { get; }

        protected MethodDefinition GetMethod(Type type, Func<MethodDefinition, bool> query)
        {
            return GetMethod(GetType(type), query);
        }

        protected MethodDefinition GetMethod(TypeDefinition testclass, Func<MethodDefinition, bool> query)
        {
            var methods = testclass.Methods;
            var member = methods.FirstOrDefault(query)?.Resolve();
            if (member == null)
                throw new Exception("Did not find the member in the test class");
            return member;
        }

        protected MethodDefinition GetProperty(TypeDefinition testclass, Func<MethodDefinition, bool> query)
        {
            var methods = testclass.Methods;
            var member = methods.FirstOrDefault(query)?.Resolve();
            if (member == null)
                throw new Exception("Did not find the member in the test class");
            return member;
        }

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

        protected MemberReference GetProperty(TypeDefinition type, string memberName)
        {
            var property = type.Properties.FirstOrDefault(i => i.Name == memberName);
            if (property == null)
                throw new Exception($"Can't find property {memberName}");
            return property;
        }

        protected MemberReference GetField(TypeDefinition type, string eventName)
        {
            var property = type.Fields.SingleOrDefault(i => i.Name == eventName);
            if (property == null)
                throw new Exception($"Can't find field {eventName}");
            return property;
        }

        protected MemberReference GetEvent(TypeDefinition type, string eventName)
        {
            var property = type.Events.SingleOrDefault(i => i.Name == eventName);
            if (property == null)
                throw new Exception($"Can't find field {eventName}");
            return property;
        }

        protected void TestTypeSignature(Type type, string expected)
        {
            expected = FormatEndings(expected);
            var signature = GetTypeSignature(type);
            Assert.AreEqual(expected, signature);
        }

        protected void TestTypeSignature(string libPath, string fullTypeName, string expected)
        {
            expected = FormatEndings(expected);
            var type = GetType(libPath, fullTypeName);
            var signature = formatter.GetDeclaration(type);
            Assert.AreEqual(expected, signature);
        }

        private string GetTypeSignature(Type type)
        {
            var tref = GetType(type);
            var signature = formatter.GetDeclaration(tref);
            return signature;
        }

        protected void TestMethodSignature(Type type, string expected, string memberName)
        {
            var signature = GetMethodSignature(type, memberName);
            Assert.AreEqual(FormatEndings(expected), signature);
        }

        protected void TestMethodSignature(string libPath, string fullTypeName, string memberName, string expected)
        {
            var type = GetType(libPath, fullTypeName);
            var method = GetMethod(type, i => i.Name == memberName);
            var signature = formatter.GetDeclaration(method);
            Assert.AreEqual(FormatEndings(expected), signature);
        }
        
        private string GetMethodSignature(Type type, string memberName)
        {
            var tref = GetType(type);
            var method = GetMethod(tref, i => i.Name == memberName);
            var signature = formatter.GetDeclaration(method);
            return signature;
        }

        protected void TestPropertySignature(Type type, string expected, string memberName)
        {
            var signature = GetPropertySignature(type, memberName);
            Assert.AreEqual(FormatEndings(expected), signature);
        }

        private string GetPropertySignature(Type type, string memberName)
        {
            var tref = GetType(type);
            return formatter.GetDeclaration(GetProperty(tref, memberName));
        }

        protected void TestPropertySignature(string libPath, string fullTypeName, string memberName, string expected)
        {
            var type = GetType(libPath, fullTypeName);
            var property = GetProperty(type, memberName);
            var signature = formatter.GetDeclaration(property);
            Assert.AreEqual(FormatEndings(expected), signature);
        }

        protected void TestEventSignature(Type type, string expected, string memberName)
        {
            var signature = GetEventSignature(type, memberName);
            Assert.AreEqual(expected, signature);
        }

        private string GetEventSignature(Type type, string memberName)
        {
            var tref = GetType(type);
            return formatter.GetDeclaration(GetEvent(tref, memberName));
        }

        protected void TestEventSignature(string libPath, string fullTypeName, string memberName, string expected)
        {
            var type = GetType(libPath, fullTypeName);
            var @event = GetEvent(type, memberName);
            var signature = formatter.GetDeclaration(@event);
            Assert.AreEqual(FormatEndings(expected), signature);
        }

        protected void TestFieldSignature(Type type, string expected, string memberName)
        {
            var usage = GetFieldUsage(type, memberName);
            Assert.AreEqual(FormatEndings(expected), usage);
        }

        private string GetFieldUsage(Type type, string memberName)
        {
            var tref = GetType(type);
            var field = GetField(tref, memberName);
            var signature = formatter.GetDeclaration(field);
            return signature;
        }

        protected void TestFieldSignature(string libPath, string fullTypeName, string memberName, string expected)
        {
            var type = GetType(libPath, fullTypeName);
            var field = GetField(type, memberName);
            var signature = formatter.GetDeclaration(field);
            Assert.AreEqual(FormatEndings(expected), signature);
        }

        protected static string FormatEndings(string s)
        {
            return s?.Replace("\r\n", MemberFormatter.GetLineEnding());
        }
    }

}
