using Mono.Cecil;
using Mono.Documentation.Updater.Frameworks;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Mono.Documentation
{
    public partial class MDocUpdater
    {
        internal static void MakeTypeParameterConstraints(XmlElement root, XmlElement e, XmlElement pe, GenericParameter typeParameter)
        {
            if (typeParameter == null)
            {
                return;
            }

#if NEW_CECIL
            Mono.Collections.Generic.Collection<GenericParameterConstraint> constraints = typeParameter.Constraints;
#else
            IList<TypeReference> constraints = typeParameter.Constraints;
#endif
            GenericParameterAttributes attrs = typeParameter.Attributes;

            XmlElement ce = null;
            var typeParameterNode = (XmlElement)e.SelectSingleNode($"TypeParameter[@Name='{typeParameter.Name}']");
            if (typeParameterNode != null)
            {
                ce = (XmlElement)typeParameterNode.SelectSingleNode("Constraints");
            }
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
            // Check for 'allows ref struct' constraint
            if ((attrs & (GenericParameterAttributes)0x0020) != 0) // Assuming 0x0020 is the flag for 'allows ref struct'
                AppendElementText(ce, "ParameterAttribute", "AllowByRefLike");

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

        internal static void ClearFrameworkAlternateIfAll(XmlElement element, string elementName, string allFrameworks)
        {
            if (element == null || string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(allFrameworks))
            {
                return;
            }

            var finalNodes = element.GetElementsByTagName(elementName).Cast<XmlElement>().ToArray();
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

        internal static XmlElement AppendElementText(XmlNode parent, string element, string value)
        {
            XmlElement n = parent.OwnerDocument.CreateElement(element);
            parent.AppendChild(n);
            n.InnerText = value;
            return n;
        }
    }
}
