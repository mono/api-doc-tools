﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Mono.Cecil;

using Mono.Documentation.Util;

namespace Mono.Documentation.Updater
{
    public class CSharpFullMemberFormatter : MemberFormatter
    {
        public CSharpFullMemberFormatter() : this(null) {}
        public CSharpFullMemberFormatter(TypeMap map) : base(map) { }

        public override string Language
        {
            get { return "C#"; }
        }

        protected override StringBuilder AppendNamespace (StringBuilder buf, TypeReference type)
        {

            string ns = DocUtils.GetNamespace (type);
            if (GetCSharpType (type.FullName) == null && ns != null && ns.Length > 0 && ns != "System")
                buf.Append (ns).Append ('.');
            return buf;
        }

        protected virtual string GetCSharpType (string t)
        {
            // make sure there are no modifiers in the type string (add them back before returning)
            string typeToCompare = t;
            string[] splitType = null;
            if (t.Contains (' '))
            {
                splitType = t.Split (' ');
                typeToCompare = splitType[0];
            }

            switch (typeToCompare)
            {
                case "System.Byte": typeToCompare = "byte"; break;
                case "System.SByte": typeToCompare = "sbyte"; break;
                case "System.Int16": typeToCompare = "short"; break;
                case "System.Int32": typeToCompare = "int"; break;
                case "System.Int64": typeToCompare = "long"; break;

                case "System.UInt16": typeToCompare = "ushort"; break;
                case "System.UInt32": typeToCompare = "uint"; break;
                case "System.UInt64": typeToCompare = "ulong"; break;

                case "System.Single": typeToCompare = "float"; break;
                case "System.Double": typeToCompare = "double"; break;
                case "System.Decimal": typeToCompare = "decimal"; break;
                case "System.Boolean": typeToCompare = "bool"; break;
                case "System.Char": typeToCompare = "char"; break;
                case "System.Void": typeToCompare = "void"; break;
                case "System.String": typeToCompare = "string"; break;
                case "System.Object": typeToCompare = "object"; break;
            }

            if (splitType != null)
            {
                // re-add modreq/modopt if it was there
                splitType[0] = typeToCompare;
                typeToCompare = string.Join (" ", splitType);
            }
            return typeToCompare == t ? null : typeToCompare;
        }

        protected override StringBuilder AppendTypeName (StringBuilder buf, TypeReference type, DynamicParserContext context)
        {
            if (context != null && context.TransformFlags != null &&
                    (context.TransformFlags.Count == 0 || context.TransformFlags[context.TransformIndex]))
            {
                context.TransformIndex++;
                return buf.Append ("dynamic");
            }

            if (type is GenericParameter)
                return AppendGenericParameterConstraints (buf, (GenericParameter)type, context).Append (type.Name);
            string t = type.FullName;
            if (!t.StartsWith ("System."))
            {
                return base.AppendTypeName (buf, type, context);
            }

            string s = GetCSharpType (t);
            if (s != null)
            {
                if (context != null)
                    context.TransformIndex++;
                return buf.Append (s);
            }

            return base.AppendTypeName (buf, type, context);
        }

        private StringBuilder AppendGenericParameterConstraints (StringBuilder buf, GenericParameter type, DynamicParserContext context)
        {
            if (MemberFormatterState != MemberFormatterState.WithinGenericTypeParameters)
                return buf;
            GenericParameterAttributes attrs = type.Attributes;
            bool isout = (attrs & GenericParameterAttributes.Covariant) != 0;
            bool isin = (attrs & GenericParameterAttributes.Contravariant) != 0;
            if (isin)
                buf.Append ("in ");
            else if (isout)
                buf.Append ("out ");
            return buf;
        }

