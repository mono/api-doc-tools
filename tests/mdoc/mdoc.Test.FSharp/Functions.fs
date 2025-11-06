module Functions

// You define functions by using the let keyword, or, if the function is recursive, the let rec keyword combination.+
let rec fib n = if n < 2 then 1 else fib (n - 1) + fib (n - 2)

let rec public publicLet n = if n < 2 then 1 else fib (n - 1) + fib (n - 2)

// Functions in F# can be composed from other functions. 
// The composition of two functions function1 and function2 is another function that represents the application of function1 followed the application of function2:
let function1 x = x + 1
let function2 x2 = x2 * 2
let function3 (x3) = x3 * 2
let function4 x4 y4 = x4 + y4
let function5 (x5, y5) = x5 + y5
let function6 (x6, y6) = ()
let function7 x7 (y7, z7) = ()
let function8 x8 y8 z8 = ()
let function9 (x9, y9) (z9, a9) = ()
let function10<'a> (x, y) (z, a) = ()
let function11<'a> (x, y, z) (a, b) = ()
let function12<'a> x (a, b, c, d, e) = ()
let function13<'a> (a:'a) = ()

let get_function x = x + 1

let h = function1 >> function2
let result5 = h 100

//Pipelining enables function calls to be chained together as successive operations. Pipelining works as follows:
let result = 100 |> function1 |> function2

type TestFunction() =
    member this.f13 : 'a -> unit = function13
    // member this.f13<'a> : 'a -> unit = function13 // Error	FS0671	A property cannot have explicit type parameters. Consider using a method instead.
    