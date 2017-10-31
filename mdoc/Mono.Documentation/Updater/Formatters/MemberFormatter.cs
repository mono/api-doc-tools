using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mono.Cecil;

using Mono.Documentation.Util;

namespace Mono.Documentation.Updater
{
    public abstract class MemberFormatter
    {

        public virtual string Language
        {
            get { return ""; }
        }

        public string GetName (MemberReference member)
        {
            return GetName (member, null);
        }

        public virtual string GetName (MemberReference member, DynamicParserContext context)
        {
            TypeReference type = member as TypeReference;
            if (type != null)
                return GetTypeName (type, context);
            MethodReference method = member as MethodReference;
            if (method != null && method.Name == ".ctor") // method.IsConstructor
                return GetConstructorName (method);
            if (method != null)
                return GetMethodName (method);
            PropertyReference prop = member as PropertyReference;
            if (prop != null)
                return GetPropertyName (prop);
            FieldReference field = member as FieldReference;
            if (field != null)
                return GetFieldName (field);
            EventReference e = member as EventReference;
            if (e != null)
                return GetEventName (e);
            throw new NotSupportedException ("Can't handle: " +
                        (member == null ? "null" : member.GetType ().ToString ()));
        }

        protected virtual string GetTypeName (TypeReference type)
        {
            return GetTypeName (type, null);
        }

        protected virtual string GetTypeName (TypeReference type, DynamicParserContext context)
        {
            if (type == null)
                throw new ArgumentNullException (nameof (type));

            var typeName = _AppendTypeName (new StringBuilder (type.Name.Length), type, context).ToString ();

            typeName = RemoveMod (typeName);

            return typeName;
        }

        public static string RemoveMod (string typeName)
        {
            if (string.IsNullOrWhiteSpace (typeName)) return typeName;

            // For custom modifiers (modopt/modreq), we are just excising them
            // via string manipulation for simplicity; since these cannot be
            // expressed in C#. If finer control is needed in the future, you can
            // use IModifierType, PointerType, ByReferenceType, etc.

            int modIndex = Math.Max (typeName.LastIndexOf ("modopt(", StringComparison.Ordinal), typeName.LastIndexOf ("modreq(", StringComparison.Ordinal));
            if (modIndex > 0)
            {
                var tname = typeName.Substring (0, modIndex - 1);
                var parenIndex = typeName.LastIndexOf (')');
                if (parenIndex == typeName.Length - 2)
                { // see if there's metadata like a pointer
                    tname += typeName.Last ();
                }
                typeName = tname;
            }

            modIndex = Math.Max (typeName.LastIndexOf ("modopt(", StringComparison.Ordinal), typeName.LastIndexOf ("modreq(", StringComparison.Ordinal));
            if (modIndex >= 0)
                return RemoveMod (typeName);
            else
                return typeName;
        }

        protected virtual char[] ArrayDelimeters
        {
            get { return new char[] { '[', ']' }; }
        }

        protected virtual MemberFormatterState MemberFormatterState { get; set; }

        protected StringBuilder _AppendTypeName (StringBuilder buf, TypeReference type, DynamicParserContext context)
        {
            if (type is ArrayType)
            {
                TypeSpecification spec = type as TypeSpecification;
                _AppendTypeName (buf, spec != null ? spec.ElementType : type.GetElementType (), context);
                return AppendArrayModifiers (buf, (ArrayType)type);
            }
            if (type is ByReferenceType)
            {
                return AppendRefTypeName (buf, type, context);
            }
            if (type is PointerType)
            {
                return AppendPointerTypeName (buf, type, context);
            }
            if (type is GenericParameter)
            {
                return AppendTypeName (buf, type, context);
            }
            AppendNamespace (buf, type);
            GenericInstanceType genInst = type as GenericInstanceType;
            if (type.GenericParameters.Count == 0 &&
                    (genInst == null ? true : genInst.GenericArguments.Count == 0))
            {
                return AppendFullTypeName (buf, type, context);
            }
            return AppendGenericType (buf, type, context);
        }

        protected virtual StringBuilder AppendNamespace (StringBuilder buf, TypeReference type)
        {
            string ns = DocUtils.GetNamespace (type);
            if (ns != null && ns.Length > 0)
                buf.Append (ns).Append ('.');
            return buf;
        }

        protected virtual StringBuilder AppendFullTypeName (StringBuilder buf, TypeReference type, DynamicParserContext context)
        {
            if (type.DeclaringType != null)
                AppendFullTypeName (buf, type.DeclaringType, context).Append (NestedTypeSeparator);
            return AppendTypeName (buf, type, context);
        }

