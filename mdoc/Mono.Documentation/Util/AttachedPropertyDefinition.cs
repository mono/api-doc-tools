using Mono.Cecil;
using Mono.Collections.Generic;

namespace Mono.Documentation.Util
{
    public class AttachedPropertyDefinition : AttachedPropertyReference, IMemberDefinition
    {
        string defName;
        Collection<CustomAttribute> defAttributes;
        bool defHasAttributes, defIsSpecialName, defIsRuntimeSpecialName;
        TypeDefinition defDeclaringType;


        public AttachedPropertyDefinition(FieldDefinition fieldDefinition, MetadataToken metadataToken) : base(fieldDefinition)
        {
            MetadataToken = metadataToken;
            defName = fieldDefinition.Name;
            defAttributes = fieldDefinition.CustomAttributes;
            defHasAttributes = fieldDefinition.HasCustomAttributes;
            defIsSpecialName = fieldDefinition.IsSpecialName;
            defIsRuntimeSpecialName = fieldDefinition.IsRuntimeSpecialName;
            defDeclaringType = fieldDefinition.DeclaringType;
        }
        public AttachedPropertyDefinition(PropertyDefinition propDefinition, MetadataToken metadataToken) : base(propDefinition)
        {
            MetadataToken = metadataToken;
            defName = propDefinition.Name;
            defAttributes = propDefinition.CustomAttributes;
            defHasAttributes = propDefinition.HasCustomAttributes;
            defIsSpecialName = propDefinition.IsSpecialName;
            defIsRuntimeSpecialName = propDefinition.IsRuntimeSpecialName;
            defDeclaringType = propDefinition.DeclaringType;
        }

        public MemberReference GetMethod 
        {
            get => this.DeclaringType.GetMember(
                $"Get{AttachedEntitiesHelper.GetPropertyName(defName)}", 
                m => (m as MethodReference)?.Parameters.Count == 1);
        }
        public MemberReference SetMethod
        {
            get => this.DeclaringType.GetMember(
                $"Set{AttachedEntitiesHelper.GetPropertyName(defName)}",
                m => (m as MethodReference)?.Parameters.Count == 2);
        }

        public Collection<CustomAttribute> CustomAttributes => defAttributes;
        public bool HasCustomAttributes => defHasAttributes;

        public bool IsSpecialName
        {
            get { return defIsSpecialName; }
            set { defIsSpecialName = value; }
        }

        public bool IsRuntimeSpecialName
        {
            get { return defIsRuntimeSpecialName; }
            set { defIsRuntimeSpecialName = value; }
        }

        public new TypeDefinition DeclaringType
        {
            get { return defDeclaringType; }
            set { defDeclaringType = value; }
        }
    }
}