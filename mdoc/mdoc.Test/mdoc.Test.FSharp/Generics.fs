module Generics

// In the following code example, makeList is generic, 
// even though neither it nor its parameters are explicitly declared as generic.
let makeList a b = [a; b]

// You can also make a function generic by using the single quotation mark syntax 
// in a type annotation to indicate that a parameter type is a generic type parameter. 
// In the following code, function1 is generic because its parameters are declared in this manner, as type parameters.
let function1 (x: 'a) (y: 'a) =
    printfn "%A %A" x y

// You can also make a function generic by explicitly 
// declaring its type parameters in angle brackets (<type-parameter>)
let function2<'T> x y =
    printfn "%A, %A" x y


type Map2<[<EqualityConditionalOn>]'Key,[<EqualityConditionalOn>][<ComparisonConditionalOn>]'Value when 'Key : comparison and 'Value : comparison> = class
    //member this.Item ('Key) : 'Value (requires comparison)
//    member Item : key:'Key -> 'Value with get
    member this.fffff : option<int> = None
    member this.l : list<int> = [ 1; 2; 3 ]
    member this.c : Choice<int, float> = Choice1Of2 0
    member this.c2 : Choice<int, float> = Choice2Of2 0.5
    member this.r : ref<int>  = ref 0
    member this.s : seq<int>  = seq { for i in 1 .. 10 do yield i * i }    
end