        protected virtual StringBuilder AppendTypeName (StringBuilder buf, TypeReference type, DynamicParserContext context)
        {
            if (context != null)
                context.TransformIndex++;
            return AppendTypeName (buf, type.Name);
        }

        protected virtual StringBuilder AppendTypeName (StringBuilder buf, string typename)
        {
            int n = typename.IndexOf ("`");
            if (n >= 0)
                return buf.Append (typename.Substring (0, n));
            return buf.Append (typename);
        }

        protected virtual StringBuilder AppendArrayModifiers (StringBuilder buf, ArrayType array)
        {
            buf.Append (ArrayDelimeters[0]);
            int rank = array.Rank;
            if (rank > 1)
                buf.Append (new string (',', rank - 1));
            return buf.Append (ArrayDelimeters[1]);
        }

        protected virtual string RefTypeModifier
        {
            get { return "@"; }
        }

        protected virtual StringBuilder AppendRefTypeName (StringBuilder buf, TypeReference type, DynamicParserContext context)
        {
            TypeSpecification spec = type as TypeSpecification;
            return _AppendTypeName (buf, spec != null ? spec.ElementType : type.GetElementType (), context)
                    .Append (RefTypeModifier);
        }

        protected virtual string PointerModifier
        {
            get { return "*"; }
        }

        protected virtual StringBuilder AppendPointerTypeName (StringBuilder buf, TypeReference type, DynamicParserContext context)
        {
            TypeSpecification spec = type as TypeSpecification;
            return _AppendTypeName (buf, spec != null ? spec.ElementType : type.GetElementType (), context)
                    .Append (PointerModifier);
        }

        protected virtual string[] GenericTypeContainer
        {
            get { return new string[] { "<", ">" }; }
        }

        protected virtual char NestedTypeSeparator
        {
            get { return '.'; }
        }

        protected virtual StringBuilder AppendGenericType (StringBuilder buf, TypeReference type, DynamicParserContext context)
        {
            List<TypeReference> decls = DocUtils.GetDeclaringTypes (
                    type is GenericInstanceType ? type.GetElementType () : type);
            List<TypeReference> genArgs = GetGenericArguments (type);
            int argIdx = 0;
            int prev = 0;
            bool insertNested = false;
            foreach (var decl in decls)
            {
                TypeReference declDef = decl.Resolve () ?? decl;
                if (insertNested)
                {
                    buf.Append (NestedTypeSeparator);
                }
                insertNested = true;
                AppendTypeName (buf, declDef, context);
                int ac = DocUtils.GetGenericArgumentCount (declDef);
                int c = ac - prev;
                prev = ac;
                if (c > 0)
                {
                    buf.Append (GenericTypeContainer[0]);
                    var origState = MemberFormatterState;
                    MemberFormatterState = MemberFormatterState.WithinGenericTypeParameters;
                    _AppendTypeName (buf, genArgs[argIdx++], context);
                    for (int i = 1; i < c; ++i)
                    {
                        _AppendTypeName (buf.Append (","), genArgs[argIdx++], context);
                    }
                    MemberFormatterState = origState;
                    buf.Append (GenericTypeContainer[1]);
                }
            }
            return buf;
        }

        protected List<TypeReference> GetGenericArguments (TypeReference type)
        {
            var args = new List<TypeReference> ();
            GenericInstanceType inst = type as GenericInstanceType;
            if (inst != null)
                args.AddRange (inst.GenericArguments);
            else
                args.AddRange (type.GenericParameters);
            return args;
        }

        protected virtual StringBuilder AppendGenericTypeConstraints (StringBuilder buf, TypeReference type)
        {
            return buf;
        }

        protected virtual string GetConstructorName (MethodReference constructor)
        {
            return constructor.Name;
        }

        protected virtual string GetMethodName (MethodReference method)
        {
            return method.Name;
        }

        protected virtual string GetPropertyName (PropertyReference property)
        {
            return property.Name;
        }

        protected virtual string GetFieldName (FieldReference field)
        {
            return field.Name;
        }

        protected virtual string GetEventName (EventReference e)
        {
            return e.Name;
        }

        public virtual string GetDeclaration (TypeReference tref)
        {
            if (!IsSupported(tref))
                return null;
            var typeSpec = tref as TypeSpecification;
            if (typeSpec != null && typeSpec.Resolve () == null && typeSpec.IsArray && typeSpec.ContainsGenericParameter)
            {
                //HACK: there's really no good reference for a generic parameter array, so we're going to use object
                return "T:System.Array";
            }
            TypeDefinition def = tref.Resolve ();
            if (def != null)
                return GetTypeDeclaration (def);
            else
                return GetTypeName (tref);
        }

