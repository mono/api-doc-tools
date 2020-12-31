using Mono.Documentation.Updater.Formatters;
using NUnit.Framework;

namespace mdoc.Test
{
    public class NullableReferenceTypesTests : BasicFormatterTests<CSharpMemberFormatter>
    {
        private const string NullableReferenceTypesAssemblyPath = "../../../../external/Test/mdoc.Test.NullableReferenceTypes.dll";

        private CSharpMemberFormatter csharpMemberFormatter = new CSharpMemberFormatter();

        protected override CSharpMemberFormatter formatter => csharpMemberFormatter;

        [TestCase("NullableInterfaceOfNullableObject", "ICollection<string?>? list, ICollection<int?>? args")]
        [TestCase("NullableInterface", "ICollection<string>? list, ICollection<int>? args")]
        [TestCase("OneNullableAndTowNonNullable", "string s, object? o, string x")]
        [TestCase("TwoDimensionalNullableArrayOfNullableObject", "object?[]?[]? args")]
        [TestCase("TwoDimensionalNullableArray", "object[]?[]? args")]
        [TestCase("NullableArrayOfNullableObject", "object?[]? args")]
        [TestCase("NullableArray", "object?[] args")]
        [TestCase("TwoNullable", "string? s, object? o")]
        [TestCase("NonNullable", "string s")]
        [TestCase("NullableInt", "int? i")]
        [TestCase("ArrayOfNullableInt", "int?[] array")]
        [TestCase("NullableDelegateOfNullableObject", "Action<object?> callback")]
        [TestCase("NullableDelegate", "Action<object>? callback")]
        [TestCase("ParamsArrayOfNullableObjest", "params object?[] args")]
        [TestCase("DictionaryOfReferenceType", "Dictionary<string, string> dictionary")]
        [TestCase("NullableDictionaryOfNullableValue", "Dictionary<string, string?>? dictionary")]
        [TestCase("NullableDictionaryOfNullableKeyAndValue", "Dictionary<string?, string?>? dictionary")]
        [TestCase("DictionaryOfValueType", "Dictionary<int, int> dictionary")]
        [TestCase("NullableDictionaryOfNullableValueTypeValue", "Dictionary<int, int?>? dictionary")]
        [TestCase("NullableDictionaryOfNullableValueTypeAndValue", "Dictionary<int?, int?>? dictionary")]
        [TestCase("DictionaryOfValueTypeAndNullableString", "Dictionary<int, string?> dictionary")]
        [TestCase("NullableDictionaryOfValueTypeAndNullableString", "Dictionary<int, string?>? dictionary")]
        [TestCase("NullableDictionaryOfNullableDictionrayOfValueTypeAndNullableString", "Dictionary<int, Dictionary<int, string?>?>? dictionary")]
        [TestCase("NullableDictionaryOfNullableDictionrayOfReferenceTypeAndNullableString", "Dictionary<int, Dictionary<string, string?>?>? dictionary")]
        [TestCase("NullableDictionaryOfNullableDictionrayOfNullableValueTypeValue", "Dictionary<int, Dictionary<int, int?>?>? dictionary")]
        [TestCase("NonNullableDynamicType", "dynamic value")]
        [TestCase("NullableDynamicType", "dynamic? value")]
        public void TestMethod_Parameter(string methodName, string methodParameterSignature)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.MethodParameter",
                methodName, $"public void {methodName} ({methodParameterSignature});");
        }

        [TestCase("GenericNullableReference", "public void GenericNullableReference<T1,T2,T3> (T1? t1, T2 t2, T3? t3) where T1 : class where T3 : class;")]
        [TestCase("GenericNullableValueType", "public void GenericNullableValueType<T1,T2,T3> (T1? t1, T2 t2, T3? t3) where T1 : struct where T3 : struct;")]
        public void TestMethod_GenericTypeParameter(string methodName, string methodSignature)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.MethodParameter",
                methodName, methodSignature);
        }

        [TestCase("NullableString", "string?")]
        [TestCase("NullableInt", "int?")]
        [TestCase("NullableInterfaceOfNullableObject", "ICollection<object?>?")]
        [TestCase("NullableInterface", "ICollection<object>?")]
        [TestCase("NullableArray", "string[]?")]
        [TestCase("NullableArrayOfNullableObject", "object?[]?")]
        [TestCase("NullableDictionaryOfNullableDictionrayOfNullableValueTypeValue", "Dictionary<int, Dictionary<int, int?>?>?")]
        [TestCase("NullableDictionaryOfNullableDictionrayOfReferenceTypeAndNullableString", "Dictionary<int, Dictionary<string, string?>?>?")]
        [TestCase("NullableDictionaryOfNullableDictionrayOfValueTypeAndNullableString", "Dictionary<int, Dictionary<int, string?>?>?")]
        [TestCase("NonNullableDynamicType", "dynamic")]
        [TestCase("NullableDynamicType", "dynamic?")]
        public void TestMethod_ReturnType(string methodName, string returnTypeSignature)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.MethodReturnType",
                methodName, $"public {returnTypeSignature} {methodName} ();");
        }

        [TestCase("NonNullableProperty", "object")]
        [TestCase("NullableProperty", "object?")]
        [TestCase("NonNullableIntArray", "int[]")]
        [TestCase("ArrayOfNullableInt", "int?[]")]
        [TestCase("NullableIntArray", "int[]?")]
        [TestCase("NullableArrayOfNullableInt", "int?[]?")]
        [TestCase("NonNullableArray", "object[]")]
        [TestCase("ArrayOfNullableObject", "object?[]")]
        [TestCase("NullableArray", "object[]?")]
        [TestCase("NullableArrayOfNullableObject", "object?[]?")]
        [TestCase("NullableInterface", "ICollection<string>?")]
        [TestCase("InterfaceOfNullableString", "ICollection<string?>")]
        [TestCase("NullableInterfaceOfNullableString", "ICollection<string?>?")]
        [TestCase("NonNullableDynamicType", "dynamic")]
        [TestCase("NullableDynamicType", "dynamic?")]
        public void TestProperty_ReturnType(string propertyName, string propertyReturnTypeSignature)
        {
            TestPropertySignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.PropertyReturnType",
                propertyName, $"public {propertyReturnTypeSignature} {propertyName} {{ get; }}");
        }

        [TestCase("NonNullableEventHandler", "EventHandler")]
        [TestCase("NullableEventHandler", "EventHandler?")]
        [TestCase("NonNullableGenericEventHandler", "EventHandler<EventArgs>")]
        [TestCase("GenericEventHandlerOfNullableEventArgs", "EventHandler<EventArgs?>")]
        [TestCase("NullableGenericEventHandler", "EventHandler<EventArgs>?")]
        [TestCase("NullableGenericEventHandlerOfNullableEventArgs", "EventHandler<EventArgs?>?")]
        public void TestEvent_EventType(string eventName, string eventType)
        {
            TestEventSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.Event",
                eventName, $"public event {eventType} {eventName};");
        }

        [TestCase("NonNullable", "object sender, EventArgs args")]
        [TestCase("NullableEventArgs", "object sender, EventArgs? args")]
        [TestCase("NullableSenderAndEventArgs", "object? sender, EventArgs? args")]
        public void TestDelegate_Parameter(string delegateName, string delegateParameterSignature)
        {
            TestTypeSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.{delegateName}",
                $"public delegate void {delegateName}({delegateParameterSignature});");
        }
    }
}
