module Inheritance

//The following code example illustrates the declaration of 
// a new virtual method function1 in a base class 
// and how to override it in a derived class.
type MyClassBase1() =
   let mutable z = 0
   abstract member function1 : int -> int
   default u.function1(a : int) = z <- z + a; z

type MyClassDerived1() =
   inherit MyClassBase1()
   override u.function1(a: int) = a + 1


//The following code shows a base class and a derived class, 
// where the derived class calls the base class constructor in the inherit clause:+
type MyClassBase2(x: int) =
   let mutable z = x * x
   do for i in 1..z do printf "%d " i

type MyClassDerived2(y: int) =
   inherit MyClassBase2(y * 2)
   do for i in 1..y do printf "%d " i



//In the case of multiple constructors, the following code can be used. 
// The first line of the derived class constructors is the inherit clause, 
// and the fields appear as explicit fields that are declared with the val keyword. 
// For more information, see Explicit Fields: The val Keyword.
type BaseClass =
    val string1 : string
    new (str) = { string1 = str }
    new () = { string1 = "" }

type DerivedClass =
    inherit BaseClass

    val string2 : string
    new (str1, str2) = { inherit BaseClass(str1); string2 = str2 }
    new (str2) = { inherit BaseClass(); string2 = str2 }

let obj1 = DerivedClass("A", "B")
let obj2 = DerivedClass("A")