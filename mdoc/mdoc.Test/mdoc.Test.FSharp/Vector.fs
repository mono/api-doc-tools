module SomeNamespace.SomeModule

// This example is a basic class with (1) local let bindings, (2) properties, (3) methods, and (4) static members.
type Vector(x : float, y : float) =
    let mag = sqrt(x * x + y * y) // (1)
    member this.X = x // (2)
    member this.Y = y
    member this.Mag = mag
    member this.Scale(s) = // (3)
        Vector(x * s, y * s)
    static member (+) (a : Vector, b : Vector) = // (4)
        Vector(a.X + b.X, a.Y + b.Y)


// Declare IVector interface and implement it in Vector'.
type IVector =
    abstract Scale : float -> IVector

type Vector'''(x, y) =
    interface IVector with
        member __.Scale(s) =
            Vector'''(x * s, y * s) :> IVector
    member __.X = x
    member __.Y = y



type Vector2(x, y) =
    interface IVector with
        member __.Scale(s) =
            Vector2(x * s, y * s) :> IVector
    member __.X = x
    member __.Y = y


