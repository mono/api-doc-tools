using System;
using mdoc.Test.FSharp;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using Mono.Documentation.Updater;
using NUnit.Framework;

namespace mdoc.Test
{
    [TestFixture]
    [Category("FSharp")]
    public class FSharpFormatterTests : BasicFSharpFormatterTests<FSharpMemberFormatter>
    {
        private static readonly FSharpMemberFormatter fSharpMemberFormatter = new FSharpMemberFormatter();
        protected override FSharpMemberFormatter formatter => fSharpMemberFormatter;

        #region Types
        [Test]
        [Category("Types")]
        public void TypeSignature_Class1() =>
            TestTypeSignature(typeof(Class1),
@"type Class1 =
    new : unit -> Class1
    member this.T : unit
    member this.X : string");

        [Test]
        [Category("Types")]
        public void TypeSignature_Struct() =>
            TestTypeSignature(typeof(Constructors.MyStruct),
@"[<Sealed>]
type Constructors.MyStruct =
    struct
        new : x:int * y:int * z:int -> Constructors.MyStruct
        val X : int
        val Y : int
        val Z : int
    end");

        [Test]
        [Category("Types")]
        public void TypeSignature_Record() =>
            TestTypeSignature(typeof(Constructors.PetData), 
@"type Constructors.PetData =
    { Name : string
      Age : int
      Animal : string }");

        [Test]
        [Category("Types")]
        public void TypeSignature_Record2() =>
            TestTypeSignature(typeof(Records.Car),
@"type Car =
    { Make : string
      Model : string
      mutable Odometer : int }");

        [Test]
        [Category("Types")]
        [Category("Modules")]
        public void TypeSignature_Module() =>
            TestTypeSignature(typeof(Records),
@"module Records");

        [Test]
        [Category("Types")]
        [Category("Modules")]
        public void TypeSignature_TopLevelModule() =>
            TestTypeSignature(typeof(NestedModules),
@"module NestedModules");

        [Test]
        [Category("Types")]
        [Category("Modules")]
        public void TypeSignature_NestingModule() =>
            TestTypeSignature(typeof(NestedModules.Y),
@"module NestedModules.Y");

        [Test]
        [Category("Types")]
        [Category("Modules")]
        public void TypeSignature_NestedModule() =>
            TestTypeSignature(typeof(NestedModules.Y.Z),
@"module NestedModules.Y.Z");
        
        [Test]
        [Category("Types")]
        public void TypeSignature_Attribute() =>
            TestTypeSignature(typeof(Attributes.TypeWithFlagAttribute), 
@"type Attributes.TypeWithFlagAttribute =
    member this.X : string");

        [Test]
        [Category("Types")]
        public void TypeSignature_Inheritance() =>
            TestTypeSignature(typeof(Attributes.OwnerAttribute), 
@"type Attributes.OwnerAttribute =
    inherit Attribute
    new : name:string -> Attributes.OwnerAttribute");

        [Test]
        [Category("Types")]
        [Category("Interfaces")]
        public void TypeSignature_InterfaceImplementation() =>
            TestTypeSignature(typeof(Interfaces.SomeClass1),
@"type Interfaces.SomeClass1 =
    interface Interfaces.IPrintable
    new : x:int * y:double -> Interfaces.SomeClass1");

        [Test]
        [Category("Types")]
        [Category("Interfaces")]
        public void TypeSignature_Interface() =>
            TestTypeSignature(typeof(Interfaces.Interface0),
@"type Interfaces.Interface0 =
    interface
        abstract member Method1 : int -> int
    end");

        [Test]
        [Category("Types")]
        [Category("DiscriminatedUnions")]
        public void TypeSignature_Union_0() =>
            TestTypeSignature(typeof(DiscriminatedUnions.Shape),
@"[<AbstractClass>]
type DiscriminatedUnions.Shape =");

        [Test]
        [Category("Types")]
        [Category("DiscriminatedUnions")]
        public void TypeSignature_Union_1() =>
            TestTypeSignature(typeof(DiscriminatedUnions.Shape.Tags),
                null);

        [Test]
        [Category("Types")]
        [Category("DiscriminatedUnions")]
        public void TypeSignature_Union_2() =>
            TestTypeSignature(typeof(DiscriminatedUnions.Shape.Circle),
                "DiscriminatedUnions.Shape.Circle : double -> DiscriminatedUnions.Shape");
        
        [Test]
        [Category("Types")]
        [Category("DiscriminatedUnions")]
        public void TypeSignature_Union_3() =>
            TestTypeSignature(typeof(DiscriminatedUnions.Shape.Rectangle),
                "DiscriminatedUnions.Shape.Rectangle : double * double -> DiscriminatedUnions.Shape");
        
        [Test]
        [Category("Types")]
        [Category("DiscriminatedUnions")]
        public void TypeSignature_Union_4() =>
            TestTypeSignature(typeof(DiscriminatedUnions.SizeUnion),
@"[<Sealed>]
type DiscriminatedUnions.SizeUnion =");

        [Test]
        [Category("Types")]
        [Category("DiscriminatedUnions")]
        public void TypeSignature_Union_5() =>
            TestPropertySignature(
                typeof(DiscriminatedUnions.SizeUnion),
                "member this.Small : DiscriminatedUnions.SizeUnion",
                nameof(DiscriminatedUnions.SizeUnion.Small));

        [Test]
        [Category("Types")]
        [Category("DiscriminatedUnions")]
        public void TypeSignature_Union_6() =>
            TestPropertySignature(
                typeof(DiscriminatedUnions.SizeUnion),
                null,
                "IsSmall");

        [Test]
        [Category("Types")]
        [Category("Enums")]
        public void TypeSignature_Enum_0() =>
            TestTypeSignature(typeof(DiscriminatedUnions.ColorEnum),
@"[<Sealed>]
type DiscriminatedUnions.ColorEnum =");

        [Test]
        [Category("Types")]
        [Category("Enums")]
        public void TypeSignature_Enum_1() =>
            TestFieldSignature(typeof(DiscriminatedUnions.ColorEnum),
                "Red = 5",
                nameof(DiscriminatedUnions.ColorEnum.Red));

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_0() =>
            TestTypeSignature(typeof(Delegates.Delegate1),
@"[<Sealed>]
type Delegates.Delegate1 = delegate of (int * int) -> int");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_1() =>
            TestTypeSignature(typeof(Delegates.Delegate2),
@"[<Sealed>]
type Delegates.Delegate2 = delegate of int * int -> int");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_2() =>
            TestTypeSignature(typeof(Delegates.Delegate3),
@"[<Sealed>]
type Delegates.Delegate3 = delegate of int * char -> string");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_3() =>
            TestTypeSignature(typeof(Delegates.Delegate4),
@"[<Sealed>]
type Delegates.Delegate4 = delegate of int -> (int -> char)");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_4() =>
            TestTypeSignature(typeof(Delegates.Delegate5),
@"[<Sealed>]
type Delegates.Delegate5 = delegate of int -> (int -> char -> string)");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_5() =>
            TestTypeSignature(typeof(Delegates.Delegate6),
@"[<Sealed>]
type Delegates.Delegate6 = delegate of (int -> double) -> char");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_6() =>
            TestTypeSignature(typeof(Delegates.Delegate7),
@"[<Sealed>]
type Delegates.Delegate7 = delegate of (int -> char -> string) -> double");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_7() =>
            TestTypeSignature(typeof(Delegates.Delegate8),
@"[<Sealed>]
type Delegates.Delegate8 = delegate of int -> char");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_8() =>
            TestTypeSignature(typeof(Delegates.Delegate9),
@"[<Sealed>]
type Delegates.Delegate9 = delegate of (int * int) -> char");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_9() =>
            TestTypeSignature(typeof(Delegates.Delegate10),
@"[<Sealed>]
type Delegates.Delegate10 = delegate of int * int -> char");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_10() =>
            TestTypeSignature(typeof(Delegates.Delegate11),
@"[<Sealed>]
type Delegates.Delegate11 = delegate of char -> unit");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_11() =>
            TestTypeSignature(typeof(Delegates.Delegate12),
@"[<Sealed>]
type Delegates.Delegate12 = delegate of unit -> char");

        [Test]
        [Category("Types")]
        [Category("Delegates")]
        public void TypeSignature_Delegate_12() =>
            TestTypeSignature(typeof(Delegates.Delegate13),
@"[<Sealed>]
type Delegates.Delegate13 = delegate of (int -> char -> string -> decimal) -> double");
        
        [Test]
        [Category("Types")]
        [Category("Tuples")]
        [Ignore("This test is tricky, since we want to ignore members for .NET metadata, but not F# types.")]
        public void TypeSignature_Tuple() =>
            TestTypeSignature(typeof(Tuple<,,,>),
@"type Tuple<'T1, 'T2, 'T3, 'T4> =
    interface IComparable
    interface IStructuralComparable
    interface IStructuralEquatable
    interface ITuple
    new : item1:'T1 * item2:'T2 * item3:'T3 * item4:'T4 -> Tuple<'T1, 'T2, 'T3, 'T4>");
        #endregion

        #region Functions
        [Test]
        [Category("Functions")]
        public void FunctionsSignature_1() =>
            TestMethodSignature(typeof(Functions), "Functions.function1 : x:int -> int", nameof(Functions.function1));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_2() =>
            TestMethodSignature(typeof(Functions), "Functions.function2 : x2:int -> int", nameof(Functions.function2));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_3() =>
            TestMethodSignature(typeof(Functions), "Functions.function3 : x3:int -> int", nameof(Functions.function3));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_4() =>
            TestMethodSignature(typeof(Functions), "Functions.function4 : x4:int -> y4:int -> int", nameof(Functions.function4));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_5() =>
            TestMethodSignature(typeof(Functions), "Functions.function5 : x5:int * y5:int -> int", nameof(Functions.function5));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_6() =>
            TestMethodSignature(typeof(Functions), "Functions.function6 : x6:'a * y6:'b -> unit", nameof(Functions.function6));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_7() =>
            TestMethodSignature(typeof(Functions), "Functions.function7 : x7:'a -> y7:'b * z7:'c -> unit", nameof(Functions.function7));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_8() =>
            TestMethodSignature(typeof(Functions), "Functions.function8 : x8:'a -> y8:'b -> z8:'c -> unit", nameof(Functions.function8));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_9() =>
            TestMethodSignature(typeof(Functions), "Functions.function9 : x9:'a * y9:'b -> z9:'c * a9:'d -> unit", nameof(Functions.function9));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_10() =>
            TestMethodSignature(typeof(Functions), "Functions.function10 : x:obj * y:obj -> z:obj * a:obj -> unit", nameof(Functions.function10));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_11() =>
            TestMethodSignature(typeof(Functions), "Functions.function11 : x:obj * y:obj * z:obj -> a:obj * b:obj -> unit", nameof(Functions.function11));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_12() =>
            TestMethodSignature(typeof(Functions), "Functions.function12 : x:obj -> a:obj * b:obj * c:obj * d:obj * e:obj -> unit", nameof(Functions.function12));
        
        [Test]
        [Category("Functions")]
        [Category("FSharpCore")]
        [Ignore("No ^U in IL code")]
        public void FunctionsSignature_13() =>
            TestMethodSignature(typeof(ArrayModule),
                "Array.averageBy : ('T -> ^U) -> 'T [] -> ^U",
                "AverageBy");
        
        [Test]
        [Category("Functions")]
        [Category("FSharpCore")]
        public void FunctionsSignature_14() =>
            TestMethodSignature(typeof(Collections),
                "Collections.f : x:Map<int, int> -> int",
                nameof(Collections.f));

        [Test]
        [Category("Functions")]
        [Category("FSharpCore")]
        public void FunctionsSignature_15() =>
            TestMethodSignature(typeof(Collections),
                "Collections.f2 : x:seq<int> -> int",
                nameof(Collections.f2));

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_16() =>
            TestMethodSignature(typeof(PatternMatching.PatternMatchingExamples),
                "PatternMatchingExamples.countValues : list:List<'a> -> value:'a -> int",
                "countValues");

        [Test]
        [Category("Functions")]
        public void FunctionsSignature_17() =>
            TestMethodSignature(typeof(FlexibleTypes),
                "FlexibleTypes.iterate1 : f:(unit -> seq<int>) -> unit",
                nameof(FlexibleTypes.iterate1));

        [Test]
        [Category("Functions")]
        [Category("FlexibleTypes")]
        public void FunctionsSignature_18() =>
            TestMethodSignature(typeof(FlexibleTypes),
                "FlexibleTypes.iterate2 : f:(unit -> #seq<int>) -> unit",
                nameof(FlexibleTypes.iterate2));
        #endregion

        #region Methods
        [Test]
        [Category("Methods")]
        public void MethodSignature_1() =>
            TestMethodSignature(typeof(Methods.SomeType), 
                "member this.SomeMethod : a:int * b:int * c:int -> int", 
                nameof(Methods.SomeType.SomeMethod));

        [Test]
        [Category("Methods")]
        public void MethodSignature_2() =>
            TestMethodSignature(typeof(Methods.SomeType), 
                "member this.SomeOtherMethod : a:int * b:int * c:int -> int", 
                nameof(Methods.SomeType.SomeOtherMethod));

        [Test]
        [Category("Methods")]
        public void MethodSignature_3() =>
            TestMethodSignature(typeof(Collections),
                "Collections.f : x:Map<int, int> -> int", 
                nameof(Collections.f));

        [Test]
        [Category("Methods")]
        public void MethodSignature_4() =>
            TestMethodSignature(typeof(Collections),
                "Collections.f2 : x:seq<int> -> int", 
                nameof(Collections.f2));

        [Test]
        [Category("Methods")]
        public void MethodSignature_StaticMethod() =>
            TestMethodSignature(typeof(Methods.SomeType), 
                "static member SomeStaticMethod : a:int * b:int * c:int -> int", 
                nameof(Methods.SomeType.SomeStaticMethod));

        [Test]
        [Category("Methods")]
        public void MethodSignature_StaticMethod_2() =>
            TestMethodSignature(typeof(Methods.SomeType), 
                "static member SomeOtherStaticMethod : a:int * b:int * c:int -> int", 
                nameof(Methods.SomeType.SomeOtherStaticMethod));
        

        [Test]
        [Category("Methods")]
        public void MethodSignature_StaticMethod_3() =>
            TestMethodSignature(typeof(Methods.SomeType),
                "static member SomeOtherStaticMethod3 : a:int * b:int -> c:int -> d:int -> int", 
                nameof(Methods.SomeType.SomeOtherStaticMethod3));

        
        [Test]
        [Category("Methods")]
        public void MethodSignature_AbstractMethod() =>
            TestMethodSignature(typeof(AbstractClasses.Shape2D), 
                "abstract member Rotate2 : double -> unit", 
                nameof(AbstractClasses.Shape2D.Rotate2));

        [Test]
        [Category("Methods")]
        public void MethodSignature_VirtualMethod() =>
            TestMethodSignature(typeof(AbstractClasses.Shape2D),
@"abstract member Rotate : angle:double -> unit
override this.Rotate : angle:double -> unit", 
            nameof(AbstractClasses.Shape2D.Rotate));

        [Test]
        [Category("Methods")]
        public void MethodSignature_OverrideMethod() =>
            TestMethodSignature(typeof(AbstractClasses.Circle), 
                "override this.Rotate : double -> unit", 
                nameof(AbstractClasses.Circle.Rotate));
        
        [Test]
        [Category("Methods")]
        public void MethodSignature_MethodWithOptionResult() =>
            TestMethodSignature(typeof(Methods.RectangleXY), 
                "static member intersection : rect1:Methods.RectangleXY * rect2:Methods.RectangleXY -> option<Methods.RectangleXY>", 
                nameof(Methods.RectangleXY.intersection));

        [Test]
        [Category("Methods")]
        public void MethodSignature_MethodWithRefParameter() =>
            TestMethodSignature(typeof(Methods.SomeType),
                "member this.TestRefParam : i:ref<int> -> int", 
                nameof(Methods.SomeType.TestRefParam));
        
        #endregion

        #region Constructors
        [Test]
        [Category("Constructors")]
        public void MethodSignature_Constructor_0() =>
            TestMethodSignature(typeof(Interfaces.SomeClass1),
                "new : x:int * y:double -> Interfaces.SomeClass1",
                ".ctor");

        [Test]
        [Category("Constructors")]
        public void MethodSignature_Constructor_1() =>
            TestMethodSignature(typeof(Constructors.MyClass),
                "new : x0:int * y0:int * z0:int -> Constructors.MyClass",
                ".ctor");

        [Test]
        [Category("Types")]
        public void TypeSignature_ClassWithPrimaryConstructorWithObjArguments() =>
            TestMethodSignature(typeof(Constructors.MyClassObjectParameters),
                "new : x0:string * y0:obj * z0:obj -> Constructors.MyClassObjectParameters",
                ".ctor");
        #endregion

        #region Properties
        [Test]
        [Category("Properties")]
        public void PropertySignature_0() =>
            TestPropertySignature(typeof(Methods.RectangleXY), 
                "member this.X1 : double", 
                nameof(Methods.RectangleXY.X1));

        [Test]
        [Category("Properties")]
        public void PropertySignature_1() =>
            TestPropertySignature(typeof(Interfaces), 
                "Interfaces.x1 : Interfaces.SomeClass1", 
                nameof(Interfaces.x1));


        [Test]
        [Category("Properties")]
        public void PropertySignature_2() =>
            TestPropertySignature(typeof(Class1),
                "member this.T : unit",
                "T");

        [Test]
        [Category("Properties")]
        public void PropertySignature_3() =>
            TestPropertySignature(typeof(Constructors.MyClass3_2),
                "val a : int",
                nameof(Constructors.MyClass3_2.a));

        [Test]
        [Category("Properties")]
        public void PropertySignature_4() =>
            TestPropertySignature(typeof(Constructors.MyClass3_2),
                "member this.b : int",
                nameof(Constructors.MyClass3_2.b));

        [Test]
        [Category("Properties")]
        public void PropertySignature_5() =>
            TestPropertySignature(typeof(Constructors.MyClass),
            @"member this.X : int with get, set",
            nameof(Constructors.MyClass.X));

        [Test]
        [Category("Properties")]
        [Category("Records")]
        public void PropertySignature_RecordTypeProperty_0() =>
            TestPropertySignature(typeof(Constructors.PetData),
                "name : string",
                nameof(Constructors.PetData.name));

        [Test]
        [Category("Properties")]
        [Category("Records")]
        public void PropertySignature_RecordTypeProperty_1() =>
            TestPropertySignature(typeof(Records.Car),
                "mutable Odometer : int",
                nameof(Records.Car.Odometer));

        [Test]
        [Category("Properties")]
        [Category("Records")]
        public void PropertySignature_RecordTypeProperty_2() =>
            TestPropertySignature(typeof(Records.Car),
                "Make : string",
                nameof(Records.Car.Make));

        [Test]
        [Category("Properties")]
        public void PropertySignature_6() =>
            TestPropertySignature(typeof(Methods.SomeType),
                "member this.SomeOtherMethod2 : int -> int * int -> int * int",
                nameof(Methods.SomeType.SomeOtherMethod2));

        [Test]
        [Category("Properties")]
        public void PropertySignature_7() =>
            TestPropertySignature(typeof(Methods.SomeType),
                "member this.SomeOtherMethod3 : (int -> int) * int -> int * int",
                nameof(Methods.SomeType.SomeOtherMethod3));
        #endregion

        #region Fields
        [Test]
        [Category("Fields")]
        public void FieldSignature_0() =>
            TestFieldSignature(typeof(Constructors.MyClass3_3),
                "val mutable b : int",
                nameof(Constructors.MyClass3_3.b));
        #endregion

        #region Interfaces
        [Test]
        [Category("Properties")]
        public void InterfaceSignature_0() =>
            TestPropertySignature(typeof(Interfaces), "Interfaces.x1 : Interfaces.SomeClass1", nameof(Interfaces.x1));

        #endregion

        #region FSharp.Core
        [Test]
        [Category("Types")]
        [Category("FSharpCore")]
        [Ignore("Test passes, but the number of members is too big to type out right now.")]
        public void TypeSignature_MailboxProcessor() =>
            TestTypeSignature(typeof(FSharpMailboxProcessor<>),
@"[<Sealed>]
type MailboxProcessor<'Msg> =
    interface IDisposable
    new : body:(MailboxProcessor<'Msg> -> Async<unit>) * cancellationToken:option<CancellationToken> -> MailboxProcessor<'Msg>");

        [Test]
        [Category("Constructors")]
        [Category("FSharpCore")]
        public void ConstructorSignature_MailboxProcessor() =>
            TestMethodSignature(typeof(FSharpMailboxProcessor<>),
                "new : body:(MailboxProcessor<'Msg> -> Async<unit>) * cancellationToken:option<CancellationToken> -> MailboxProcessor<'Msg>",
                ".ctor");

        [Test]
        [Category("Methods")]
        [Category("FSharpCore")]
        public void MethodSignature_MailboxProcessor_0() =>
            TestMethodSignature(typeof(FSharpMailboxProcessor<>),
                "member this.TryPostAndReply : buildMessage:(AsyncReplyChannel<'Reply> -> 'Msg) * timeout:option<int> -> option<'Reply>",
                "TryPostAndReply");

        [Test]
        [Category("Methods")]
        [Category("FSharpCore")]
        public void MethodSignature_MailboxProcessor_1() =>
            TestMethodSignature(typeof(FSharpMailboxProcessor<>),
                "member this.Scan : scanner:('Msg -> option<Async<'T>>) * timeout:option<int> -> Async<'T>",
                "Scan");

        [Test]
        [Category("Methods")]
        [Category("FSharpCore")]
        public void MethodSignature_MailboxProcessor_2() =>
            TestMethodSignature(typeof(FSharpMailboxProcessor<>),
                "member this.TryScan : scanner:('Msg -> option<Async<'T>>) * timeout:option<int> -> Async<option<'T>>",
                "TryScan");

        [Test]
        [Category("Events")]
        [Category("FSharpCore")]
        public void MethodSignature_MailboxProcessor_3() =>
            TestEventSignature(typeof(FSharpMailboxProcessor<>),
                "member this.Error : Handler<Exception> ",
                "Error");

        [Test]
        [Category("Properties")]
        [Category("FSharpCore")]
        public void MethodSignature_MailboxProcessor_4() =>
            TestPropertySignature(typeof(FSharpMailboxProcessor<>),
                "member this.DefaultTimeout : int with get, set",
                "DefaultTimeout");

        [Test]
        [Category("Types")]
        [Category("FSharpCore")]
        public void TypeSignature_Map() =>
        TestTypeSignature(typeof(Collections.MDocTestMap<,>),
@"type Collections.MDocTestMap<'Key, 'Value> =
    interface Collections.MDocInterface<KeyValuePair<'Key, 'Value>>");

        [Test]
        [Category("Types")]
        [Category("FSharpCore")]
        public void PropertySignature_Map() =>
            TestPropertySignature(typeof(FSharpMap<,>),
                "member this.Item(key:'Key) : 'Value",
                "Item");

        [Test]
        [Category("Methods")]
        [Category("FSharpCore")]
        public void TypeSignature_FSharpCore_3() =>
            TestMethodSignature(typeof(FSharpMailboxProcessor<>),
                "member this.PostAndReply : buildMessage:(AsyncReplyChannel<'Reply> -> 'Msg) * timeout:option<int> -> 'Reply", 
                "PostAndReply");

        [Test]
        [Category("Methods")]
        [Category("FlexibleTypes")]
        [Category("FSharpCore")]
        public void TypeSignature_FSharpCore_4() =>
            TestMethodSignature(typeof(ExtraTopLevelOperators),
                "ExtraTopLevelOperators.array2D : rows:seq<#seq<'T>> -> 'T[,]",
                "CreateArray2D");
        #endregion

        #region Constraints
        [Test]
        [Category("Constraints")]
        public void TestConstraints_1() => 
            TestTypeSignature(typeof(Constraints.Class1<>),
@"type Constraints.Class1<'T (requires 'T :> Exception)> =
    new : unit -> Constraints.Class1<'T (requires 'T :> Exception)>");

        [Test]
        [Category("Constraints")]
        public void TestConstraints_2() => 
            TestTypeSignature(typeof(Constraints.Class2<>),
@"type Constraints.Class2<'T (requires 'T :> IComparable)> =
    new : unit -> Constraints.Class2<'T (requires 'T :> IComparable)>");

        [Test]
        [Category("Constraints")]
        public void TestConstraints_2_1() => 
            TestTypeSignature(typeof(Constraints.Class2_1<>),
@"type Constraints.Class2_1<'T (requires 'T :> IComparable and 'T :> Exception)> =
    new : unit -> Constraints.Class2_1<'T (requires 'T :> IComparable and 'T :> Exception)>");

        [Test]
        [Category("Constraints")]
        public void TestConstraints_2_2() => 
            TestTypeSignature(typeof(Constraints.Class2_2<>),
@"type Constraints.Class2_2<'T (requires 'T :> IComparable and 'T :> seq<'T>)> =
    new : unit -> Constraints.Class2_2<'T (requires 'T :> IComparable and 'T :> seq<'T>)>");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_3() => 
            TestTypeSignature(typeof(Constraints.Class3<>),
                "type Constraints.Class3<'T (requires 'T : null)> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_4() => 
            TestTypeSignature(typeof(Constraints.Class4<>),
                "type Constraints.Class4<'T (requires 'T : (static member staticMethod1 : unit -> 'T)) > = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_5() => 
            TestTypeSignature(typeof(Constraints.Class5<>),
                "type Constraints.Class5<'T (requires 'T : (member Method1 : 'T -> int))> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_6() => 
            TestTypeSignature(typeof(Constraints.Class6<>),
                "type Constraints.Class6<'T (requires 'T : (member Property1 : int))> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_7() => 
            TestTypeSignature(typeof(Constraints.Class7<>),
                "type Constraints.Class7<'T (requires 'T : (new : unit -> 'T))> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_8() => 
            TestTypeSignature(typeof(Constraints.Class8<>),
                "type Constraints.Class8<'T (requires 'T : not struct)> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_9() => 
            TestTypeSignature(typeof(Constraints.Class9<>),
                "type Constraints.Class9<'T (requires 'T : enum<uint32>)> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_10() => 
            TestTypeSignature(typeof(Constraints.Class10<>),
                "type Constraints.Class10<'T (requires 'T : comparison)> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_11() => 
            TestTypeSignature(typeof(Constraints.Class11<>),
                "type Constraints.Class11<'T (requires 'T : equality)> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_12() => 
            TestTypeSignature(typeof(Constraints.Class12<>),
                "type Constraints.Class12<'T (requires 'T : delegate<obj * System.EventArgs, unit>)> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_13() => 
            TestTypeSignature(typeof(Constraints.Class13<>),
                "type Constraints.Class13<'T (requires 'T : unmanaged)> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_14() => 
            TestTypeSignature(typeof(Constraints.Class14<,>),
                "type Constraints.Class14<'T,'U (requires 'T : equality and 'U : equality)> = class end");

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_15() =>
            TestMethodSignature(typeof(Constraints.Class15),
                "static member add : 'T * 'T -> 'T (requires 'T : static member ( + ))",
                nameof(Constraints.Class15.add));

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_15_2() =>
            TestMethodSignature(typeof(Constraints.Class15),
                "static member heterogenousAdd : 'T * 'U -> 'T (requires 'T : static member ( + ))",
                nameof(Constraints.Class15.heterogenousAdd));

        [Test]
        [Category("Constraints")]
        [Ignore("No constraint info in IL code")]
        public void TestConstraints_16() =>
            TestMethodSignature(typeof(Constraints.Class16),
                "static member add : 'T * 'T -> 'T (requires 'T : static member ( + ))",
                nameof(Constraints.Class17.method));

        [Test]
        [Category("Constraints")]
        public void TestConstraints_17() =>
            TestMethodSignature(typeof(Constraints.Class17),
                "static member method : value1:'T * value2:'T -> unit (requires 'T : null)",
                nameof(Constraints.Class17.method));

        [Test]
        [Category("Constraints")]
        public void TestConstraints_18() =>
            TestMethodSignature(typeof(Constraints.Class18),
@"static member method : value1:obj * value2:obj -> unit",
                nameof(Constraints.Class18.method));
        
        [Test]
        [Ignore("No constraint info in IL code")]
        public void TestFSharpConstraints()
        {
            Constraints.Class1<Exception> c1 = new Constraints.Class1<Exception>();
            Constraints.Class2<IComparable> c2 = new Constraints.Class2<IComparable>();
            Constraints.Class2_1<ComparableException> c2_1 = new Constraints.Class2_1<ComparableException>();
            Constraints.Class3<EmptyClass> c3 = new Constraints.Class3<EmptyClass>();
            Constraints.Class4<EmptyClass> c4 = new Constraints.Class4<EmptyClass>();
            Constraints.Class5<EmptyClass> c5 = new Constraints.Class5<EmptyClass>();
            Constraints.Class6<EmptyClass> c6 = new Constraints.Class6<EmptyClass>();
            Constraints.Class7<EmptyClass> c7 = new Constraints.Class7<EmptyClass>();
            Constraints.Class8<EmptyClass> c8 = new Constraints.Class8<EmptyClass>();
            Constraints.Class9<EmptyClass> c9 = new Constraints.Class9<EmptyClass>();
            Constraints.Class10<EmptyClass> c10 = new Constraints.Class10<EmptyClass>();
            Constraints.Class11<EmptyClass> c11 = new Constraints.Class11<EmptyClass>();
            Constraints.Class12<EmptyClass> c12 = new Constraints.Class12<EmptyClass>();
            Constraints.Class13<EmptyClass> c13 = new Constraints.Class13<EmptyClass>();
            Constraints.Class14<EmptyClass, EmptyClass> c14 = new Constraints.Class14<EmptyClass, EmptyClass>();
            Constraints.Class15 c15 = new Constraints.Class15();


            Constraints.Class16.method(new EmptyClass(), new EmptyClass());
            Constraints.Class17.method(new EmptyClass(), new EmptyClass());
            Constraints.Class18.method(new EmptyClass(), new EmptyClass());

            Constraints.Class15.add(new EmptyClass(), new EmptyClass());
            Constraints.Class15.heterogenousAdd(new EmptyClass(), new EmptyStruct());
        }

        private class ComparableException : Exception, IComparable
        {
            public int CompareTo(object obj)
            {
                return 0;
            }
        }

        private class EmptyClass
        {
        }
        private struct EmptyStruct
        {
        }

        #endregion

        #region Operators
        [Test]
        [Category("Operators")]
        public void Operators_0() =>
            TestMethodSignature(typeof(OperatorsOverloading.Vector),
"static member ( * ) : v:OperatorsOverloading.Vector * a:double -> OperatorsOverloading.Vector",
"op_Multiply");
        // Well, there would be OperatorsOverloading.Vector -> double in MSDN, but there are no signs of currying in IL code.
        // Moreover Visual Studio QuickInfo shows Vector * double 

        [Test]
        [Category("Operators")]
        public void Operators_1() =>
            TestMethodSignature(typeof(OperatorsOverloading.Vector),
@"static member ( |+-+ ) : a:int * v:OperatorsOverloading.Vector -> OperatorsOverloading.Vector",
"op_BarPlusMinusPlus");
        
        [Test]
        [Category("Operators")]
        [Category("FSharpCore")]
        public void Operators_2() =>
            TestMethodSignature(typeof(Operators),
@"( << ) : func2:('T2 -> 'T3) -> func1:('T1 -> 'T2) -> ('T1 -> 'T3)",
"op_ComposeLeft");

        [Test]
        [Category("Operators")]
        public void Operators_3() =>
            TestMethodSignature(typeof(OperatorGlobalLevel),
@"( +? ) : x:int -> y:int -> int",
"op_PlusQmark");

        #endregion

        #region UnitsOfMeasure

        [Test]
        [Category("UnitsOfMeasure")]
        public void UnitsOfMeasure_0() =>
            TestPropertySignature(typeof(UnitsOfMeasure),
@"UnitsOfMeasure.xvec : UnitsOfMeasure.vector3D",
nameof(UnitsOfMeasure.xvec));

        [Test]
        [Category("UnitsOfMeasure")]
        public void UnitsOfMeasure_1() =>
            TestTypeSignature(typeof(UnitsOfMeasure.vector3D),
@"[<Sealed>]
type UnitsOfMeasure.vector3D = {}");
        #endregion

        #region Extensions
        [Test]
        [Category("Extensions")]
        public void Extensions_0() =>
            TestMethodSignature(typeof(Extensions.MyModule1.MyClass),
@"member this.G : unit -> int", nameof(Extensions.MyModule1.MyClass.G));

        [Test]
        [Category("Extensions")]
        public void Extensions_1() =>
            TestMethodSignature(typeof(Extensions),
@"Extensions.Int32.FromString : s:string -> int",
"Int32.FromString");
        #endregion
    }
}