using Mono.Cecil;
using Mono.Documentation.Util;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mono.Documentation.Updater
{
    public class AttributeParserContext : IAttributeParserContext
    {
        private int nullableAttributeIndex;
        private int dynamicAttributeIndex;
        private ICustomAttributeProvider provider;
        private ReadOnlyCollection<bool?> nullableAttributeFlags;
        private ReadOnlyCollection<bool> dynamicAttributeFlags;

        public AttributeParserContext(ICustomAttributeProvider provider)
        {
            this.provider = provider;

            ReadDynamicAttribute();
            ReadNullableAttribute();
        }

        private bool ExistsNullableAttribute
        {
            get
            {
                return nullableAttributeFlags?.Count > 0;
            }
        }

        private bool HasSameNullableValue
        {
            get
            {
                return nullableAttributeFlags.Count == 1;
            }
        }

        public void MoveToNextDynamicFlag()
        {
            dynamicAttributeIndex++;
        }

        public bool IsDynamic()
        {
            return dynamicAttributeFlags != null && (dynamicAttributeFlags.Count == 0 || dynamicAttributeFlags[dynamicAttributeIndex]);
        }

        public bool IsNullable()
        {
            if (ExistsNullableAttribute)
            {
                if (HasSameNullableValue)
                {
                    return nullableAttributeFlags[0].IsTrue();
                }

                if (nullableAttributeIndex < nullableAttributeFlags?.Count)
                {
                    return nullableAttributeFlags[nullableAttributeIndex++].IsTrue();
                }

                throw new IndexOutOfRangeException("You are out of range in the nullable attribute values, please call the method for each nullable checking only once.");
            }

            return false;
        }

        private void ReadDynamicAttribute()
        {
            if (provider.HasCustomAttributes)
            {
                CustomAttribute dynamicAttribute = provider.CustomAttributes.SafeCast<CustomAttribute>().SingleOrDefault(ca => ca.GetDeclaringType() == "System.Runtime.CompilerServices.DynamicAttribute");
                if (dynamicAttribute != null)
                {
                    CustomAttributeArgument[] attributeValues = new CustomAttributeArgument[0];
                    if (dynamicAttribute.ConstructorArguments.Count > 0)
                    {
                        attributeValues = (CustomAttributeArgument[])dynamicAttribute.ConstructorArguments[0].Value;
                    }

                    dynamicAttributeFlags = new ReadOnlyCollection<bool>(attributeValues.Select(t => (bool)t.Value).ToList());
                }
            }
        }

        private void ReadNullableAttribute()
        {
            NullableReferenceTypeProvider nullableReferenceTypeProvider = new NullableReferenceTypeProvider(provider);
            nullableAttributeFlags = new ReadOnlyCollection<bool?>(nullableReferenceTypeProvider.GetNullableReferenceTypeFlags());
        }
    }
}