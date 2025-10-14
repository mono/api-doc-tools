module Properties

type MyPropertiesType() =
    let mutable myInternalValue = 0

    // A read-only property.
    member this.MyReadOnlyProperty = myInternalValue

    // A write-only property.
    member this.MyWriteOnlyProperty with set (value) = myInternalValue <- value

    // A read-write property.
    member this.MyReadWriteProperty
        with get () = myInternalValue
        and set (value) = myInternalValue <- value

type MyPropertyClass2(property1 : int) =
    member val Property1 = property1
    member val Property2 = "" with get, set

type MyAutoPropertyClass() =
    let random  = new System.Random()
    member val AutoProperty = random.Next() with get, set
    member this.ExplicitProperty = random.Next()