module InlineFunctions

// The following code example illustrates an inline function at the top level, 
// an inline instance method, and an inline static method.

let inline increment x = x + 1
type WrapInt32() =
    member inline this.incrementByOne(x) = x + 1
    static member inline Increment(x) = x + 1

// The presence of inline affects type inference. 
// This is because inline functions can have statically resolved type parameters, 
// whereas non-inline functions cannot. The following code example shows a case where inline 
// is helpful because you are using a function that has a statically resolved type parameter, the float conversion operator.
let inline printAsFloatingPoint number =
    printfn "%f" (float number)