        public virtual string GetDeclaration (MemberReference mreference)
        {
            if (!IsSupported(mreference))
                return null;
            return GetDeclaration (mreference.Resolve ());
        }

        string GetDeclaration (IMemberDefinition member)
        {
            if (member == null)
                throw new ArgumentNullException ("member");
            TypeDefinition type = member as TypeDefinition;
            if (type != null)
                return GetTypeDeclaration (type);
            MethodDefinition method = member as MethodDefinition;
            if (method != null && method.IsConstructor)
                return GetConstructorDeclaration (method);
            if (method != null)
                return GetMethodDeclaration (method);
            PropertyDefinition prop = member as PropertyDefinition;
            if (prop != null)
                return GetPropertyDeclaration (prop);
            FieldDefinition field = member as FieldDefinition;
            if (field != null)
                return GetFieldDeclaration (field);
            EventDefinition e = member as EventDefinition;
            if (e != null)
                return GetEventDeclaration (e);
            throw new NotSupportedException ("Can't handle: " + member.GetType ().ToString ());
        }

        protected virtual string GetTypeDeclaration (TypeDefinition type)
        {
            if (type == null)
                throw new ArgumentNullException ("type");
            StringBuilder buf = new StringBuilder (type.Name.Length);
            _AppendTypeName (buf, type, null);
            AppendGenericTypeConstraints (buf, type);
            return buf.ToString ();
        }

        protected virtual string GetConstructorDeclaration (MethodDefinition constructor)
        {
            return GetConstructorName (constructor);
        }

        protected virtual string GetMethodDeclaration (MethodDefinition method)
        {
            if (method.HasCustomAttributes && method.CustomAttributes.Cast<CustomAttribute> ().Any (
                        ca => ca.GetDeclaringType () == "System.Diagnostics.Contracts.ContractInvariantMethodAttribute"))
                return null;

            // Special signature for destructors.
            if (method.Name == "Finalize" && method.Parameters.Count == 0)
                return GetFinalizerName (method);

            StringBuilder buf = new StringBuilder ();

            AppendVisibility (buf, method);
            if (buf.Length == 0 &&
                    !(DocUtils.IsExplicitlyImplemented (method) && !method.IsSpecialName))
                return null;

            AppendModifiers (buf, method);

            if (buf.Length != 0)
                buf.Append (" ");

            buf.Append (GetTypeName (method.ReturnType, new DynamicParserContext (method.MethodReturnType))).Append (" ");

            AppendMethodName (buf, method);
            AppendGenericMethod (buf, method).Append (" ");
            AppendParameters (buf, method, method.Parameters);
            AppendGenericMethodConstraints (buf, method);
            return buf.ToString ();
        }

        protected virtual StringBuilder AppendMethodName (StringBuilder buf, MethodDefinition method)
        {
            return buf.Append (method.Name);
        }

        protected virtual string GetFinalizerName (MethodDefinition method)
        {
            return "Finalize";
        }

        protected virtual StringBuilder AppendVisibility (StringBuilder buf, MethodDefinition method)
        {
            return buf;
        }

        protected virtual StringBuilder AppendModifiers (StringBuilder buf, MethodDefinition method)
        {
            return buf;
        }

        protected virtual StringBuilder AppendGenericMethod (StringBuilder buf, MethodDefinition method)
        {
            return buf;
        }

        protected virtual StringBuilder AppendParameters (StringBuilder buf, MethodDefinition method, IList<ParameterDefinition> parameters)
        {
            return buf;
        }

        protected virtual StringBuilder AppendGenericMethodConstraints (StringBuilder buf, MethodDefinition method)
        {
            return buf;
        }

        protected virtual string GetPropertyDeclaration (PropertyDefinition property)
        {
            return GetPropertyName (property);
        }

        protected virtual string GetFieldDeclaration (FieldDefinition field)
        {
            return GetFieldName (field);
        }

        protected virtual string GetEventDeclaration (EventDefinition e)
        {
            return GetEventName (e);
        }

        public virtual bool IsSupported(TypeReference tref) => true;

        public virtual bool IsSupported(MemberReference mref) => true;
        
        protected static bool IsPublicEII (EventDefinition e)
        {
            bool isPublicEII = false;
            if (e.AddMethod.HasOverrides)
            {
                var resolvedAddMethod = e.AddMethod.Overrides[0].Resolve ();
                var resolvedInterface = e.AddMethod.Overrides[0].DeclaringType.Resolve ();
                if (DocUtils.IsPublic (resolvedInterface) && resolvedAddMethod != null && resolvedAddMethod.IsPublic)
                {
                    isPublicEII = true;
                }
            }

            return isPublicEII;
        }
    }
}