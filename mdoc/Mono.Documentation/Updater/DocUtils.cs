using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using Mono.Cecil;

using Mono.Documentation.Util;

namespace Mono.Documentation.Updater
{
    static class DocUtils
    {

        public static bool DoesNotHaveApiStyle (this XmlElement element, ApiStyle style)
        {
            string styleString = style.ToString ().ToLowerInvariant ();
            string apistylevalue = element.GetAttribute ("apistyle");
            return apistylevalue != styleString || string.IsNullOrWhiteSpace (apistylevalue);
        }
        public static bool HasApiStyle (this XmlElement element, ApiStyle style)
        {
            string styleString = style.ToString ().ToLowerInvariant ();
            return element.GetAttribute ("apistyle") == styleString;
        }
        public static bool HasApiStyle (this XmlNode node, ApiStyle style)
        {
            var attribute = node.Attributes["apistyle"];
            return attribute != null && attribute.Value == style.ToString ().ToLowerInvariant ();
        }
        public static void AddApiStyle (this XmlElement element, ApiStyle style)
        {
            string styleString = style.ToString ().ToLowerInvariant ();
            var existingValue = element.GetAttribute ("apistyle");
            if (string.IsNullOrWhiteSpace (existingValue) || existingValue != styleString)
            {
                element.SetAttribute ("apistyle", styleString);
            }

            // Propagate the API style up to the membernode if necessary
            if (element.LocalName == "AssemblyInfo" && element.ParentNode != null && element.ParentNode.LocalName == "Member")
            {
                var member = element.ParentNode;
                var unifiedAssemblyNode = member.SelectSingleNode ("AssemblyInfo[@apistyle='unified']");
                var classicAssemblyNode = member.SelectSingleNode ("AssemblyInfo[not(@apistyle) or @apistyle='classic']");

                var parentAttribute = element.ParentNode.Attributes["apistyle"];
                Action removeStyle = () => element.ParentNode.Attributes.Remove (parentAttribute);
                Action propagateStyle = () =>
                {
                    if (parentAttribute == null)
                    {
                        // if it doesn't have the attribute, then add it
                        parentAttribute = element.OwnerDocument.CreateAttribute ("apistyle");
                        parentAttribute.Value = styleString;
                        element.ParentNode.Attributes.Append (parentAttribute);
                    }
                };

                if ((style == ApiStyle.Classic && unifiedAssemblyNode != null) || (style == ApiStyle.Unified && classicAssemblyNode != null))
                    removeStyle ();
                else
                    propagateStyle ();
            }
        }
        public static void AddApiStyle (this XmlNode node, ApiStyle style)
        {
            string styleString = style.ToString ().ToLowerInvariant ();
            var existingAttribute = node.Attributes["apistyle"];
            if (existingAttribute == null)
            {
                existingAttribute = node.OwnerDocument.CreateAttribute ("apistyle");
                node.Attributes.Append (existingAttribute);
            }
            existingAttribute.Value = styleString;
        }
        public static void RemoveApiStyle (this XmlElement element, ApiStyle style)
        {
            string styleString = style.ToString ().ToLowerInvariant ();
            string existingValue = element.GetAttribute ("apistyle");
            if (string.IsNullOrWhiteSpace (existingValue) || existingValue == styleString)
            {
                element.RemoveAttribute ("apistyle");
            }
        }
        public static void RemoveApiStyle (this XmlNode node, ApiStyle style)
        {
            var styleAttribute = node.Attributes["apistyle"];
            if (styleAttribute != null && styleAttribute.Value == style.ToString ().ToLowerInvariant ())
            {
                node.Attributes.Remove (styleAttribute);
            }
        }

        public static bool IsExplicitlyImplemented (MethodDefinition method)
        {
            return method != null && method.IsPrivate && method.IsFinal && method.IsVirtual;
        }

        public static string GetTypeDotMember (string name)
        {
            int startType, startMethod;
            startType = startMethod = -1;
            for (int i = 0; i < name.Length; ++i)
            {
                if (name[i] == '.')
                {
                    startType = startMethod;
                    startMethod = i;
                }
            }
            return name.Substring (startType + 1);
        }

        public static string GetMember (string name)
        {
            int i = name.LastIndexOf ('.');
            if (i == -1)
                return name;
            return name.Substring (i + 1);
        }

        public static void GetInfoForExplicitlyImplementedMethod (
                MethodDefinition method, out TypeReference iface, out MethodReference ifaceMethod)
        {
            iface = null;
            ifaceMethod = null;
            if (method.Overrides.Count != 1)
                throw new InvalidOperationException ("Could not determine interface type for explicitly-implemented interface member " + method.Name);
            iface = method.Overrides[0].DeclaringType;
            ifaceMethod = method.Overrides[0];
        }

