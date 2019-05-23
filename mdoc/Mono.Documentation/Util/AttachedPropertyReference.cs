using Mono.Cecil;

namespace Mono.Documentation.Util
{
    public class AttachedPropertyReference : FieldReference
    {
        private readonly FieldDefinition fieldDefinition;
        private readonly PropertyDefinition propDefinition;
        private AttachedPropertyDefinition definition;

        public AttachedPropertyReference(FieldDefinition fieldDefinition) : base(AttachedEntitiesHelper.GetPropertyName(fieldDefinition.Name), fieldDefinition.FieldType, fieldDefinition.DeclaringType)
        {
            this.fieldDefinition = fieldDefinition;
        }
        public AttachedPropertyReference(PropertyDefinition propDefinition) : base(AttachedEntitiesHelper.GetPropertyName(propDefinition.Name), propDefinition.PropertyType, propDefinition.DeclaringType)
        {
            this.propDefinition = propDefinition;
        }

        protected override IMemberDefinition ResolveDefinition()
        {
            if (definition == null)
            {
                if (fieldDefinition != null)
                    definition = new AttachedPropertyDefinition(fieldDefinition, MetadataToken);

                if (propDefinition != null)
                    definition = new AttachedPropertyDefinition(propDefinition, MetadataToken);
            }
            return definition;
        }
    }
}