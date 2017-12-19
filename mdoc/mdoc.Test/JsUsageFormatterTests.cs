using mdoc.Mono.Documentation.Updater.Formatters;
using mdoc.Test.SampleClasses;
using Mono.Documentation.Updater;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture]
    [Category("Javascript")]
    [Category("Usage")]
    class JsUsageFormatterTests : BasicFormatterTests<JsUsageFormatter>
    {
        protected override JsUsageFormatter formatter => new JsUsageFormatter();

        #region IsSupported
        [Test]
        public void Test_IsSupportedType_WebHostHidden()
        {
            Assert.IsFalse(formatter.IsSupported(GetType(typeof(WebHostHiddenTestClass))));
        }

        [Test]
        public void Test_IsSupportedMember_WebHostHidden()
        {
            Assert.IsFalse(formatter.IsSupported(GetMethod(typeof(WebHostHiddenTestClass), 
                i => i.Name == nameof(WebHostHiddenTestClass.SomeMethod))));
        }

        [Test]
        public void Test_IsSupportedType_Generic()
        {
            Assert.IsFalse(formatter.IsSupported(GetType(typeof(SomeGenericClass<>))));
        }

        [Test]
        public void Test_IsSupportedMember_Generic()
        {
            Assert.IsFalse(formatter.IsSupported(GetMethod(typeof(SomeGenericClass<>),
                i => i.Name == "SomeMethod")));
        }

        [Test]
        public void Test_IsSupportedMember_Generic2()
        {
            Assert.IsFalse(formatter.IsSupported(GetMethod(typeof(SomeGenericClass<>),
                i => i.Name == "SomeMethod2")));
        }

        [Test]
        public void Test_IsSupportedMember_Generic3()
        {
            // WinRT doesn't support public types with generic parameters
            Assert.IsFalse(formatter.IsSupported(GetMethod(typeof(SomeGenericClass<>),
                i => i.Name == "SomeMethod3")));
        }

        [Test]
        public void Test_IsSupported_MethodWithWebHostHiddenParameter()
        {
            Assert.IsFalse(formatter.IsSupported(GetMethod(typeof(SomeClass),
                i => i.Name == nameof(SomeClass.SomeMethodWebHostHiddenParameter))));
        }

        [Test]
        public void Test_IsSupported_MethodWithWebHostHiddenReturn()
        {
            Assert.IsFalse(formatter.IsSupported(GetMethod(typeof(SomeClass),
                i => i.Name == nameof(SomeClass.SomeMethodWebHostHiddenReturn))));
        }

        [Test]
        public void Test_IsSupportedMember_Operator()
        {
            // Operator overloads are not supported as they are not supported for Windows Runtime 
            // (that is to say, they are not supported at the ABI level). 
            // C# supports operator overloading, and C# implementations get compiled to metadata as static methods 
            // on the type in question, such as TypeName::op_Equality, TypeName::op_GreaterThan, etc. 
            // You can see a list on StackOverflow.  As long as they’re public, they can be invoked by name 
            // just like any other static method.
            Assert.IsFalse(formatter.IsSupported(GetMethod(typeof(TestClass),
                i => i.Name == "op_UnaryPlus")));
        }

        [Test]
        public void Test_IsSupportedMember_NotOperator()
        {
            Assert.IsFalse(formatter.IsSupported(GetMethod(typeof(SomeClass),
                i => i.Name == nameof(SomeClass.op_NotOperator))));
        }

        #endregion

        #region Fields
        [Test]
        [Category("Fields")]
        public void Test_Field()
            => TestFieldSignature(typeof(SomeStruct),
                null, nameof(SomeStruct.IntMember));
        #endregion

        #region Properties
        [Test]
        [Category("Properties")]
        public void Test_Property_0()
            => TestPropertySignature(typeof(SomeClass),
@"var int32 = someClass.property;
someClass.property = int32;", 
nameof(SomeClass.Property));

        [Test]
        [Category("Properties")]
        public void Test_Property_1()
            => TestPropertySignature(typeof(SomeClass),
@"var testClass = someClass.property2;
someClass.property2 = testClass;", 
nameof(SomeClass.Property2));

        [Test]
        [Category("Properties")]
        public void Test_Property_2()
            => TestPropertySignature(typeof(SomeClass),
                "var testClass = someClass.property3;",
                nameof(SomeClass.Property3));

        [Test]
        [Category("Properties")]
        public void Test_Property_3()
            => TestPropertySignature(typeof(SomeClass),
                "someClass.property4 = testClass;",
                nameof(SomeClass.Property4));
        
        [Test]
        [Category("Properties")]
        public void Test_Static_Property_0()
            => TestPropertySignature(typeof(SomeClass),
@"var int32 = SomeClass.staticProperty;
SomeClass.staticProperty = int32;",
nameof(SomeClass.StaticProperty));
        #endregion

        #region Types
        [Test]
        [Category("Types")]
        [Category("Enums")]
        public void Test_Enum_0()
            => TestTypeSignature(typeof(SomeEnum), "var value = mdoc.Test.SampleClasses.SomeEnum.testEnumElement1");

        [Test]
        [Category("Types")]
        [Category("Enums")]
        public void Test_Enum_1()
            => TestTypeSignature(typeof(SomeEmptyEnum), null);

        [Test]
        [Category("Types")]
        [Category("Struct")]
        public void Test_Struct()
            => TestTypeSignature(typeof(SomeStruct),
@"var someStruct = {
intMember : /* Your value */,
staticMember : /* Your value */,
testClassMember : /* Your value */
}");

        [Test]
        [Category("Types")]
        [Category("Deleagates")]
        public void Test_Delegate()
            => TestTypeSignature(typeof(SomeDelegate),
@"var someDelegateHandler = function(str){
/* Your code */
}");

        [Test]
        [Category("Types")]
        [Category("Class")]
        public void Test_Class_0()
            => TestTypeSignature(typeof(SomeClass),
@"var someClass = new SomeClass(i, j);");

        [Test]
        [Category("Types")]
        [Category("Class")]
        public void Test_Class_1()
            => TestTypeSignature(typeof(SomeClassWithManyConstructors), null);

        #endregion

        #region Methods
        [Test]
        [Category("Methods")]
        public void Test_AyncMethod()
            => TestMethodSignature(typeof(SomeClass),
                "someClass.asyncMethod().done( /* Your success and error handlers */ )",
                nameof(SomeClass.AsyncMethod));

        [Test]
        [Category("Methods")]
        public void Test_StaticAyncMethod()
            => TestMethodSignature(typeof(SomeClass),
                "mdoc.Test.SampleClasses.SomeClass.staticAsyncMethod().done( /* Your success and error handlers */ )",
                nameof(SomeClass.StaticAsyncMethod));

        [Test]
        [Category("Methods")]
        public void Test_Method()
            => TestMethodSignature(typeof(SomeClass),
                "someClass.someMethod()",
                nameof(SomeClass.SomeMethod));

        [Test]
        [Category("Methods")]
        public void Test_StaticMethod()
            => TestMethodSignature(typeof(SomeClass),
                "mdoc.Test.SampleClasses.SomeClass.someStaticMethod()",
                nameof(SomeClass.SomeStaticMethod));

        [Test]
        [Category("Methods")]
        public void Test_MethodWithParameters()
            => TestMethodSignature(typeof(SomeClass),
                "someClass.someMethodWithParameters(someClass, i)",
                nameof(SomeClass.SomeMethodWithParameters));

        [Test]
        [Category("Methods")]
        public void Test_MethodWithReturn()
            => TestMethodSignature(typeof(SomeClass),
                "var int32 = someClass.someMethod2()",
                nameof(SomeClass.SomeMethod2));

        [Test]
        [Category("Methods")]
        public void Test_MethodWithReturnBool()
            => TestMethodSignature(typeof(SomeClass),
                "var boolean = someClass.someMethodWithReturnBool()",
                nameof(SomeClass.SomeMethodWithReturnBool));

        [Test]
        [Category("Methods")]
        public void Test_StaticMethodWithReturn()
            => TestMethodSignature(typeof(SomeClass),
                "var int32 = mdoc.Test.SampleClasses.SomeClass.someStaticMethod2()",
                nameof(SomeClass.SomeStaticMethod2));

        [Test]
        [Category("Methods")]
        [Category("Constructors")]
        public void Test_Constructor()
            => TestMethodSignature(typeof(TestClass),
                "var testClass = new TestClass();",
                ".ctor");
        #endregion

        #region Events
        [Test]
        [Category("Events")]
        public void Test_Event()
            => TestEventSignature(typeof(SomeClass),
"function onAppMemoryUsageIncreased(eventArgs) { /* Your code */ }" + MemberFormatter.GetLineEnding() +
"someClass.addEventListener(\"appmemoryusageincreased\", onAppMemoryUsageIncreased);" + MemberFormatter.GetLineEnding() +
"someClass.removeEventListener(\"appmemoryusageincreased\", onAppMemoryUsageIncreased);" + MemberFormatter.GetLineEnding() +
"- or -" + MemberFormatter.GetLineEnding() +
"someClass.onappmemoryusageincreased = onAppMemoryUsageIncreased;",
                nameof(SomeClass.AppMemoryUsageIncreased));

        [Test]
        [Category("Events")]
        public void Test_StaticEvent()
            => TestEventSignature(typeof(SomeClass),
"function onStaticEvent(eventArgs) { /* Your code */ }" + MemberFormatter.GetLineEnding() +
"mdoc.Test.SampleClasses.SomeClass.addEventListener(\"staticevent\", onStaticEvent);" + MemberFormatter.GetLineEnding() +
"mdoc.Test.SampleClasses.SomeClass.removeEventListener(\"staticevent\", onStaticEvent);" + MemberFormatter.GetLineEnding() +
"- or -" + MemberFormatter.GetLineEnding() +
"mdoc.Test.SampleClasses.SomeClass.onstaticevent = onStaticEvent;",
                nameof(SomeClass.StaticEvent));

        [Test]
        [Category("Events")]
        public void Test_EventPrivate()
            => TestEventSignature(typeof(SomeClass),
                null,
                "PrivateEvent");
        #endregion
    }
}
