using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Documentation.Updater.Formatters.CppFormatters;

namespace Mono.Documentation.Updater.CppFormatters
{
    public class CppWinRtFullMemberFormatter : CppCxFullMemberFormatter
    {
        protected override bool AppendHatOnReturn => false;
        protected override string HatModifier => $" const{RefTypeModifier}";
        public override string Language => Consts.CppWinRt;
        protected override string RefTypeModifier => " &";
        
        protected override IList<string> GetAllowedTypes()
        {
            return new List<string>(AllowedFundamentalTypes);
        }

        protected override StringBuilder AppendNamespace(StringBuilder buf, TypeReference type)
        {
            var @namespace=base.AppendNamespace(buf, type);
            if (@namespace.ToString().StartsWith($"Windows{NestedTypeSeparator}"))
            {
                buf.Insert(0, $"winrt{NestedTypeSeparator}");
            }

            return @namespace;
        }

        protected override string GetCppType(string t)
        {
            // make sure there are no modifiers in the type string (add them back before returning)
            string typeToCompare = t;
            string[] splitType = null;
            if (t.Contains(' '))
            {
                splitType = t.Split(' ');
                typeToCompare = splitType[0];

                foreach (var str in splitType)
                {
                    if (str == "modopt(System.Runtime.CompilerServices.IsLong)" && typeToCompare == "System.Int32")
                        return "long";
                    if (str == "modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte)" &&
                        typeToCompare == "System.SByte")
                        return "char";
                }
            }

            switch (typeToCompare)
            {
                case "System.Byte": typeToCompare = "byte"; break;
                case "System.Int16": typeToCompare = "short"; break;
                case "System.Int32": typeToCompare = "int"; break;
                case "System.Int64": typeToCompare = "long"; break;
                case "System.UInt16": typeToCompare = "unsigned short"; break;
                case "System.UInt32": typeToCompare = "unsigned int"; break;
                case "System.UInt64": typeToCompare = "unsigned long";break;
                    
                case "System.Single": typeToCompare = "float"; break;
                case "System.Double": typeToCompare = "double"; break;

                case "System.Boolean": typeToCompare = "bool"; break;
                case "System.Char": typeToCompare = "char"; break;
                case "System.Void": typeToCompare = "void"; break;
                //API specific type is "winrt::hstring"; but c++ in built type is better variant
                case "System.String": typeToCompare = "std::wstring"; break;
                case "System.Object": typeToCompare = "winrt::Windows::Foundation::IInspectable"; break;
            }

            if (splitType != null)
            {
                // re-add modreq/modopt if it was there
                splitType[0] = typeToCompare;
                typeToCompare = string.Join(" ", splitType);
            }
            return typeToCompare == t ? null : typeToCompare;
        }

        protected override  StringBuilder AppendParameter(StringBuilder buf, ParameterDefinition parameter)
        {
            if (parameter.ParameterType is ByReferenceType && parameter.IsOut)
            {
               //no notion of out -> mark with attribute to distinguish in other languages 
                buf.Append("[Runtime::InteropServices::Out] ");
            }

            if (parameter.HasCustomAttributes)
            {
                var isParams = parameter.CustomAttributes.Any(ca => ca.AttributeType.Name == "ParamArrayAttribute");
                if (isParams)
                    buf.AppendFormat("... ");
            }

            buf.Append(GetTypeNameWithOptions(parameter.ParameterType, !AppendHatOnReturn)).Append(" ");
            buf.Append(parameter.Name);

            if (parameter.HasDefault && parameter.IsOptional && parameter.HasConstant)
            {
                buf.AppendFormat(" = {0}", MDocUpdater.MakeAttributesValueString(parameter.Constant, parameter.ParameterType));
            }

            return buf;
        }
        
        protected override string GetTypeKind(TypeDefinition t)
        {
            if (t.IsEnum || t.FullName == "System.Enum")
                return "enum";
            if (t.IsValueType)
                return "struct";
            if (t.IsClass)
                return "class";
            if (t.IsInterface)
                return "__interface";
            throw new ArgumentException(t.FullName);
        }

        protected override StringBuilder AppendArrayTypeName(StringBuilder buf, TypeReference type,
            DynamicParserContext context)
        {
            buf.Append("std::Array <");

            var item = type is TypeSpecification spec ? spec.ElementType : type.GetElementType();
            _AppendTypeName(buf, item, context);
            AppendHat(buf, item);

            if (type is ArrayType arrayType)
            {
                int rank = arrayType.Rank;
                if (rank > 1)
                {
                    buf.AppendFormat(", {0}", rank);
                }
            }

            buf.Append(">");

            return buf;
        }

        protected override StringBuilder AppendExplisitImplementationMethod(StringBuilder buf, MethodDefinition method)
        {
            //no need to add additional syntax
            return buf;
        }