        protected override string GetTypeName(TypeReference type, DynamicParserContext context, bool appendGeneric = true, bool useTypeProjection = true)
        {
            GenericInstanceType genType = type as GenericInstanceType;
            if (genType != null)
            {
                if (genType.Name.StartsWith("Nullable`") && genType.HasGenericArguments)
                {

                    var underlyingTypeName = base.GetTypeName(genType.GenericArguments.First(), context, appendGeneric, useTypeProjection);
                    return underlyingTypeName + "?";
                }

                if (genType.Name.StartsWith("Tuple`") || genType.Name.StartsWith("ValueTuple`"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("(");

                    var genArgList = genType.GenericArguments.Select(genArg => base.GetTypeName(genArg, context, appendGeneric, useTypeProjection)).ToArray();

                    sb.Append(string.Join(",", genArgList));

                    sb.Append(")");

                    return sb.ToString();
                }
            }

            return base.GetTypeName(type, context, appendGeneric, useTypeProjection);
        }

        protected override string GetTypeDeclaration (TypeDefinition type)
        {
            string visibility = GetTypeVisibility (type.Attributes);
            if (visibility == null)
                return null;

            StringBuilder buf = new StringBuilder ();

            buf.Append (visibility);
            buf.Append (" ");

            MemberFormatter full = new CSharpFullMemberFormatter (this.TypeMap);

            if (DocUtils.IsDelegate (type))
            {
                buf.Append ("delegate ");
                MethodDefinition invoke = type.GetMethod ("Invoke");
                buf.Append (full.GetName (invoke.ReturnType, new DynamicParserContext (invoke.MethodReturnType))).Append (" ");
                buf.Append (GetName (type));
                AppendParameters (buf, invoke, invoke.Parameters);
                AppendGenericTypeConstraints (buf, type);
                buf.Append (";");

                return buf.ToString ();
            }

            if (type.IsAbstract && !type.IsInterface)
                buf.Append ("abstract ");
            if (type.IsSealed && !DocUtils.IsDelegate (type) && !type.IsValueType)
                buf.Append ("sealed ");
            buf.Replace ("abstract sealed", "static");

            buf.Append (GetTypeKind (type));
            buf.Append (" ");
            buf.Append (GetCSharpType (type.FullName) == null
                    ? GetName (type)
                    : type.Name);

            if (!type.IsEnum)
            {
                TypeReference basetype = type.BaseType;
                if (basetype != null && basetype.FullName == "System.Object" || type.IsValueType)   // FIXME
                    basetype = null;

                List<string> interface_names = DocUtils.GetUserImplementedInterfaces (type)
                        .Select (iface => full.GetName (iface))
                        .OrderBy (s => s)
                        .ToList ();

                if (basetype != null || interface_names.Count > 0)
                    buf.Append (" : ");

                if (basetype != null)
                {
                    buf.Append (full.GetName (basetype));
                    if (interface_names.Count > 0)
                        buf.Append (", ");
                }

                for (int i = 0; i < interface_names.Count; i++)
                {
                    if (i != 0)
                        buf.Append (", ");
                    buf.Append (interface_names[i]);
                }
                AppendGenericTypeConstraints (buf, type);
            }

            return buf.ToString ();
        }

        static string GetTypeKind (TypeDefinition t)
        {
            if (t.IsEnum)
                return "enum";
            if (t.IsValueType)
                return "struct";
            if (t.IsClass || t.FullName == "System.Enum")
                return "class";
            if (t.IsInterface)
                return "interface";
            throw new ArgumentException (t.FullName);
        }

        static string GetTypeVisibility (TypeAttributes ta)
        {
            switch (ta & TypeAttributes.VisibilityMask)
            {
                case TypeAttributes.Public:
                case TypeAttributes.NestedPublic:
                    return "public";

                case TypeAttributes.NestedFamily:
                    return "protected";

                case TypeAttributes.NestedFamORAssem:
                    return "protected internal";

                default:
                    return null;
            }
        }

        protected override StringBuilder AppendGenericTypeConstraints (StringBuilder buf, TypeReference type)
        {
            if (type.GenericParameters.Count == 0)
                return buf;
            return AppendConstraints (buf, type.GenericParameters);
        }

        private StringBuilder AppendConstraints (StringBuilder buf, IList<GenericParameter> genArgs)
        {
            foreach (GenericParameter genArg in genArgs)
            {
                GenericParameterAttributes attrs = genArg.Attributes;
#if NEW_CECIL
                Mono.Collections.Generic.Collection<GenericParameterConstraint> constraints = genArg.Constraints;
#else
                IList<TypeReference> constraints = genArg.Constraints;
#endif
                if (attrs == GenericParameterAttributes.NonVariant && constraints.Count == 0)
                    continue;

                bool isref = (attrs & GenericParameterAttributes.ReferenceTypeConstraint) != 0;
                bool isvt = (attrs & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0;
                bool isnew = (attrs & GenericParameterAttributes.DefaultConstructorConstraint) != 0;
                bool comma = false;

                if (!isref && !isvt && !isnew && constraints.Count == 0)
                    continue;
                buf.Append (" where ").Append (genArg.Name).Append (" : ");
                if (isref)
                {
                    buf.Append ("class");
                    comma = true;
                }
                else if (isvt)
                {
                    buf.Append ("struct");
                    comma = true;
                }
                if (constraints.Count > 0 && !isvt)
                {
                    if (comma)
                        buf.Append (", ");
#if NEW_CECIL
                    buf.Append (GetTypeName (constraints[0].ConstraintType));
                    for (int i = 1; i < constraints.Count; ++i)
                        buf.Append (", ").Append (GetTypeName (constraints[i].ConstraintType));
#else
                    buf.Append (GetTypeName (constraints[0]));
                    for (int i = 1; i < constraints.Count; ++i)
                        buf.Append (", ").Append (GetTypeName (constraints[i]));
#endif
                }
                if (isnew && !isvt)
                {
                    if (comma)
                        buf.Append (", ");
                    buf.Append ("new()");
                }
            }
            return buf;
        }

        protected override string GetConstructorDeclaration (MethodDefinition constructor)
        {
            StringBuilder buf = new StringBuilder ();
            AppendVisibility (buf, constructor);
            if (buf.Length == 0 && !constructor.IsStatic) //Static constructor is needed
                return null;

            if (constructor.IsStatic)
                buf.Append(buf.Length == 0 ? "static" : " static");

            buf.Append (' ');
            base.AppendTypeName (buf, constructor.DeclaringType.Name).Append (' ');
            AppendParameters (buf, constructor, constructor.Parameters);
            buf.Append (';');

            return buf.ToString ();
        }

        protected override string GetMethodDeclaration (MethodDefinition method)
        {
            string decl = base.GetMethodDeclaration (method);
            if (decl != null)
                return decl + ";";
            return null;
        }

        protected override StringBuilder AppendMethodName (StringBuilder buf, MethodDefinition method)
        {
            if (DocUtils.IsExplicitlyImplemented (method))
            {
                TypeReference iface;
                MethodReference ifaceMethod;
                DocUtils.GetInfoForExplicitlyImplementedMethod (method, out iface, out ifaceMethod);
                return buf.Append (new CSharpMemberFormatter (this.TypeMap).GetName (iface))
                    .Append ('.')
                    .Append (ifaceMethod.Name);
            }

            if (method.Name.StartsWith ("op_", StringComparison.Ordinal))
            {
                // this is an operator
                switch (method.Name)
                {
                    case "op_Implicit":
                    case "op_Explicit":
                        buf.Length--; // remove the last space, which assumes a member name is coming
                        return buf;
                    case "op_Addition":
                    case "op_UnaryPlus":
                        return buf.Append ("operator +");
                    case "op_Subtraction":
                    case "op_UnaryNegation":
                        return buf.Append ("operator -");
                    case "op_Division":
                        return buf.Append ("operator /");
                    case "op_Multiply":
                        return buf.Append ("operator *");
                    case "op_Modulus":
                        return buf.Append ("operator %");
                    case "op_BitwiseAnd":
                        return buf.Append ("operator &");
                    case "op_BitwiseOr":
                        return buf.Append ("operator |");
                    case "op_ExclusiveOr":
                        return buf.Append ("operator ^");
                    case "op_LeftShift":
                        return buf.Append ("operator <<");
                    case "op_RightShift":
                        return buf.Append ("operator >>");
                    case "op_LogicalNot":
                        return buf.Append ("operator !");
                    case "op_OnesComplement":
                        return buf.Append ("operator ~");
                    case "op_Decrement":
                        return buf.Append ("operator --");
                    case "op_Increment":
                        return buf.Append ("operator ++");
                    case "op_True":
                        return buf.Append ("operator true");
                    case "op_False":
                        return buf.Append ("operator false");
                    case "op_Equality":
                        return buf.Append ("operator ==");
                    case "op_Inequality":
                        return buf.Append ("operator !=");
                    case "op_LessThan":
                        return buf.Append ("operator <");
                    case "op_LessThanOrEqual":
                        return buf.Append ("operator <=");
                    case "op_GreaterThan":
                        return buf.Append ("operator >");
                    case "op_GreaterThanOrEqual":
                        return buf.Append ("operator >=");
                    default:
                        return base.AppendMethodName (buf, method);
                }
            }
            else
                return base.AppendMethodName (buf, method);
        }

        protected override StringBuilder AppendGenericMethodConstraints (StringBuilder buf, MethodDefinition method)
        {
            if (method.GenericParameters.Count == 0)
                return buf;
            return AppendConstraints (buf, method.GenericParameters);
        }

        protected override string RefTypeModifier
        {
            get { return ""; }
        }

        protected override StringBuilder AppendRefTypeName(
            StringBuilder buf, ByReferenceType type, DynamicParserContext context)
        {
            buf.Append("ref ");
            return base.AppendRefTypeName(buf, type, context);
        }

        protected override StringBuilder AppendRequiredModifierType(
            StringBuilder buf, RequiredModifierType type, DynamicParserContext context)
        {
            if (type.ModifierType.FullName == "System.Runtime.InteropServices.InAttribute" &&
                type.ElementType is ByReferenceType refType)
            {
                buf.Append("ref readonly ");
                return _AppendTypeName(buf, refType.ElementType, context);
            }

            return base.AppendRequiredModifierType(buf, type, context);
        }

        protected override string GetFinalizerName (MethodDefinition method)
        {
            StringBuilder buf = new StringBuilder();
            base.AppendTypeName(buf, method.DeclaringType.Name);

            return $"~{buf} ()";
        }

        protected override StringBuilder AppendVisibility (StringBuilder buf, MethodDefinition method)
        {
            if (method == null)
                return buf;
            if (method.IsPublic)
                return buf.Append ("public");
            if (method.IsFamily)
                return buf.Append ("protected");
            if (method.IsFamilyOrAssembly)
                return buf.Append("protected internal");
            return buf;
        }

        protected override StringBuilder AppendModifiers (StringBuilder buf, MethodDefinition method)
        {
            string modifiers = String.Empty;
            if (method.IsStatic) modifiers += " static";
            if (method.IsVirtual && !method.IsAbstract)
            {
                if ((method.Attributes & MethodAttributes.NewSlot) != 0) modifiers += " virtual";
                else modifiers += " override";
            }
            TypeDefinition declType = (TypeDefinition)method.DeclaringType;
            if (method.IsAbstract && !declType.IsInterface) modifiers += " abstract";
            if (method.IsFinal) modifiers += " sealed";
            if (modifiers == " virtual sealed") modifiers = "";

            switch (method.Name)
            {
                case "op_Implicit":
                    modifiers += " implicit operator";
                    break;
                case "op_Explicit":
                    modifiers += " explicit operator";
                    break;
            }

            return buf.Append (modifiers);
        }

        protected override StringBuilder AppendGenericMethod (StringBuilder buf, MethodDefinition method)
        {
            if (method.IsGenericMethod ())
            {
                IList<GenericParameter> args = method.GenericParameters;
                if (args.Count > 0)
                {
                    buf.Append ("<");
                    buf.Append (args[0].Name);
                    for (int i = 1; i < args.Count; ++i)
                        buf.Append (",").Append (args[i].Name);
                    buf.Append (">");
                }
            }
            return buf;
        }

        protected override StringBuilder AppendParameters (StringBuilder buf, MethodDefinition method, IList<ParameterDefinition> parameters)
        {
            return AppendParameters (buf, method, parameters, '(', ')');
        }

        private StringBuilder AppendParameters (StringBuilder buf, MethodDefinition method, IList<ParameterDefinition> parameters, char begin, char end)
        {
            buf.Append (begin);

            if (parameters.Count > 0)
            {
                if (DocUtils.IsExtensionMethod (method))
                    buf.Append ("this ");
                AppendParameter (buf, parameters[0]);
                for (int i = 1; i < parameters.Count; ++i)
                {
                    buf.Append (", ");
                    AppendParameter (buf, parameters[i]);
                }
            }

            return buf.Append (end);
        }

        private StringBuilder AppendParameter (StringBuilder buf, ParameterDefinition parameter)
        {
            TypeReference parameterType = parameter.ParameterType;

            if (parameterType is ByReferenceType byReferenceType)
            {
                if (parameter.IsOut)
                    buf.Append ("out ");
                else if (parameter.IsIn &&
                    parameter.CustomAttributes.Any(ca => ca.AttributeType.FullName == "System.Runtime.CompilerServices.IsReadOnlyAttribute"))
                    buf.Append ("in ");
                else
                    buf.Append ("ref ");

                parameterType = byReferenceType.ElementType;
            }
            if (parameter.HasCustomAttributes)
            {
                var isParams = parameter.CustomAttributes.Any (ca => ca.AttributeType.Name == "ParamArrayAttribute");
                if (isParams)
                    buf.AppendFormat ("params ");
            }
            buf.Append (GetTypeName (parameterType, new DynamicParserContext (parameter))).Append (" ");
            buf.Append (parameter.Name);
            if (parameter.HasDefault && parameter.IsOptional && parameter.HasConstant)
            {
                var ReturnVal = MDocUpdater.MakeAttributesValueString(parameter.Constant, parameter.ParameterType);
                buf.AppendFormat (" = {0}", ReturnVal == "null" ? "default" : ReturnVal);
            }
            return buf;
        }

        protected override string GetPropertyDeclaration (PropertyDefinition property)
        {
            MethodDefinition method;

            string get_visible = null;
            if ((method = property.GetMethod) != null &&
                    (DocUtils.IsExplicitlyImplemented (method) ||
                     (!method.IsPrivate && !method.IsAssembly && !method.IsFamilyAndAssembly)))
                get_visible = AppendVisibility (new StringBuilder (), method).ToString ();
            string set_visible = null;
            if ((method = property.SetMethod) != null &&
                    (DocUtils.IsExplicitlyImplemented (method) ||
                     (!method.IsPrivate && !method.IsAssembly && !method.IsFamilyAndAssembly)))
                set_visible = AppendVisibility (new StringBuilder (), method).ToString ();

            if ((set_visible == null) && (get_visible == null))
                return null;

            string visibility;
            StringBuilder buf = new StringBuilder ();
            if (get_visible != null && (set_visible == null || (set_visible != null && get_visible == set_visible)))
                buf.Append (visibility = get_visible);
            else if (set_visible != null && get_visible == null)
                buf.Append (visibility = set_visible);
            else
                buf.Append (visibility = "public");

            // Pick an accessor to use for static/virtual/override/etc. checks.
            method = property.SetMethod;
            if (method == null)
                method = property.GetMethod;

            string modifiers = String.Empty;
            if (method.IsStatic) modifiers += " static";
            if (method.IsVirtual && !method.IsAbstract)
            {
                if ((method.Attributes & MethodAttributes.NewSlot) != 0)
                    modifiers += " virtual";
                else
                    modifiers += " override";
            }
            TypeDefinition declDef = (TypeDefinition)method.DeclaringType;
            if (method.IsAbstract && !declDef.IsInterface)
                modifiers += " abstract";
            if (method.IsFinal)
                modifiers += " sealed";
            if (modifiers == " virtual sealed")
                modifiers = "";
            buf.Append (modifiers).Append (' ');

            buf.Append (GetTypeName (property.PropertyType, new DynamicParserContext (property))).Append (' ');

            IEnumerable<MemberReference> defs = property.DeclaringType.GetDefaultMembers ();
            string name = property.Name;
            foreach (MemberReference mi in defs)
            {
                if (mi == property)
                {
                    name = "this";
                    break;
                }
            }
            buf.Append (name == "this" ? name : DocUtils.GetPropertyName (property, NestedTypeSeparator));

            if (property.Parameters.Count != 0)
            {
                AppendParameters (buf, method, property.Parameters, '[', ']');
            }

            buf.Append (" {");
            if (get_visible != null)
            {
                if (get_visible != visibility)
                    buf.Append (' ').Append (get_visible);
                buf.Append (" get;");
            }
            if (set_visible != null)
            {
                if (set_visible != visibility)
                    buf.Append (' ').Append (set_visible);
                buf.Append (" set;");
            }
            buf.Append (" }");

            return buf[0] != ' ' ? buf.ToString () : buf.ToString (1, buf.Length - 1);
        }

        protected override string GetFieldDeclaration (FieldDefinition field)
        {
            TypeDefinition declType = (TypeDefinition)field.DeclaringType;
            if (declType.IsEnum && field.Name == "value__")
                return null; // This member of enums aren't documented.

            StringBuilder buf = new StringBuilder ();
            AppendFieldVisibility (buf, field);
            if (buf.Length == 0)
                return null;

            if (declType.IsEnum)
                return field.Name;

            if (field.IsStatic && !field.IsLiteral)
                buf.Append (" static");
            if (field.IsInitOnly)
                buf.Append (" readonly");
            if (field.IsLiteral)
                buf.Append (" const");

            buf.Append (' ').Append (GetTypeName (field.FieldType, new DynamicParserContext (field))).Append (' ');
            buf.Append (field.Name);
            DocUtils.AppendFieldValue (buf, field);
            buf.Append (';');

            return buf.ToString ();
        }

        static void AppendFieldVisibility (StringBuilder buf, FieldDefinition field)
        {
            if (field.IsPublic)
            {
                buf.Append("public");
                return;
            }
            if (field.IsFamily) 
            {
                buf.Append("protected");
            }
            if ( field.IsFamilyOrAssembly)
            {
                buf.Append("protected internal");
            }
        }

        protected override string GetEventDeclaration (EventDefinition e)
        {
            StringBuilder buf = new StringBuilder ();

            if (AppendVisibility (buf, e.AddMethod).Length == 0 && !IsPublicEII (e))
            {
                return null;
            }
            if (e.DeclaringType.IsInterface)
                buf.Clear ();

            AppendModifiers (buf, e.AddMethod);

            buf.Append (buf.Length == 0 ? "event " : " event ");
            buf.Append (GetTypeName (e.EventType, new DynamicParserContext (e.AddMethod.Parameters[0]))).Append (' ');
            buf.Append (e.Name).Append (';');

            return buf.ToString ();
        }
    }
}