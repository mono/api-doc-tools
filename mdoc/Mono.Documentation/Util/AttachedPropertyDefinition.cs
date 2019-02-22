using Mono.Cecil;
using Mono.Collections.Generic;

namespace Mono.Documentation.Util
{
    public class AttachedPropertyDefinition : AttachedPropertyReference, IMemberDefinition
    {
        private readonly FieldDefinition fieldDefinition;

        public AttachedPropertyDefinition(FieldDefinition fieldDefinition, MetadataToken metadataToken) : base(fieldDefinition)
        {
            this.fieldDefinition = fieldDefinition;
            MetadataToken = metadataToken;
        }

        public MemberReference GetMethod 
        {
            get => this.DeclaringType.GetMember(
                $"Get{AttachedEntitiesHelper.GetPropertyName(fieldDefinition.Name)}", 
                m => (m as MethodReference)?.Parameters.Count == 1);
        }
        public MemberReference SetMethod
        {
            get => this.DeclaringType.GetMember(
                $"Set{AttachedEntitiesHelper.GetPropertyName(fieldDefinition.Name)}",
                m => (m as MethodReference)?.Parameters.Count == 2);
        }

        public Collection<CustomAttribute> CustomAttributes => fieldDefinition.CustomAttributes;
        public bool HasCustomAttributes => fieldDefinition.HasCustomAttributes;

        public bool IsSpecialName
        {
            get { return fieldDefinition.IsSpecialName; }
            set { fieldDefinition.IsSpecialName = value; }
        }

        public bool IsRuntimeSpecialName
        {
            get { return fieldDefinition.IsRuntimeSpecialName; }
            set { fieldDefinition.IsRuntimeSpecialName = value; }
        }

        public new TypeDefinition DeclaringType
        {
            get { return fieldDefinition.DeclaringType; }
            set { fieldDefinition.DeclaringType = value; }
        }
    }
}