using Mono.Documentation.Updater.Formatters;
using NUnit.Framework;

namespace mdoc.Test
{
    public class NullableReferenceTypesTests : BasicFormatterTests<CSharpMemberFormatter>
    {
        private const string NullableReferenceTypesAssemblyPath = "../../../../external/Test/mdoc.Test.NullableReferenceTypes.dll";

        private CSharpMemberFormatter csharpMemberFormatter = new CSharpMemberFormatter();

        protected override CSharpMemberFormatter formatter => csharpMemberFormatter;

        [TestCase("dynamic", "DynamicType")]
        [TestCase("dynamic?", "NullableDynamicType")]
        [TestCase("DayOfWeek", "Enumeration")]
        [TestCase("DayOfWeek?", "NullableEnumeration")]
        [TestCase("int", "ValueType")]
        [TestCase("int?", "NullableValueType")]
        [TestCase("int[]", "ArrayOfValueType")]
        [TestCase("int?[]", "ArrayOfValueTypeNullable")]
        [TestCase("int[]?", "NullableArrayOfValueType")]
        [TestCase("int?[]?", "NullableArrayOfNullableValueType")]
        [TestCase("int[][]", "DimensionalArrayOfValueType")]
        [TestCase("int?[][]", "DimensionalArrayOfNullableValueType")]
        [TestCase("int?[]?[]", "DimensionalArrayOfNullableValueTypeOfNullableRow")]
        [TestCase("int?[]?[]?", "NullableDimensionalArrayOfNullableValueTypeOfNullableRow")]
        [TestCase("int?[][]?", "NullableDimensionalArrayOfNullableValueType")]
        [TestCase("int[][]?", "NullableDimensionalArrayOfValueType")]
        [TestCase("int[][]?[][]?", "NullableFourDimensionalArrayOfValueTypeOfMiddleNullableArray")]
        [TestCase("int?[][]?[][]", "FourDimensionalArrayOfNullableValueTypeOfMiddleNullableArray")]
        [TestCase("ICollection<int>", "InterfaceOfValueType")]
        [TestCase("ICollection<int>?", "NullableInterfaceOfValueType")]
        [TestCase("ICollection<int?>?", "NullableInterfaceOfNullableValueType")]
        [TestCase("Action<int>", "ActionOfValueType")]
        [TestCase("Action<int?>", "ActionOfNullableValueType")]
        [TestCase("Action<int>?", "NullableActionOfValueType")]
        [TestCase("Action<int?>?", "NullableActionOfNullableValueType")]
        [TestCase("Dictionary<int, int>", "DictionaryOfValueType")]
        [TestCase("Dictionary<int, int>?", "NullableDictionaryOfValueType")]
        [TestCase("Dictionary<int?, int?>", "DictionaryOfNullableValueType")]
        [TestCase("Dictionary<int?, int?>?", "NullableDictionaryOfNullableValueType")]
        [TestCase("Dictionary<int, int?>", "DictionaryOfNullableValueTypeValue")]
        [TestCase("Dictionary<int, int?>?", "NullableDictionaryOfNullableValueTypeValue")]
        [TestCase("Dictionary<int?, int>?", "NullableDictionaryOfNullableValueTypeKey")]
        [TestCase("Dictionary<int, Dictionary<int, int>>", "DictionaryOfValueTypeKeyAndDictionaryOfValueTypeValue")]
        [TestCase("Dictionary<int, Dictionary<int, int>>?", "NullableDictionaryOfValueTypeKeyAndDictionaryOfValueTypeValue")]
        [TestCase("Dictionary<int?, Dictionary<int, int>?>?", "NullableDictionaryOfNullableValueTypeKeyAndNullableDictionaryOfValueTypeValue")]
        [TestCase("Dictionary<int?, Dictionary<int?, int?>?>?", "NullableDictionaryOfNullableValueTypeKeyAndNullableDictionaryOfNullableValueTypeValue")]
        [TestCase("Dictionary<Dictionary<int, int>, Dictionary<int, int>>", "DictionaryOfDictionaryOfValueType")]
        [TestCase("Dictionary<Dictionary<int, int>?, Dictionary<int, int>?>?", "NullableDictionaryOfNullableDictionaryOfValueType")]
        [TestCase("Dictionary<Dictionary<int?, int?>?, Dictionary<int?, int?>?>?", "NullableDictionaryOfNullableDictionaryOfNullableValueType")]
        [TestCase("string", "ReferenceType")]
        [TestCase("string?", "NullableReferenceType")]
        [TestCase("string[]", "ArrayOfReferenceType")]
        [TestCase("string?[]", "ArrayOfNullableReferenceType")]
        [TestCase("string[]?", "NullableArrayOfReferenceType")]
        [TestCase("string?[]?", "NullableArrayOfNullableReferenceType")]
        [TestCase("string[][]", "DimensionalArrayOfReferenceType")]
        [TestCase("string?[][]", "DimensionalArrayOfNullableReferenceType")]
        [TestCase("string?[]?[]", "DimensionalArrayOfNullableReferenceTypeOfNullableRow")]
        [TestCase("string?[]?[]?", "NullableDimensionalArrayOfNullableReferenceTypeOfNullableRow")]
        [TestCase("string?[][]?", "NullableDimensionalArrayOfNullableReferenceType")]
        [TestCase("string[][]?", "NullableDimensionalArrayOfReferenceType")]
        [TestCase("string[][]?[][]?", "NullableFourDimensionalArrayOfReferenceTypeOfMiddleNullableArray")]
        [TestCase("string?[][]?[][]", "FourDimensionalArrayOfNullableReferenceTypeOfMiddleNullableArray")]
        [TestCase("ICollection<string>", "InterfaceOfReferenceType")]
        [TestCase("ICollection<string>?", "NullableInterfaceOfReferenceType")]
        [TestCase("ICollection<string?>?", "NullableInterfaceOfNullableReferenceType")]
        [TestCase("Action<string>", "ActionOfReferenceType")]
        [TestCase("Action<string?>", "ActionOfNullableReferenceType")]
        [TestCase("Action<string>?", "NullableActionOfReferenceType")]
        [TestCase("Action<string?>?", "NullableActionOfNullableReferenceType")]
        [TestCase("Dictionary<string, string>", "DictionaryOfReferenceType")]
        [TestCase("Dictionary<string, string>?", "NullableDictionaryOfReferenceType")]
        [TestCase("Dictionary<string?, string?>", "DictionaryOfNullableReferenceType")]
        [TestCase("Dictionary<string?, string?>?", "NullableDictionaryOfNullableReferenceType")]
        [TestCase("Dictionary<string, string?>", "DictionaryOfNullableReferenceTypeValue")]
        [TestCase("Dictionary<string, string?>?", "NullableDictionaryOfNullableReferenceTypeValue")]
        [TestCase("Dictionary<string?, string>?", "NullableDictionaryOfNullableReferenceTypeKey")]
        [TestCase("Dictionary<Dictionary<string, string>, Dictionary<string, string>>", "DictionaryOfDictionaryOfReferenceType")]
        [TestCase("Dictionary<Dictionary<string, string>?, Dictionary<string, string>?>?", "NullableDictionaryOfNullableDictionaryOfReferenceType")]
        [TestCase("Dictionary<Dictionary<string?, string?>?, Dictionary<string?, string?>?>?", "NullableDictionaryOfNullableDictionaryOfNullableReferenceType")]
        [TestCase("Dictionary<string, Dictionary<string, string>>", "DictionaryOfReferenceTypeKeyAndDictionaryOfReferenceTypeValue")]
        [TestCase("Dictionary<string, Dictionary<string, string>>?", "NullableDictionaryOfReferenceTypeKeyAndDictionaryOfReferenceTypeValue")]
        [TestCase("Dictionary<string?, Dictionary<string, string>?>?", "NullableDictionaryOfNullableReferenceTypeKeyAndNullableDictionaryOfReferenceTypeValue")]
        [TestCase("Dictionary<string?, Dictionary<string?, string?>?>?", "NullableDictionaryOfNullableReferenceTypeKeyAndNullableDictionaryOfNullableReferenceTypeValue")]
        public void TestCommonType(string returnType, string methodName)
        {
            // Test single parameter and return type for method, extension method, property, field, event, delegate.
            // They have the same processing logic that we just test once the type of them.
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.CommonType",
                methodName, $"public {returnType} {methodName} ();");
        }

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
        [TestCase("NonNullableDelegateOfNullableObject", "Action<object?> callback")]
        [TestCase("NullableDelegateOfNullableObject", "Action<object?>? callback")]
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
        public void TestMethod_Parameter(string methodName, string methodParameter)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.MethodParameter",
                methodName, $"public void {methodName} ({methodParameter});");
        }

        [TestCase("GenericType", "<T> ({0})", "T t")]
        [TestCase("GenericReferenceType", "<T> ({0}) where T : class", "T t")]
        [TestCase("GenericNullableReferenceType", "<T> ({0}) where T : class", "T? t")]
        [TestCase("ActionOfGenericNullableReferenceType", "<T> ({0}) where T : class", "Action<T?> t")]
        [TestCase("NullableActionOfGenericNullableReferenceType", "<T> ({0}) where T : class", "Action<T?>? t")]
        [TestCase("FuncGenericNullableReferenceType", "<T> ({0}) where T : class", "Func<T?> t")]
        [TestCase("NullableFuncGenericNullableReferenceType", "<T> ({0}) where T : class", "Func<T?>? t")]
        [TestCase("GenericNonNullableAndNullableReferenceType", "<T1,T2> ({0}) where T2 : class", "T1 t1, T2? t2")]
        [TestCase("GenericValueType", "<T> ({0}) where T : struct", "T t")]
        [TestCase("GenericNullableValueType", "<T> ({0}) where T : struct", "T? t")]
        [TestCase("ActionOfGenericNullableValueType", "<T> ({0}) where T : struct", "Action<T?> action")]
        [TestCase("NullableActionOfGenericNullableValueType", "<T> ({0}) where T : struct", "Action<T?>? action")]
        [TestCase("FuncGenericNullableValueType", "<T> ({0}) where T : struct", "Func<T?> func")]
        [TestCase("NullableFuncGenericNullableValueType", "<T> ({0}) where T : struct", "Func<T?>? func")]
        [TestCase("GenericNonNullableAndNullableValueType", "<T1,T2> ({0}) where T2 : struct", "T1 t1, T2? t2")]
        public void TestGenericMethod_TypeParameter(string methodName, string otherPartOfMethodSignature, string genericParameter)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.GenericMethodParameter",
                methodName, $"public void {methodName}{string.Format(otherPartOfMethodSignature, genericParameter)};");
        }

        [TestCase("string?", "NullableReferenceType")]
        [TestCase("int?", "NullableValueType")]
        [TestCase("ICollection<object?>?", "NullableInterfaceOfNullableReferenceType")]
        [TestCase("ICollection<object>?", "NullableInterface")]
        [TestCase("string[]?", "NullableArray")]
        [TestCase("object?[]?", "NullableArrayOfNullableReferenceType")]
        [TestCase("Dictionary<int, Dictionary<int, int?>?>?", "NullableDictionaryOfNullableDictionrayOfNullableValueTypeValue")]
        [TestCase("Dictionary<int, Dictionary<string, string?>?>?", "NullableDictionaryOfNullableDictionrayOfReferenceTypeKeyAndNullableReferenceTypeValue")]
        [TestCase("Dictionary<int, Dictionary<int, string?>?>?", "NullableDictionaryOfNullableDictionrayOfValueTypeKeyAndNullableReferenceTypeValue")]
        [TestCase("dynamic", "NonNullableDynamicType")]
        [TestCase("dynamic?", "NullableDynamicType")]
        public void TestMethod_ReturnType(string returnType, string methodName)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.MethodReturnType",
                methodName, $"public {returnType} {methodName} ();");
        }

        [TestCase("T", "GenericType", "<T> ()")]
        [TestCase("T", "GenericReferenceType", "<T> () where T : class")]
        [TestCase("T?", "GenericNullableReferenceType", "<T> () where T : class")]
        [TestCase("T", "GenericValueType", "<T> () where T : struct")]
        [TestCase("T?", "GenericNullableValueType", "<T> () where T : struct")]
        public void TestGenericMethod_ReturnType(string returnType, string methodName, string otherPartOfMethodSignature)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.GenericMethodReturnType",
                methodName, $"public {returnType} {methodName}{otherPartOfMethodSignature};");
        }

        [TestCase("object", "ReferenceType")]
        [TestCase("object?", "NullableReferenceType")]
        [TestCase("int[]", "ArrayOfValueType")]
        [TestCase("int?[]", "ArrayOfNullableValueType")]
        [TestCase("int[]?", "NullableArrayOfValueType")]
        [TestCase("int?[]?", "NullableArrayOfNullableValueType")]
        [TestCase("object[]", "ReferenceTypeArray")]
        [TestCase("object?[]", "ArrayOfNullableReferenceType")]
        [TestCase("object[]?", "NullableArrayOfReferenceType")]
        [TestCase("object?[]?", "NullableArrayOfNullableReferenceType")]
        [TestCase("ICollection<string>", "InterfaceOfReferenceType")]
        [TestCase("ICollection<string>?", "NullableInterfaceOfReferenceType")]
        [TestCase("ICollection<string?>", "InterfaceOfNullableReferenceType")]
        [TestCase("ICollection<string?>?", "NullableInterfaceOfNullableReferenceType")]
        [TestCase("ICollection<int>", "InterfaceOfValueType")]
        [TestCase("ICollection<int>?", "NullableInterfaceOfValueType")]
        [TestCase("ICollection<int?>", "InterfaceOfNullableValueType")]
        [TestCase("ICollection<int?>?", "NullableInterfaceOfNullableValueType")]
        [TestCase("dynamic", "NonNullableDynamicType")]
        [TestCase("dynamic?", "NullableDynamicType")]
        public void TestProperty_ReturnType(string returnType, string propertyName)
        {
            TestPropertySignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.PropertyReturnType",
                propertyName, $"public {returnType} {propertyName} {{ get; }}");
        }

        [TestCase("T", "GenericType")]
        [TestCase("TClass", "GenericReferenceType")]
        [TestCase("TClass?", "GenericNullableReferenceType")]
        [TestCase("TStruct", "GenericValueType")]
        [TestCase("TStruct?", "GenericNullableValueType")]
        public void TestGenericProperty_ReturnType(string returnType, string propertyName)
        {
            TestPropertySignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.GenericPropertyReturnType`3",
                propertyName, $"public {returnType} {propertyName} {{ get; }}");
        }

        [TestCase("EventHandler", "EventHandler")]
        [TestCase("EventHandler?", "NullableEventHandler")]
        [TestCase("EventHandler<EventArgs>", "GenericEventHandler")]
        [TestCase("EventHandler<EventArgs?>", "GenericEventHandlerOfNullableEventArgs")]
        [TestCase("EventHandler<EventArgs>?", "NullableGenericEventHandler")]
        [TestCase("EventHandler<EventArgs?>?", "NullableGenericEventHandlerOfNullableEventArgs")]
        public void TestEvent_EventType(string eventType, string eventName)
        {
            TestEventSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.Event",
                eventName, $"public event {eventType} {eventName};");
        }

        [TestCase("Handler", "object sender, EventArgs args")]
        [TestCase("NullableSender", "object? sender, EventArgs args")]
        [TestCase("NullableSenderAndEventArgs", "object? sender, EventArgs? args")]
        public void TestDelegate_Parameter(string delegateName, string delegateParameter)
        {
            TestTypeSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.Delegate.{delegateName}",
                $"public delegate void {delegateName}({delegateParameter});");
        }

        [TestCase("GenericHandler<TEventArgs>", "object sender, TEventArgs args", "{0}", "GenericHandler`1")]
        [TestCase("NullableSender<TEventArgs>", "object? sender, TEventArgs args", "{0}", "NullableSender`1")]
        [TestCase("NullableSenderAndEventArgs<TEventArgs>", "object? sender, TEventArgs? args", "{0} where TEventArgs : class", "NullableSenderAndEventArgs`1")]
        [TestCase("ActionHandler<TClass, TStruct>", "TClass t1, TStruct t2", "{0} where TClass : class where TStruct : struct", "ActionHandler`2")]
        [TestCase("NullableActionHandler<TClass, TStruct>", "TClass? t1, TStruct? t2", "{0} where TClass : class where TStruct : struct", "NullableActionHandler`2")]
        public void TestGenericDelegate_Parameter(string delegateName, string delegateParameter, string otherPartOfMethodSignature, string delegateILName)
        {
            TestTypeSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.Delegate.{delegateILName}",
                $"public delegate void {delegateName}{string.Format(otherPartOfMethodSignature, $"({delegateParameter})")};");
        }

        [TestCase("TReturn", "FuncHandler<TReturn>", "()", "FuncHandler`1")]
        [TestCase("TReturn?", "NullableReferenceType<TReturn>", "() where TReturn : class", "NullableReferenceType`1")]
        [TestCase("TReturn?", "NullableValueType<TReturn>", "() where TReturn : struct", "NullableValueType`1")]
        public void TestGenericDelegate_ReturnType(string returnType, string delegateName, string otherPartOfMethodSignature, string delegateILName)
        {
            TestTypeSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.Delegate.{delegateILName}",
                $"public delegate {returnType} {delegateName}{otherPartOfMethodSignature};");
        }

        [TestCase("ValueType", "int i")]
        [TestCase("NullableValueType", "int? i")]
        [TestCase("NonNullableAndNullableValueType", "int i1, int? i2")]
        [TestCase("ReferenceType", "string s")]
        [TestCase("NullableReferenceType", "string? s")]
        [TestCase("NonNullableAndNullableReferenceType", "string s1, string? s2")]
        [TestCase("NonNullableInterfaceOfValueType", "ICollection<int> collection")]
        [TestCase("NullableInterfaceOfValueType", "ICollection<int>? collection")]
        [TestCase("NonNullableInterfaceOfNullableValueType", "ICollection<int?> collection")]
        [TestCase("NullableInterfaceOfNullableValueType", "ICollection<int?>? collection")]
        [TestCase("FourTypesOfInterfaceOfValueType", "ICollection<int> collection1, ICollection<int>? collection2, ICollection<int?> collection3, ICollection<int?>? collection4")]
        [TestCase("NonNullableInterfaceOfReferenceType", "ICollection<string> collection")]
        [TestCase("NullableInterfaceOfReferenceType", "ICollection<string>? collection")]
        [TestCase("NonNullableInterfaceOfNullableReferenceType", "ICollection<string?> collection")]
        [TestCase("NullableInterfaceOfNullableReferenceType", "ICollection<string?>? collection")]
        [TestCase("FourTypesOfInterfaceOfReferenceType", "ICollection<string> collection1, ICollection<string>? collection2, ICollection<string?> collection3, ICollection<string?>? collection4")]
        public void TestConstructor_Parameter(string typeName, string constructorParameter)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.Constructor.{typeName}",
                ".ctor", $"public {typeName} ({constructorParameter});");
        }

        [TestCase("int", "ValueType")]
        [TestCase("int?", "NullableValueType")]
        [TestCase("string", "ReferenceType")]
        [TestCase("string?", "NullableReferenceType")]
        [TestCase("DayOfWeek", "Enumeration")]
        [TestCase("DayOfWeek?", "NullableEnumeration")]
        [TestCase("ICollection<int>", "InterfaceOfValueType")]
        [TestCase("ICollection<int?>", "InterfaceOfNullableValueType")]
        [TestCase("ICollection<int>?", "NullableInterfaceOfValueType")]
        [TestCase("ICollection<int?>?", "NullableInterfaceOfNullableValueType")]
        [TestCase("ICollection<string>", "InterfaceOfReferenceType")]
        [TestCase("ICollection<string?>", "InterfaceOfNullableReferenceType")]
        [TestCase("ICollection<string>?", "NullableInterfaceOfReferenceType")]
        [TestCase("ICollection<string?>?", "NullableInterfaceOfNullableReferenceType")]
        public void TestField_ReturnType(string returnType, string fieldName)
        {
            TestFieldSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.FieldReturnType",
                fieldName, $"public {returnType} {fieldName};");
        }

        [TestCase("GenericFieldType", "T", "GenericType")]
        [TestCase("GenericFieldTypeOfValueType", "T", "GenericType")]
        [TestCase("GenericFieldTypeOfValueType", "T?", "NullableGenericType")]
        [TestCase("GenericFieldTypeOfValueType", "ICollection<T>", "InterfaceOfGenericType")]
        [TestCase("GenericFieldTypeOfValueType", "ICollection<T?>", "InterfaceOfNullableGenericType")]
        [TestCase("GenericFieldTypeOfValueType", "ICollection<T>?", "NullableInterfaceOfGenericType")]
        [TestCase("GenericFieldTypeOfValueType", "ICollection<T?>?", "NullableInterfaceOfNullableGenericType")]
        [TestCase("GenericFieldTypeOfValueType", "Dictionary<Dictionary<T, string>, string>", "DictionaryOfDictionary")]
        [TestCase("GenericFieldTypeOfValueType", "Dictionary<Dictionary<T?, string>, string>", "DictionaryOfDictionaryOfNullableGenericTypeKey")]
        [TestCase("GenericFieldTypeOfValueType", "Dictionary<Dictionary<T?, string>, string>?", "NullableDictionaryOfDictionaryOfNullableGenericTypeKey")]
        [TestCase("GenericFieldTypeOfReferenceType", "T", "GenericType")]
        [TestCase("GenericFieldTypeOfReferenceType", "T?", "NullableGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType", "ICollection<T>", "InterfaceOfGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType", "ICollection<T?>", "InterfaceOfNullableGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType", "ICollection<T>?", "NullableInterfaceOfGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType", "ICollection<T?>?", "NullableInterfaceOfNullableGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType", "Dictionary<Dictionary<T, string>, string>", "DictionaryOfDictionary")]
        [TestCase("GenericFieldTypeOfReferenceType", "Dictionary<Dictionary<T?, string>, string>", "DictionaryOfDictionaryOfNullableGenericTypeKey")]
        [TestCase("GenericFieldTypeOfReferenceType", "Dictionary<Dictionary<T?, string>, string>?", "NullableDictionaryOfDictionaryOfNullableGenericTypeKey")]
        public void TestGenericField_ReturnType(string typeName, string returnType, string fieldName)
        {
            TestFieldSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.{typeName}`1",
                fieldName, $"public {returnType} {fieldName};");
        }

        [TestCase("ValueType", "this int type, int i")]
        [TestCase("NullableValueType", "this int? type, int? i")]
        [TestCase("InterfaceOfValueType", "this int type, ICollection<int> collection")]
        [TestCase("NullableInterfaceOfNullableValueType", "this int? type, ICollection<int?>? collection")]
        [TestCase("NullableAndNonNullableValueType", "this int? type, int? i1, int i2, int? i3")]
        [TestCase("ReferenceType", "this string type, string s")]
        [TestCase("NullableReferenceType", "this string? type, string? s")]
        [TestCase("InterfaceOfReferenceType", "this string type, ICollection<string> collection")]
        [TestCase("NullableInterfaceOfNullableReferenceType", "this string? type, ICollection<string?>? collection")]
        [TestCase("NullableAndNonNullableNullableReferenceType", "this string? type, string? s1, string s2, string? s3")]
        [TestCase("NullableAndNonNullableNullableReferenceTypeAndValueType", "this string? type, string? s1, int? i1, int i2, string s2, string? s3")]
        public void TestExtensionMethod_Parameter(string methodName, string methodParameter)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.ExtensionMethod",
               methodName, $"public static void {methodName} ({methodParameter});");
        }

        public void TestOperatorOverloading()
        {

        }
    }
}
