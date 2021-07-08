using Mono.Cecil;
using Mono.Documentation.Updater;
using NUnit.Framework;

namespace mdoc.Test
{
    public class DocTypeNullableReferenceTypesTests : BasicFormatterTests<MemberFormatter>
    {
        private const string NullableReferenceTypesAssemblyPath = "../../../../external/Test/mdoc.Test.NullableReferenceTypes.dll";

        protected override MemberFormatter formatter => DocTypeFullMemberFormatter.Default;

        [TestCase("System.Int32", "ValueType")]
        [TestCase("System.Nullable<System.Int32>", "NullableValueType")]
        [TestCase("System.ValueTuple<System.Int32,System.Int32>", "ValueTupleOfValueType")]
        [TestCase("System.Nullable<System.ValueTuple<System.Int32,System.Int32>>", "NullableValueTupleOfValueType")]
        [TestCase("System.Collections.Generic.Dictionary<System.String,System.Collections.Generic.Dictionary<System.String,System.String>>", "DictionaryOfReferenceTypeKeyAndDictionaryOfReferenceTypeValue")]
        [TestCase("System.Collections.Generic.Dictionary<System.String?,System.Collections.Generic.Dictionary<System.String?,System.String?>?>?", "NullableDictionaryOfNullableReferenceTypeKeyAndNullableDictionaryOfNullableReferenceTypeValue")]
        public void DocTypeReturnType(string returnType, string methodName)
        {
            // This is a common test for the return type of method, extension method, property, field, and operator overloading.
            // They have the same process logic that we just test once the type of them.
            var type = GetType(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.CommonType");
            var method = GetMethod(type, i => i.Name == methodName);

            var typeName = GetDocTypeName(method.MethodReturnType, method.ReturnType);

            Assert.AreEqual(returnType, typeName);
        }

        [TestCase("System.EventHandler", "EventHandler")]
        [TestCase("System.EventHandler<System.EventArgs>?", "NullableGenericEventHandler")]
        [TestCase("System.EventHandler<System.EventArgs?>?", "NullableGenericEventHandlerOfNullableEventArgs")]
        public void DocTypeEventType(string returnType, string methodName)
        {
            var type = GetType(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.Event");
            var @event = GetEvent(type, methodName) as EventDefinition;

            var typeName = GetDocTypeName(@event, @event.EventType);

            Assert.AreEqual(returnType, typeName);
        }

        [TestCase("NullableAndNonNullableValueType", "System.Int32", "System.Nullable<System.Int32>", "System.Int32")]
        [TestCase("NullableAndNonNullableReferenceType", "System.String", "System.String?", "System.String")]
        [TestCase("NullableAndNonNullableInterfaceOfValueType", "System.Collections.Generic.ICollection<System.Int32>", "System.Collections.Generic.ICollection<System.Int32>?", "System.Collections.Generic.ICollection<System.Int32>")]
        [TestCase("NullableAndNonNullableInterfaceOfReferenceType", "System.Collections.Generic.ICollection<System.String>", "System.Collections.Generic.ICollection<System.String>?", "System.Collections.Generic.ICollection<System.String>")]
        public void DocTypeParameterType(string methodName, params string[] methodParameterType)
        {
            // This is a common test for the parameter type of constructor, method, extension method, delegate and operator overloading.
            // They have the same process logic that we just test once the type of them.
            var type = GetType(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.MethodParameter");
            var method = GetMethod(type, i => i.Name == methodName);

            for (int i = 0; i < method.Parameters.Count; i++)
            {
                var methodParameter = method.Parameters[i];
                var expectedParameterType = methodParameterType[i];

                var typeName = GetDocTypeName(methodParameter, methodParameter.ParameterType);

                Assert.AreEqual(expectedParameterType, typeName);
            }
        }

        private string GetDocTypeName(ICustomAttributeProvider provider, TypeReference type)
        {
            var context = AttributeParserContext.Create(provider);
            var isNullable = context.IsNullable();
            var typeName = DocTypeFullMemberFormatter.Default.GetName(type, context, useTypeProjection: false);

            if (isNullable)
            {
                typeName += DocUtils.GetTypeNullableSymbol(type, isNullable);
            }

            return typeName;
        }
    }
}