        protected override string GetTypeDeclaration(TypeDefinition type)
        {
            StringBuilder buf = new StringBuilder();
            
            var genericParamList = GetTypeSpecifiGenericParameters(type);
            AppendGenericItem(buf, genericParamList);
            AppendGenericTypeConstraints(buf, type);

            AppendWebHostHiddenAttribute(buf, type);

            buf.Append(GetTypeKind(type));
            buf.Append(" ");
            buf.Append(GetCppType(type.FullName) == null
                    ? GetNameWithOptions(type, false, false)
                    : type.Name);

            if (type.IsAbstract && !type.IsInterface)
                buf.Append(" abstract");
            if (type.IsSealed && !DocUtils.IsDelegate(type) && !type.IsValueType)
                buf.Append(" sealed");

            CppWinRtFullMemberFormatter full = new CppWinRtFullMemberFormatter();

            if (!type.IsEnum)
            {
                TypeReference basetype = type.BaseType;
                if (basetype != null && basetype.FullName == "System.Object" || type.IsValueType)   // FIXME
                    basetype = null;

                List<string> interfaceNames;
                try
                {
                    //for winRt Resolve() can fail as Cecil understands winRt types as .net (eg, "System.Object" cannot be resolved)
                    interfaceNames = DocUtils.GetUserImplementedInterfaces(type)
                        .Select(iface => full.GetNameWithOptions(iface, true, false))
                        .OrderBy(s => s)
                        .ToList();
                }
                catch
                {
                    interfaceNames = null;
                }

                if (basetype != null || interfaceNames?.Count > 0)
                    buf.Append(" : ");

                if (basetype != null)
                {
                    buf.Append(full.GetNameWithOptions(basetype, true, false));
                    if (interfaceNames?.Count > 0)
                        buf.Append(", ");
                }

                for (int i = 0; i < interfaceNames?.Count; i++)
                {
                    if (i != 0)
                        buf.Append(", ");
                    buf.Append(interfaceNames?[i]);
                }

            }

            return buf.ToString();
        }

        protected override string GetTypeVisibility(TypeAttributes ta)
        {
            //Cannot have pubic/protected visibility since it uses native C++ which cannot be exposed 
            return string.Empty;
        }

        protected override StringBuilder AppendVisibility(StringBuilder buf, MethodDefinition method)
        {
            //Cannot have pubic/protected visibility since it uses native C++ which cannot be exposed 
            return buf;
        }

        protected override StringBuilder AppendFieldVisibility(StringBuilder buf, FieldDefinition field)
        {
            //Cannot have pubic/protected visibility since it uses native C++ which cannot be exposed 
            return buf;
        }

        protected override StringBuilder AppendGenericItem(StringBuilder buf, IList<GenericParameter> args)
        {
            if (args != null && args.Any())
            {
                buf.Append("template <typename ");
                buf.Append(args[0].Name);
                for (int i = 1; i < args.Count; ++i)
                    buf.Append(", typename ").Append(args[i].Name);
                buf.Append(">");
                buf.Append(GetLineEnding());
            }
            return buf;
        }

        protected override string AppendSealedModifiers(string modifiersString, MethodDefinition method)
        {
            if (method.IsFinal) modifiersString += " sealed";
            if (modifiersString == " virtual sealed") modifiersString = "";
            
            return modifiersString;
        }

        public override bool IsSupported(TypeReference tref)
        {
            if (HasNestedClassesDuplicateNames(tref))
                return false;

            var ns = DocUtils.GetNamespace(tref);
            var allowedTypes = GetAllowedTypes();

            if (allowedTypes.Contains(tref.FullName.Split(' ')[0]) 
                //winRt specific namespaces so no need for further check
                || ns.StartsWith("Windows.")
                )
            {
                return true;
            }

            TypeDefinition typedef = null;
            try
            {
                typedef = tref.Resolve();
            }
            catch
            {
                //for winRt Resolve() can fail as Cecil understands winRt types as .net (eg, "System.Object" cannot be resolved)
            }

            if (typedef != null)
            {
                if(allowedTypes.Contains(typedef.FullName))
                    //to check type of array
                    return true;

                if (DocUtils.IsDelegate(typedef))
                {
                    //delegates can be used only in managed context
                    return false;
                }
                
                if (typedef.HasGenericParameters &&
                    typedef.GenericParameters.Any(x => x.HasConstraints
                                                       || x.HasReferenceTypeConstraint
                                                       || x.HasDefaultConstructorConstraint
                                                       || x.HasNotNullableValueTypeConstraint)
                )
                {
                    //Type parameters cannot be constrained
                    return false;
                }

                if (HasUnsupportedParent(typedef))
                {
                    return false;
                }
            }

            return IsSupportedGenericParameter(tref) 
                && !ns.StartsWith("System.") && !ns.Equals("System");
        }

        public override bool IsSupportedProperty(PropertyDefinition pdef)
        {
            //properties can be used only in managed context
            return false;
        }

        public override bool IsSupportedEvent(EventDefinition edef)
        {
            //events can be used only in managed context
            return false;
        }

        public override bool IsSupportedField(FieldDefinition fdef)
        {
            return IsSupported(fdef.FieldType);
        }

        public override bool IsSupportedMethod(MethodDefinition mdef)
        {
            if (DocUtils.IsExtensionMethod(mdef)
                //no support of 'params';
                //can be substituted with 'Variadic functions' hovewer it's not full equivalent(not type safe + specific mechanism for reading)
                || mdef.Parameters.Any(IsParamsParameter)
                )
            {
                return false;
            }

            return
                IsSupported(mdef.ReturnType)
                && mdef.Parameters.All(i => IsSupported(i.ParameterType));
        }
    }
}