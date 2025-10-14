module Animals

// Call a base class from a derived one.
type Animal() =
    member __.Rest() = ()

type Dog() =
    inherit Animal()
    member __.Run() =
        base.Rest()

// Upcasting is denoted by :> operator.
let dog = Dog() 
let animal = dog :> Animal

//Dynamic downcasting (:?>) might throw an InvalidCastException if the cast doesn't succeed at runtime.
let shouldBeADog = animal :?> Dog
