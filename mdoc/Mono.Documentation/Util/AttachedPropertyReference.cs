using Mono.Cecil;

namespace Mono.Documentation.Util
{
    public class AttachedPropertyReference : FieldReference
    {
        private readonly FieldDefinition fieldDefinition;
        private AttachedPropertyDefinition definition;

        public AttachedPropertyReference(FieldDefinition fieldDefinition) : base(AttachedEntitiesHelper.GetPropertyName(fieldDefinition.Name), fieldDefinition.FieldType, fieldDefinition.DeclaringType)
        {
            this.fieldDefinition = fieldDefinition;
        }

        protected override IMemberDefinition ResolveDefinition()
        {
            return definition ??
                   (definition = new AttachedPropertyDefinition(fieldDefinition, MetadataToken));
        }
    }
}