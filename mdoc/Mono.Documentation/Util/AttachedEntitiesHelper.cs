using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;
using Mono.Documentation.Updater;

namespace Mono.Documentation.Util
{
    public static class AttachedEntitiesHelper
    {
        private const string PropertyConst = "Property";
        private const string EventConst = "Event";
        
        private static readonly int EventLength = EventConst.Length;
        private static readonly int PropertyLength = PropertyConst.Length;

        public static string GetEventName(string fieldDefinitionName)
        {
            return fieldDefinitionName.Substring(0, fieldDefinitionName.Length - EventLength);
        }

        public static string GetPropertyName(string fieldDefinitionName)
        {
            return fieldDefinitionName.Substring(0, fieldDefinitionName.Length - PropertyLength);
        }

        public static IEnumerable<MemberReference> GetAttachedEntities(TypeDefinition type)
        {
            var methodsLookUpTable = GetMethodsLookUpTable(type);
            foreach (var attachedEventReference in GetAttachedEvents(type, methodsLookUpTable))
            {
                yield return attachedEventReference;
            }
            foreach (var attachedEventProperty in GetAttachedProperties(type, methodsLookUpTable))
            {
                yield return attachedEventProperty;
            }
        }

        private static Dictionary<string, IEnumerable<MethodDefinition>> GetMethodsLookUpTable(TypeDefinition type)
        {
            return type.Methods.GroupBy(i => i.Name, i => i).ToDictionary(i => i.Key, i => i.AsEnumerable());
        }

        #region Attached Events
        private static IEnumerable<AttachedEventReference> GetAttachedEvents(TypeDefinition type, Dictionary<string, IEnumerable<MethodDefinition>> methods)
        {
            foreach (var field in type.Fields)
            {
                if (IsAttachedEvent(field, methods))
                    yield return new AttachedEventReference(field);
            }
        }

        private static bool IsAttachedEvent(FieldDefinition field, Dictionary<string, IEnumerable<MethodDefinition>> methods)
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/attached-events-overview
            if (!field.Name.EndsWith(EventConst))
                return false;
            var addMethodName = $"Add{GetEventName(field.Name)}Handler";
            var removeMethodName = $"Remove{GetEventName(field.Name)}Handler";
            return
                // WPF implements attached events as routed events; the identifier to use for an event (RoutedEvent) is already defined by the WPF event system
                IsAssignableTo(field.FieldType, "System.Windows.RoutedEvent")
                && field.IsPublic
                && field.IsStatic
                && field.IsInitOnly

                // Has a method Add*Handler with two parameters. 
                // Has a method Remove*Handler with two parameters. 
                && methods.ContainsKey(addMethodName)
                && methods.ContainsKey(removeMethodName)
                && methods[addMethodName].Any(IsAttachedEventMethod)
                && methods[removeMethodName].Any(IsAttachedEventMethod);
        }

        private static bool IsAttachedEventMethod(MethodDefinition method)
        {
            // The method must be public and static, with no return value.
            return method.IsPublic
                && method.IsStatic
                && method.ReturnType.FullName == Consts.VoidFullName
                && AreAttachedEventMethodParameters(method.Parameters);
        }

        private static bool AreAttachedEventMethodParameters(Collection<ParameterDefinition> parameters)
        {
            if (parameters.Count != 2)
                return false;
            return
                // The first parameter is DependencyObject
                IsAssignableTo(parameters[0].ParameterType, "System.Windows.DependencyObject")
                
                // The second parameter is the handler to add/remove
                && IsAttachedEventHandler(parameters[1].ParameterType);
        }

        private static bool IsAttachedEventHandler(TypeReference typeReference)
        {
            var type = typeReference.Resolve();
            if (!DocUtils.IsDelegate(type))
                return false;
            MethodDefinition invoke = type.GetMethod("Invoke");
            return invoke.Parameters.Count == 2;
        }
        #endregion

        #region Attached Properties
        private static IEnumerable<AttachedPropertyReference> GetAttachedProperties(TypeDefinition type, Dictionary<string, IEnumerable<MethodDefinition>> methods)
        {
            foreach (var field in type.Fields)
            {
                if (IsAttachedProperty(field, methods))
                    yield return new AttachedPropertyReference(field);
            }
        }

        private static bool IsAttachedProperty(FieldDefinition field, Dictionary<string, IEnumerable<MethodDefinition>> methods)
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/attached-properties-overview
            // https://github.com/mono/api-doc-tools/issues/63#issuecomment-328995418
            if (!field.Name.EndsWith(PropertyConst, StringComparison.Ordinal))
                return false;
            var propertyName = GetPropertyName(field.Name);
            var getMethodName = $"Get{propertyName}";
            var setMethodName = $"Set{propertyName}";

            var hasExistingProperty = field?.DeclaringType?.Properties.Any (p => p.Name.Equals (propertyName, System.StringComparison.Ordinal));
            var hasExistingField = field?.DeclaringType?.Fields.Any (f => f.Name.Equals (propertyName, System.StringComparison.Ordinal));

            return !hasExistingProperty.IsTrue () && !hasExistingField.IsTrue () &&
                // Class X has a static field of type DependencyProperty [Name]Property
                (field.FieldType.FullName == Consts.DependencyPropertyFullName || field.FieldType.FullName == Consts.DependencyPropertyFullNameXaml)
                && field.IsPublic
                && field.IsStatic
                && field.IsInitOnly

                // Class X also has static methods with the following names: Get[Name] and Set[Name]
                && ((methods.ContainsKey(getMethodName) && methods[getMethodName].Any(IsAttachedPropertyGetMethod))
                    || (methods.ContainsKey(setMethodName) && methods[setMethodName].Any(IsAttachedPropertySetMethod)));

        }

        private static bool IsAttachedPropertyGetMethod(MethodDefinition method)
        {
            return method.Parameters.Count == 1

                   // returns a value of type dp.PropertyType (or IsAssignableTo…), where dp is the value of the static field.
                   // && IsAssignableTo(method.ReturnType, "");

                   // The Get method takes one argument of type DependencyObject(or something IsAssignableTo(DependencyObject), 
                   && (IsAssignableTo(method.Parameters[0].ParameterType, Consts.DependencyObjectFullName) || IsAssignableTo(method.Parameters[0].ParameterType, Consts.DependencyObjectFullNameXaml));
        }

        private static bool IsAttachedPropertySetMethod(MethodDefinition method)
        {
            return method.Parameters.Count == 2// The Set method takes two arguments.
                   
                   // The first has type DependencyObject(or IsAssignableTo…), 
                   && (IsAssignableTo(method.Parameters[0].ParameterType, Consts.DependencyObjectFullName) || IsAssignableTo(method.Parameters[0].ParameterType, Consts.DependencyObjectFullNameXaml))

                   // the second has type dp.PropertyType (or IsAssignableTo…).
                   // && IsAssignableTo(method.Parameters[1].ParameterType, "")

                   // It returns void.
                   && method.ReturnType.FullName == Consts.VoidFullName;
        }
        #endregion
        
        private static bool IsAssignableTo(TypeReference type, string targetTypeName)
        {
            if (type == null)
                return false;
            var typeDefenition = type.Resolve();
            if (typeDefenition == null || typeDefenition.IsSealed)
                return type.FullName == targetTypeName;

            return type.FullName == targetTypeName || IsAssignableTo(typeDefenition.BaseType, targetTypeName);
        }


    }
    internal static class NBoolExtensions
    {
        public static bool IsTrue (this Nullable<bool> value) => 
            value.HasValue && value.Value;
    }
}
