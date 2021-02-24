using Mono.Documentation;
using Mono.Documentation.Updater.Formatters;
using NUnit.Framework;
using System;
using System.Linq;

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
        [TestCase("Tuple<int,int>", "TupleOfValueType")]
        [TestCase("Tuple<int,int>?", "NullableTupleOfValueType")]
        [TestCase("Tuple<int?,int?>", "TupleOfNullableValueType")]
        [TestCase("Tuple<int?,int?>?", "NullableTupleOfNullableValueType")]
        [TestCase("(int,int)", "ValueTupleOfValueType")]
        [TestCase("(int,int)?", "NullableValueTupleOfValueType")]
        [TestCase("(int?,int?)", "ValueTupleOfNullableValueType")]
        [TestCase("(int?,int?)?", "NullableValueTupleOfNullableValueType")]
        [TestCase("ICollection<int>", "InterfaceOfValueType")]
        [TestCase("ICollection<int>?", "NullableInterfaceOfValueType")]
        [TestCase("ICollection<int?>?", "NullableInterfaceOfNullableValueType")]
        [TestCase("Action<int>", "ActionOfValueType")]
        [TestCase("Action<int?>", "ActionOfNullableValueType")]
        [TestCase("Action<int>?", "NullableActionOfValueType")]
        [TestCase("Action<int?>?", "NullableActionOfNullableValueType")]
        [TestCase("Dictionary<int,int>", "DictionaryOfValueType")]
        [TestCase("Dictionary<int,int>?", "NullableDictionaryOfValueType")]
        [TestCase("Dictionary<int?,int?>", "DictionaryOfNullableValueType")]
        [TestCase("Dictionary<int?,int?>?", "NullableDictionaryOfNullableValueType")]
        [TestCase("Dictionary<int,int?>", "DictionaryOfNullableValueTypeValue")]
        [TestCase("Dictionary<int,int?>?", "NullableDictionaryOfNullableValueTypeValue")]
        [TestCase("Dictionary<int?,int>?", "NullableDictionaryOfNullableValueTypeKey")]
        [TestCase("Dictionary<int,Dictionary<int,int>>", "DictionaryOfValueTypeKeyAndDictionaryOfValueTypeValue")]
        [TestCase("Dictionary<int,Dictionary<int,int>>?", "NullableDictionaryOfValueTypeKeyAndDictionaryOfValueTypeValue")]
        [TestCase("Dictionary<int?,Dictionary<int,int>?>?", "NullableDictionaryOfNullableValueTypeKeyAndNullableDictionaryOfValueTypeValue")]
        [TestCase("Dictionary<int?,Dictionary<int?,int?>?>?", "NullableDictionaryOfNullableValueTypeKeyAndNullableDictionaryOfNullableValueTypeValue")]
        [TestCase("Dictionary<int,Tuple<int,int>>", "DictionaryOfValueTypeKeyAndTupleOfValueTypeValue")]
        [TestCase("Dictionary<int,Tuple<int,int>>?", "NullableDictionaryOfValueTypeKeyAndTupleOfValueTypeValue")]
        [TestCase("Dictionary<int?,Tuple<int,int>?>?", "NullableDictionaryOfNullableValueTypeKeyAndNullableTupleOfValueTypeValue")]
        [TestCase("Dictionary<int?,Tuple<int?,int?>?>?", "NullableDictionaryOfNullableValueTypeKeyAndNullableTupleOfNullableValueTypeValue")]
        [TestCase("Dictionary<Dictionary<int,int>,Dictionary<int,int>>", "DictionaryOfDictionaryOfValueType")]
        [TestCase("Dictionary<Dictionary<int,int>?,Dictionary<int,int>?>?", "NullableDictionaryOfNullableDictionaryOfValueType")]
        [TestCase("Dictionary<Dictionary<int?,int?>?,Dictionary<int?,int?>?>?", "NullableDictionaryOfNullableDictionaryOfNullableValueType")]
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
        [TestCase("Tuple<string,string>", "TupleOfReferenceType")]
        [TestCase("Tuple<string,string>?", "NullableTupleOfReferenceType")]
        [TestCase("Tuple<string?,string?>", "TupleOfNullableReferenceType")]
        [TestCase("Tuple<string?,string?>?", "NullableTupleOfNullableReferenceType")]
        [TestCase("(string,string)", "ValueTupleOfReferenceType")]
        [TestCase("(string,string)?", "NullableValueTupleOfReferenceType")]
        [TestCase("(string?,string?)", "ValueTupleOfNullableReferenceType")]
        [TestCase("(string?,string?)?", "NullableValueTupleOfNullableReferenceType")]
        [TestCase("ICollection<string>", "InterfaceOfReferenceType")]
        [TestCase("ICollection<string>?", "NullableInterfaceOfReferenceType")]
        [TestCase("ICollection<string?>?", "NullableInterfaceOfNullableReferenceType")]
        [TestCase("ICollection<dynamic>", "InterfaceOfDynamicType")]
        [TestCase("ICollection<dynamic>?", "NullableInterfaceOfDynamicType")]
        [TestCase("ICollection<dynamic?>?", "NullableInterfaceOfNullableDynamicType")]
        [TestCase("Action<string>", "ActionOfReferenceType")]
        [TestCase("Action<string?>", "ActionOfNullableReferenceType")]
        [TestCase("Action<string>?", "NullableActionOfReferenceType")]
        [TestCase("Action<string?>?", "NullableActionOfNullableReferenceType")]
        [TestCase("Dictionary<string,string>", "DictionaryOfReferenceType")]
        [TestCase("Dictionary<string,string>?", "NullableDictionaryOfReferenceType")]
        [TestCase("Dictionary<string?,string?>", "DictionaryOfNullableReferenceType")]
        [TestCase("Dictionary<string?,string?>?", "NullableDictionaryOfNullableReferenceType")]
        [TestCase("Dictionary<string,string?>", "DictionaryOfNullableReferenceTypeValue")]
        [TestCase("Dictionary<string,string?>?", "NullableDictionaryOfNullableReferenceTypeValue")]
        [TestCase("Dictionary<string?,string>?", "NullableDictionaryOfNullableReferenceTypeKey")]
        [TestCase("Dictionary<Dictionary<string,string>,Dictionary<string,string>>", "DictionaryOfDictionaryOfReferenceType")]
        [TestCase("Dictionary<Dictionary<string,string>?,Dictionary<string,string>?>?", "NullableDictionaryOfNullableDictionaryOfReferenceType")]
        [TestCase("Dictionary<Dictionary<string?,string?>?,Dictionary<string?,string?>?>?", "NullableDictionaryOfNullableDictionaryOfNullableReferenceType")]
        [TestCase("Dictionary<string,Dictionary<string,string>>", "DictionaryOfReferenceTypeKeyAndDictionaryOfReferenceTypeValue")]
        [TestCase("Dictionary<string,Dictionary<string,string>>?", "NullableDictionaryOfReferenceTypeKeyAndDictionaryOfReferenceTypeValue")]
        [TestCase("Dictionary<string?,Dictionary<string,string>?>?", "NullableDictionaryOfNullableReferenceTypeKeyAndNullableDictionaryOfReferenceTypeValue")]
        [TestCase("Dictionary<string?,Dictionary<string?,string?>?>?", "NullableDictionaryOfNullableReferenceTypeKeyAndNullableDictionaryOfNullableReferenceTypeValue")]
        [TestCase("Dictionary<string,Tuple<string,string>>", "DictionaryOfReferenceTypeKeyAndTupleOfReferenceTypeValue")]
        [TestCase("Dictionary<string,Tuple<string,string>>?", "NullableDictionaryOfReferenceTypeKeyAndTupleOfReferenceTypeValue")]
        [TestCase("Dictionary<string?,Tuple<string,string>?>?", "NullableDictionaryOfNullableReferenceTypeKeyAndNullableTupleOfReferenceTypeValue")]
        [TestCase("Dictionary<string?,Tuple<string?,string?>?>?", "NullableDictionaryOfNullableReferenceTypeKeyAndNullableTupleOfNullableReferenceTypeValue")]
        public void TypeName(string returnType, string methodName)
        {
            // Test single parameter and return type for method, extension method, property, field, event, delegate.
            // They have the same process logic that we just test once the type of them.
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.CommonType",
                methodName, $"public {returnType} {methodName} ();");
        }

        [TestCase("ParamsArrayOfNullableValueType", "int i, params int?[] array")]
        [TestCase("ParamsArrayOfNullableReferenceType", "string s, params object?[] array")]
        [TestCase("NullableAndNonNullableValueType", "int i1, int? i2, int i3")]
        [TestCase("NullableAndNonNullableReferenceType", "string s1, string? s2, string s3")]
        [TestCase("NullableGenericValueTypeOfValueType", "ReadOnlySpan<int> s1, ReadOnlySpan<int?> s2, ReadOnlySpan<int> s3")]
        [TestCase("NullableGenericValueTypeOfReferenceType", "ReadOnlySpan<string> s1, ReadOnlySpan<string?> s2, ReadOnlySpan<string> s3")]
        [TestCase("NullableAndNonNullableInterfaceOfValueType", "ICollection<int> collection1, ICollection<int>? collection2, ICollection<int> collection3")]
        [TestCase("NullableAndNonNullableInterfaceOfReferenceType", "ICollection<string> collection1, ICollection<string>? collection2, ICollection<string> collection3")]
        [TestCase("NonNullableValueTypeWithOutModifier", "string? value, out bool result")]
        [TestCase("NonNullableValueTypeWithRefModifier", "string? value, ref bool result")]
        public void MethodParameter(string methodName, string methodParameter)
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
        public void GenericMethodParameter(string methodName, string otherPartOfMethodSignature, string genericParameter)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.GenericMethodParameter",
                methodName, $"public void {methodName}{string.Format(otherPartOfMethodSignature, genericParameter)};");
        }

        [TestCase("T", "GenericType", "<T> ()")]
        [TestCase("T", "GenericReferenceType", "<T> () where T : class")]
        [TestCase("T?", "GenericNullableReferenceType", "<T> () where T : class")]
        [TestCase("T", "GenericValueType", "<T> () where T : struct")]
        [TestCase("T?", "GenericNullableValueType", "<T> () where T : struct")]
        public void GenericMethodReturnType(string returnType, string methodName, string otherPartOfMethodSignature)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.GenericMethodReturnType",
                methodName, $"public {returnType} {methodName}{otherPartOfMethodSignature};");
        }

        [TestCase("T", "GenericType")]
        [TestCase("TClass", "GenericReferenceType")]
        [TestCase("TClass?", "GenericNullableReferenceType")]
        [TestCase("TStruct", "GenericValueType")]
        [TestCase("TStruct?", "GenericNullableValueType")]
        public void GenericPropertyReturnType(string returnType, string propertyName)
        {
            TestPropertySignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.{GetGenericTypeILName("GenericPropertyReturnType<T, TClass, TStruct>")}",
                propertyName, $"public {returnType} {propertyName} {{ get; }}");
        }

        [TestCase("EventHandler", "EventHandler")]
        [TestCase("EventHandler?", "NullableEventHandler")]
        [TestCase("EventHandler<EventArgs>", "GenericEventHandler")]
        [TestCase("EventHandler<EventArgs?>", "GenericEventHandlerOfNullableEventArgs")]
        [TestCase("EventHandler<EventArgs>?", "NullableGenericEventHandler")]
        [TestCase("EventHandler<EventArgs?>?", "NullableGenericEventHandlerOfNullableEventArgs")]
        public void EventDelegateType(string delegateType, string eventName)
        {
            TestEventSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.Event",
                eventName, $"public event {delegateType} {eventName};");
        }

        [TestCase("Handler", "object sender, EventArgs args")]
        [TestCase("NullableSender", "object? sender, EventArgs args")]
        [TestCase("NullableSenderAndEventArgs", "object? sender, EventArgs? args")]
        public void DelegateTypeParameter(string delegateName, string delegateParameter)
        {
            TestTypeSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.Delegate.{delegateName}",
                $"public delegate void {delegateName}({delegateParameter});");
        }

        [TestCase("GenericHandler<TEventArgs>", "object sender, TEventArgs args", "{0}")]
        [TestCase("NullableSender<TEventArgs>", "object? sender, TEventArgs args", "{0}")]
        [TestCase("NullableSenderAndEventArgs<TEventArgs>", "object? sender, TEventArgs? args", "{0} where TEventArgs : class")]
        [TestCase("ActionHandler<TClass,TStruct>", "TClass t1, TStruct t2", "{0} where TClass : class where TStruct : struct")]
        [TestCase("NullableActionHandler<TClass,TStruct>", "TClass? t1, TStruct? t2", "{0} where TClass : class where TStruct : struct")]
        public void GenericDelegateTypeParameter(string delegateName, string delegateParameter, string otherPartOfMethodSignature)
        {
            TestTypeSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.Delegate.{GetGenericTypeILName(delegateName)}",
                $"public delegate void {delegateName}{string.Format(otherPartOfMethodSignature, $"({delegateParameter})")};");
        }

        [TestCase("TReturn", "FuncHandler<TReturn>", "()")]
        [TestCase("TReturn?", "NullableReferenceType<TReturn>", "() where TReturn : class")]
        [TestCase("TReturn?", "NullableValueType<TReturn>", "() where TReturn : struct")]
        public void GenericDelegateTypeReturnType(string returnType, string delegateName, string otherPartOfMethodSignature)
        {
            TestTypeSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.Delegate.{GetGenericTypeILName(delegateName)}",
                $"public delegate {returnType} {delegateName}{otherPartOfMethodSignature};");
        }

        [TestCase("NonNullableAndNullableValueType", "int i1, int? i2, int i3")]
        [TestCase("NonNullableAndNullableReferenceType", "string s1, string? s2, string s3")]
        [TestCase("InterfaceOfValueType", "ICollection<int> collection1, ICollection<int>? collection2, ICollection<int> collection3")]
        [TestCase("InterfaceOfReferenceType", "ICollection<string> collection1, ICollection<string>? collection2, ICollection<string> collection3")]
        public void ConstructorParameter(string typeName, string constructorParameter)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.Constructor.{typeName}",
                ".ctor", $"public {typeName} ({constructorParameter});");
        }

        [TestCase("GenericFieldType<T>", "T", "GenericType")]
        [TestCase("GenericFieldTypeOfValueType<T>", "T", "GenericType")]
        [TestCase("GenericFieldTypeOfValueType<T>", "T?", "NullableGenericType")]
        [TestCase("GenericFieldTypeOfValueType<T>", "ICollection<T>", "InterfaceOfGenericType")]
        [TestCase("GenericFieldTypeOfValueType<T>", "ICollection<T?>", "InterfaceOfNullableGenericType")]
        [TestCase("GenericFieldTypeOfValueType<T>", "ICollection<T>?", "NullableInterfaceOfGenericType")]
        [TestCase("GenericFieldTypeOfValueType<T>", "ICollection<T?>?", "NullableInterfaceOfNullableGenericType")]
        [TestCase("GenericFieldTypeOfValueType<T>", "Dictionary<Dictionary<T,string>,string>", "DictionaryOfDictionary")]
        [TestCase("GenericFieldTypeOfValueType<T>", "Dictionary<Dictionary<T?,string>,string>", "DictionaryOfDictionaryOfNullableGenericTypeKey")]
        [TestCase("GenericFieldTypeOfValueType<T>", "Dictionary<Dictionary<T?,string>,string>?", "NullableDictionaryOfDictionaryOfNullableGenericTypeKey")]
        [TestCase("GenericFieldTypeOfReferenceType<T>", "T", "GenericType")]
        [TestCase("GenericFieldTypeOfReferenceType<T>", "T?", "NullableGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType<T>", "ICollection<T>", "InterfaceOfGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType<T>", "ICollection<T?>", "InterfaceOfNullableGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType<T>", "ICollection<T>?", "NullableInterfaceOfGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType<T>", "ICollection<T?>?", "NullableInterfaceOfNullableGenericType")]
        [TestCase("GenericFieldTypeOfReferenceType<T>", "Dictionary<Dictionary<T,string>,string>", "DictionaryOfDictionary")]
        [TestCase("GenericFieldTypeOfReferenceType<T>", "Dictionary<Dictionary<T?,string>,string>", "DictionaryOfDictionaryOfNullableGenericTypeKey")]
        [TestCase("GenericFieldTypeOfReferenceType<T>", "Dictionary<Dictionary<T?,string>,string>?", "NullableDictionaryOfDictionaryOfNullableGenericTypeKey")]
        public void GenericFieldReturnType(string typeName, string returnType, string fieldName)
        {
            TestFieldSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.{GetGenericTypeILName(typeName)}",
                fieldName, $"public {returnType} {fieldName};");
        }

        [TestCase("NullableAndNonNullableValueType", "this int? type, int? i1, int i2, int? i3")]
        [TestCase("NullableAndNonNullableNullableReferenceType", "this string? type, string? s1, string s2, string? s3")]
        [TestCase("NullableAndNonNullableNullableReferenceTypeAndValueType", "this string? type, string? s1, int? i1, int i2, string s2, string? s3")]
        public void ExtensionMethodParameter(string methodName, string methodParameter)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, "mdoc.Test.NullableReferenceTypes.ExtensionMethod",
               methodName, $"public static void {methodName} ({methodParameter});");
        }

        [TestCase("Student", "op_Addition", "Student operator +", "Student s1, Student s2")]
        [TestCase("Student", "op_Subtraction", "Student? operator -", "Student? s1, Student? s2")]
        [TestCase("Student", "op_Multiply", "Student operator *", "Student s1, Student? s2")]
        [TestCase("Student", "op_Division", "Student operator /", "Student? s1, Student s2")]
        [TestCase("Student", "op_Implicit", "implicit operator ExamScore", "Student? s")]
        [TestCase("Student", "op_Explicit", "explicit operator Student?", "ExamScore? s")]
        [TestCase("ExamScore", "op_Addition", "ExamScore operator +", "ExamScore s1, ExamScore s2")]
        [TestCase("ExamScore", "op_Subtraction", "ExamScore? operator -", "ExamScore? s1, ExamScore? s2")]
        [TestCase("ExamScore", "op_Multiply", "ExamScore operator *", "ExamScore s1, ExamScore? s2")]
        [TestCase("ExamScore", "op_Division", "ExamScore operator /", "ExamScore? s1, ExamScore s2")]
        [TestCase("ExamScore", "op_Implicit", "implicit operator ExamScore", "Student? s")]
        [TestCase("ExamScore", "op_Explicit", "explicit operator Student?", "ExamScore? s")]
        public void OperatorOverloading(string typeName, string methodName, string operatorName, string methodParameter)
        {
            TestMethodSignature(NullableReferenceTypesAssemblyPath, $"mdoc.Test.NullableReferenceTypes.OperatorOverloading.{typeName}",
               methodName, $"public static {operatorName} ({methodParameter});");
        }

        private string GetGenericTypeILName(string genericTypeName)
        {
            var startParameterIndex = genericTypeName.IndexOf('<');
            var endParameterIndex = genericTypeName.IndexOf('>');

            if (startParameterIndex != -1 && endParameterIndex != -1 && endParameterIndex == genericTypeName.Length - 1)
            {
                var parameterList = genericTypeName.Substring(startParameterIndex + 1, endParameterIndex - startParameterIndex - 1);
                var parameterCount = parameterList.Split(',').Count();
                var genericTypeNameWithoutParameter = genericTypeName.Substring(0, startParameterIndex);

                return $"{genericTypeNameWithoutParameter}`{parameterCount}";
            }

            throw new ArgumentException("The generic type name is not a valid value.", nameof(genericTypeName));
        }
    }
}
