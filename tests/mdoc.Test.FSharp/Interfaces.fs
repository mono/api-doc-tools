module Interfaces

// You can implement one or more interfaces in a class type by using the interface keyword, 
// the name of the interface, and the with keyword, followed by the interface member definitions, 
// as shown in the following code.
type IPrintable =
   abstract member Print : unit -> unit
   abstract member MyReadOnlyProperty :int

type SomeClass1(x: int, y: float) =
   interface IPrintable with
      member this.Print() = printfn "%d %f" x y
      member this.MyReadOnlyProperty = 10 

// To call the interface method when you have an object of type SomeClass, 
// you must upcast the object to the interface type, as shown in the following code.+
let x1 = new SomeClass1(1, 2.0)
(x1 :> IPrintable).Print()


// An alternative is to declare a method on the object that upcasts and calls the interface method, 
// as in the following example.
type SomeClass2(x: int, y: float) =
   member this.Print() = (this :> IPrintable).Print()
   interface IPrintable with
      member this.Print() = printfn "%d %f" x y
      member this.MyReadOnlyProperty = 10 

let x2 = new SomeClass2(1, 2.0)
x2.Print()

// Interface Inheritance
type Interface0 = interface
    abstract member Method1 : int -> int
end

// Interface Inheritance
type Interface1 =
    abstract member Method1 : int -> int

type Interface2 =
    abstract member Method2 : int -> int

type Interface3 =
    inherit Interface1
    inherit Interface2
    abstract member Method3 : int -> int

type MyClass() =
    interface Interface3 with
        member this.Method1(n) = 2 * n
        member this.Method2(n) = n + 100
        member this.Method3(n) = n / 10