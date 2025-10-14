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
        private int nativeIntegerAttributeIndex;
        private ICustomAttributeProvider provider;
        private ReadOnlyCollection<bool?> nullableAttributeFlags;
        private ReadOnlyCollection<bool> dynamicAttributeFlags;
        private string[] tupleElementNames;
        private bool[] nativeIntegerFlags;

        private AttributeParserContext(ICustomAttributeProvider provider)
        {
            this.provider = provider;

            ReadDynamicAttribute();
            ReadNullableAttribute();
            ReadTupleElementNames();
            ReadNativeIntegerAttribute();
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

        public bool IsNativeInteger()
        {
            return nativeIntegerFlags != null && nativeIntegerAttributeIndex < nativeIntegerFlags.Length && nativeIntegerFlags[nativeIntegerAttributeIndex++];
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
            tupleElementNames = ReadCustomAttributeValue<string>(Consts.TupleElementNamesAttribute);
        }

        private void ReadNativeIntegerAttribute()
        {
            nativeIntegerFlags = ReadCustomAttributeValue<bool>(
                Consts.NativeIntegerAttribute,
                () => new bool[] { true });
        }

        private T[] ReadCustomAttributeValue<T>(string attributeName, Func<T[]> init = null)
        {
            if (provider == null || !provider.HasCustomAttributes) return null;

            var customAttribute = provider.CustomAttributes.Where(attr => attr.AttributeType.FullName == attributeName).FirstOrDefault();

            if (customAttribute == null) return null;

            if (!customAttribute.HasConstructorArguments) return init?.Invoke();

            var constructorArgs = customAttribute.ConstructorArguments[0].Value as CustomAttributeArgument[];
            return constructorArgs?.Select(arg => (T)arg.Value).ToArray();
        }
    }
}