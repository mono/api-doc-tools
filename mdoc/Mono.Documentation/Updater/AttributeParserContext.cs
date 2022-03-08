using mdoc.Mono.Documentation.Updater;
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
        private int tupleNameAttributeIndex;
        private ICustomAttributeProvider provider;
        private ReadOnlyCollection<bool?> nullableAttributeFlags;
        private ReadOnlyCollection<bool> dynamicAttributeFlags;
        private string[] tupleElementNames;

        private AttributeParserContext(ICustomAttributeProvider provider)
        {
            this.provider = provider;

            ReadDynamicAttribute();
            ReadNullableAttribute();
            ReadTupleElementNames();
        }

        private bool ExistsNullableAttribute
        {
            get
            {
                return nullableAttributeFlags.Count > 0;
            }
        }

        private bool HasSameNullableValue
        {
            get
            {
                return nullableAttributeFlags.Count == 1;
            }
        }

        public static IAttributeParserContext Create(ICustomAttributeProvider provider)
        {
            return new AttributeParserContext(provider);
        }

        public void NextDynamicFlag()
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

                if (nullableAttributeIndex < nullableAttributeFlags.Count)
                {
                    return nullableAttributeFlags[nullableAttributeIndex++].IsTrue();
                }

                throw new IndexOutOfRangeException("You are out of range in the nullable attribute values, please call the method for each nullable checking only once.");
            }

            return false;
        }

        public string GetTupleElementName()
        {
            return (tupleElementNames == null || tupleNameAttributeIndex >= tupleElementNames.Length) ? null : tupleElementNames[tupleNameAttributeIndex++];
        }

        private void ReadDynamicAttribute()
        {
            DynamicTypeProvider dynamicTypeProvider = new DynamicTypeProvider(provider);
            var dynamicTypeFlags = dynamicTypeProvider.GetDynamicTypeFlags();
            if (dynamicTypeFlags != null)
            {
                dynamicAttributeFlags = new ReadOnlyCollection<bool>(dynamicTypeFlags);
            }
        }

        private void ReadNullableAttribute()
        {
            NullableReferenceTypeProvider nullableReferenceTypeProvider = new NullableReferenceTypeProvider(provider);
            nullableAttributeFlags = new ReadOnlyCollection<bool?>(nullableReferenceTypeProvider.GetNullableReferenceTypeFlags());
        }

        private void ReadTupleElementNames()
        {
            if (provider != null && provider.HasCustomAttributes)
            {
                var tupleNamesAttr = provider.CustomAttributes.Where(attr => attr.AttributeType.FullName == Consts.TupleElementNamesAttribute).FirstOrDefault();
                if (tupleNamesAttr != null)
                {
                    var constructorArgs = tupleNamesAttr.ConstructorArguments.FirstOrDefault().Value as CustomAttributeArgument[];
                    tupleElementNames = constructorArgs?.Select(arg => arg.Value as string).ToArray();
                }
            }
        }
    }
}