using Mono.Cecil;
using Mono.Documentation;
using Mono.Documentation.Updater;
using Mono.Documentation.Updater.Frameworks;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace mdoc.Mono.Documentation.Util
{
    class MdocUpdaterHelper
    {
        internal static void MakeTypeParameterConstraints(XmlElement root, XmlElement e, XmlElement pe, GenericParameter typeParameter)
        {
#if NEW_CECIL
            Mono.Collections.Generic.Collection<GenericParameterConstraint> constraints = typeParameter.Constraints;
#else
            IList<TypeReference> constraints = typeParameter.Constraints;
#endif
            GenericParameterAttributes attrs = typeParameter.Attributes;

            XmlElement ce = (XmlElement)e.SelectSingleNode("Constraints");
            if (attrs == GenericParameterAttributes.NonVariant && constraints.Count == 0)
            {
                if (ce != null)
                    e.RemoveChild(ce);
            }
            if (ce != null)
                ce.RemoveAll();
            else
            {
                ce = root.OwnerDocument.CreateElement("Constraints");
            }
            if ((attrs & GenericParameterAttributes.Contravariant) != 0)
                AppendElementText(ce, "ParameterAttribute", "Contravariant");
            if ((attrs & GenericParameterAttributes.Covariant) != 0)
                AppendElementText(ce, "ParameterAttribute", "Covariant");
            if ((attrs & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
                AppendElementText(ce, "ParameterAttribute", "DefaultConstructorConstraint");
            if ((attrs & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
                AppendElementText(ce, "ParameterAttribute", "NotNullableValueTypeConstraint");
            if ((attrs & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
                AppendElementText(ce, "ParameterAttribute", "ReferenceTypeConstraint");

#if NEW_CECIL
                       foreach (GenericParameterConstraint c in constraints)
                       {
                           TypeDefinition cd = c.ConstraintType.Resolve ();
                            AppendElementText (ce,
                                    (cd != null && cd.IsInterface) ? "InterfaceName" : "BaseTypeName",
                                    GetDocTypeFullName (c.ConstraintType));
                        }
#else
            foreach (TypeReference c in constraints)
            {
                TypeDefinition cd = c.Resolve();
                AppendElementText(ce,
                        (cd != null && cd.IsInterface) ? "InterfaceName" : "BaseTypeName",
                        GetDocTypeFullName(c));
            }
#endif
            if (ce.HasChildNodes)
            {
                pe.AppendChild(ce);
            }
        }

        internal static void CheckFrameworkAlternateAttribute(FrameworkTypeEntry entry, XmlElement e, string elementName)
        {
            if (entry.Framework.IsLastFrameworkForType(entry))
            {
                var allFrameworks = entry.Framework.AllFrameworksWithType(entry);
                var finalNodes = e.GetElementsByTagName(elementName).Cast<XmlElement>().ToArray();
                foreach (var node in finalNodes)
                {
                    // if FXAlternate is entire list, just remove it
                    if (node.HasAttribute(Consts.FrameworkAlternate) && node.GetAttribute(Consts.FrameworkAlternate) == allFrameworks)
                    {
                        node.RemoveAttribute(Consts.FrameworkAlternate);
                    }
                }

                // if there are no fx attributes left, just remove the indices entirely
                if (!finalNodes.Any(n => n.HasAttribute(Consts.FrameworkAlternate)))
                {
                    foreach (var node in finalNodes)
                    {
                        node.RemoveAttribute(Consts.Index);
                    }
                }
            }
        }

        internal static XmlElement AppendElementText(XmlNode parent, string element, string value)
        {
            XmlElement n = parent.OwnerDocument.CreateElement(element);
            parent.AppendChild(n);
            n.InnerText = value;
            return n;
        }

        internal static string GetDocTypeFullName(TypeReference type, bool useTypeProjection = true, bool isTypeofOperator = false)
        {
            return DocTypeFullMemberFormatter.Default.GetName(type, useTypeProjection: useTypeProjection, isTypeofOperator: isTypeofOperator);
        }
    }
}