        public static string GetPropertyName (PropertyDefinition pi)
        {
            // Issue: (g)mcs-generated assemblies that explicitly implement
            // properties don't specify the full namespace, just the 
            // TypeName.Property; .NET uses Full.Namespace.TypeName.Property.
            MethodDefinition method = pi.GetMethod;
            if (method == null)
                method = pi.SetMethod;
            if (!IsExplicitlyImplemented (method))
                return pi.Name;

            // Need to determine appropriate namespace for this member.
            TypeReference iface;
            MethodReference ifaceMethod;
            GetInfoForExplicitlyImplementedMethod (method, out iface, out ifaceMethod);
            return string.Join (".", new string[]{
                DocTypeFullMemberFormatter.Default.GetName (iface),
                GetMember (pi.Name)});
        }

        public static string GetNamespace (TypeReference type)
        {
            if (type.GetElementType ().IsNested)
                type = type.GetElementType ();
            while (type != null && type.IsNested)
                type = type.DeclaringType;
            if (type == null)
                return string.Empty;

            string typeNS = type.Namespace;

            // first, make sure this isn't a type reference to another assembly/module

            bool isInAssembly = MDocUpdater.IsInAssemblies (type.Module.Name);
            if (isInAssembly && !typeNS.StartsWith ("System") && MDocUpdater.HasDroppedNamespace (type))
            {
                typeNS = string.Format ("{0}.{1}", MDocUpdater.droppedNamespace, typeNS);
            }
            return typeNS;
        }

        public static string PathCombine (string dir, string path)
        {
            if (dir == null)
                dir = "";
            if (path == null)
                path = "";
            return Path.Combine (dir, path);
        }

        public static bool IsExtensionMethod (MethodDefinition method)
        {
            return
                method.CustomAttributes
                        .Any (m => m.AttributeType.FullName == "System.Runtime.CompilerServices.ExtensionAttribute")
                && method.DeclaringType.CustomAttributes
                        .Any (m => m.AttributeType.FullName == "System.Runtime.CompilerServices.ExtensionAttribute");
        }

        public static bool IsDelegate (TypeDefinition type)
        {
            TypeReference baseRef = type.BaseType;
            if (baseRef == null)
                return false;
            return !type.IsAbstract && baseRef.FullName == "System.Delegate" || // FIXME
                    baseRef.FullName == "System.MulticastDelegate";
        }

        public static List<TypeReference> GetDeclaringTypes (TypeReference type)
        {
            List<TypeReference> decls = new List<TypeReference> ();
            decls.Add (type);
            while (type.DeclaringType != null)
            {
                decls.Add (type.DeclaringType);
                type = type.DeclaringType;
            }
            decls.Reverse ();
            return decls;
        }

        public static int GetGenericArgumentCount (TypeReference type)
        {
            GenericInstanceType inst = type as GenericInstanceType;
            return inst != null
                    ? inst.GenericArguments.Count
                    : type.GenericParameters.Count;
        }

        public static IEnumerable<TypeReference> GetUserImplementedInterfaces (TypeDefinition type)
        {
            HashSet<string> inheritedInterfaces = GetInheritedInterfaces (type);
            List<TypeReference> userInterfaces = new List<TypeReference> ();
            foreach (var ii in type.Interfaces)
            {
                var iface = ii.InterfaceType;
                TypeReference lookup = iface.Resolve () ?? iface;
                if (!inheritedInterfaces.Contains (GetQualifiedTypeName (lookup)))
                    userInterfaces.Add (iface);
            }
            return userInterfaces.Where (i => MDocUpdater.IsPublic (i.Resolve ()));
        }

        private static string GetQualifiedTypeName (TypeReference type)
        {
            return "[" + type.Scope.Name + "]" + type.FullName;
        }

        private static HashSet<string> GetInheritedInterfaces (TypeDefinition type)
        {
            HashSet<string> inheritedInterfaces = new HashSet<string> ();
            Action<TypeDefinition> a = null;
            a = t =>
            {
                if (t == null) return;
                foreach (var r in t.Interfaces)
                {
                    inheritedInterfaces.Add (GetQualifiedTypeName (r.InterfaceType));
                    a (r.InterfaceType.Resolve ());
                }
            };
            TypeReference baseRef = type.BaseType;
            while (baseRef != null)
            {
                TypeDefinition baseDef = baseRef.Resolve ();
                if (baseDef != null)
                {
                    a (baseDef);
                    baseRef = baseDef.BaseType;
                }
                else
                    baseRef = null;
            }
            foreach (var r in type.Interfaces)
                a (r.InterfaceType.Resolve ());
            return inheritedInterfaces;
        }
    }
}