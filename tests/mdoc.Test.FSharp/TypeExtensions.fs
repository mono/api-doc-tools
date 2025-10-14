module TypeExtensions

module TypeExtensions1 =

    // Define a type.
    type MyClass() =
      member this.F() = 100

    // Define type extension.
    type MyClass with
       member this.G() = 200

module TypeExtensions2 =
   let function1 (obj1: TypeExtensions1.MyClass) =
      // Call an ordinary method.
      printfn "%d" (obj1.F())
      // Call the extension method.
      printfn "%d" (obj1.G())




// Define a new member method FromString on the type Int32.
type System.Int32 with
    member this.FromString( s : string ) =
       System.Int32.Parse(s)

let testFromString str =
    let mutable i = 0
    // Use the extension method.
    i <- i.FromString(str)
    printfn "%d" i

testFromString "500"




// Generic Extension Methods
open System.Collections.Generic

type IEnumerable<'T> with
    /// Repeat each element of the sequence n times
    member xs.RepeatElements(n: int) =
        seq { for x in xs do for i in 1 .. n do yield x }

//However, for a generic type, the type variable may not be constrained. 
// You can now declare a C#-style extension member in F# to work around this limitation. 
// When you combine this kind of declaration with the inline feature of F#, 
// you can present generic algorithms as extension members.
// Consider the following declaration:
open System.Runtime.CompilerServices
[<Extension>]
type ExtraCSharpStyleExtensionMethodsInFSharp () =
    [<Extension>]
    static member inline Sum(xs: IEnumerable<'T>) = Seq.sum xs

let listOfIntegers = [ 1 .. 100 ]
//let listOfBigIntegers = [ 1I to 100I ]
let listOfBigIntegers = [ 1I .. 100I ]
let sum1 = listOfIntegers.Sum()
let sum2 = listOfBigIntegers.Sum()
//In this code, the same generic arithmetic code is applied to lists of two types without overloading, 
// by defining a single extension member.+

