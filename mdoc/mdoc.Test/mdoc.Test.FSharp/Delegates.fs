module Delegates

// The following code shows the syntax for creating delegates that represent various methods in a class.
// Depending on whether the method is a static method or an instance method, 
// and whether it has arguments in the tuple form or the curried form, 
// the syntax for declaring and assigning the delegate is a little different.

type Test1() =
  static member add(a : int, b : int) =
     a + b
  static member add2 (a : int) (b : int) =
     a + b

  member x.Add(a : int, b : int) =
     a + b
  member x.Add2 (a : int) (b : int) =
     a + b

let replicate n c = String.replicate n (string c)


// Delegate specifications must not be curried types. 
// Use 'typ * ... * typ -> typ' for multi-argument delegates, 
// and 'typ -> (typ -> typ)' for delegates returning function values.	

type Delegate1 = delegate of (int * int) -> int// Delegate1 works with tuple arguments.
type Delegate2 = delegate of int * int -> int // Delegate2 works with curried arguments.
type Delegate3 = delegate of int * char -> string
type Delegate4 = delegate of int -> (int -> char)
type Delegate5 = delegate of int -> (int -> char -> string)
type Delegate6 = delegate of (int -> float) -> char
type Delegate7 = delegate of (int -> char -> string) -> float
type Delegate8 = delegate of int -> char
type Delegate9 = delegate of (int * int) -> char
type Delegate10 = delegate of int * int -> char
type Delegate11 = delegate of char -> unit
type Delegate12 = delegate of unit -> char
type Delegate13 = delegate of (int -> char -> string -> decimal) -> float


let function1(i : int, i2 : int) = 1
let function2(i : int) (ch : int) = 1
let function3(i : int) (s : char) = ""
let function4(i : int) (ch : int) = ' '
let function5(i : int) (i2 : int) (ch : char) = ""
let function6(intIntFunction : int -> float) = ' '
let function7(intCharStringFunction : int -> char -> string) = 0.5
let function8(i : int) = ' '
let function9(i : (int * int)) = ' '
let function10(i : int) (i2 : int) = ' '
let function11(c : char) = ()
let function12(c : unit) = ' '
let function12_1() = ' '
let function13(intCharStringDecimalFunction : int -> char -> string -> decimal) = 0.5

let delObject1 = new Delegate1(function1)
let delObject2 = new Delegate2(function2)
let delObject3 = new Delegate3(function3)
let delObject4 = new Delegate4(function4)
let delObject5 = new Delegate5(function5)
let delObject6 = new Delegate6(function6)
let delObject7 = new Delegate7(function7)
let delObject8 = new Delegate8(function8)
let delObject9 = new Delegate9(function9)
let delObject10 = new Delegate10(function10)
let delObject11 = new Delegate11(function11)
let delObject12 = new Delegate12(function12)
let delObject12_1 = new Delegate12(function12_1)
let delObject13 = new Delegate13(function13)





let InvokeDelegate1 (dlg : Delegate1) (a : int) (b: int) =
   dlg.Invoke(a, b)
let InvokeDelegate2 (dlg : Delegate2) (a : int) (b: int) =
   dlg.Invoke(a, b)

// For static methods, use the class name, the dot operator, and the
// name of the static method.
let del1 : Delegate1 = new Delegate1( Test1.add )
let del2 : Delegate2 = new Delegate2( Test1.add2 )

let testObject = Test1()

// For instance methods, use the instance value name, the dot operator, and the instance method name.
let del3 : Delegate1 = new Delegate1( testObject.Add )
let del4 : Delegate2 = new Delegate2( testObject.Add2 )

for (a, b) in [ (100, 200); (10, 20) ] do
  printfn "%d + %d = %d" a b (InvokeDelegate1 del1 a b)
  printfn "%d + %d = %d" a b (InvokeDelegate2 del2 a b)
  printfn "%d + %d = %d" a b (InvokeDelegate1 del3 a b)
  printfn "%d + %d = %d" a b (InvokeDelegate2 del4 a b)

// The following code shows some of the different ways you can work with delegates.


// An F# function value constructed from an unapplied let-bound function
let function1_ = replicate

// A delegate object constructed from an F# function value
let delObject = new Delegate3(function1_)

// An F# function value constructed from an unapplied .NET member
let functionValue = delObject.Invoke

List.map (fun c -> functionValue(5,c)) ['a'; 'b'; 'c']
|> List.iter (printfn "%s")

// Or if you want to get back the same curried signature
let replicate' n c =  delObject.Invoke(n,c)

// You can pass a lambda expression as an argument to a function expecting a compatible delegate type
// System.Array.ConvertAll takes an array and a converter delegate that transforms an element from
// one type to another according to a specified function.
let stringArray = System.Array.ConvertAll([|'a';'b'|], fun c -> replicate' 3 c)
printfn "%A" stringArray
