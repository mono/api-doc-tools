using Mono.Cecil;
using Mono.Documentation.Updater;
using System;

namespace mdoc.Test
{
    public abstract class BasicFSharpFormatterTests<T> : BasicFormatterTests<T> where T : MemberFormatter
    {
        protected override TypeDefinition GetType(Type type)
        {
            var moduleName = type.Module.FullyQualifiedName;

            // Can't use base.GetType, F# assemblies use '/' instead of '+'
            var tref = GetType(moduleName, type.FullName.Replace("+", "/"));
            return tref;
        }
    }
}