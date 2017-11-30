module OperatorsOverloading

// The following code illustrates a vector class that has just two operators, one for unary minus 
// and one for multiplication by a scalar.  In the example, two overloads for scalar multiplication are needed 
// because the operator must work regardless of the order in which the vector and scalar appear.+
type Vector(x: float, y : float) =
   member this.x = x
   member this.y = y
   static member (~-) (v : Vector) =
     Vector(-1.0 * v.x, -1.0 * v.y)
   static member (*) (v : Vector, a) =
     Vector(a * v.x, a * v.y)
   static member (^^^) (a, v: Vector) =
     Vector(a * v.x, a * v.y)
   static member (?<-) (a, v, b: Vector) =
     Vector(a * b.x, a * b.y)
   static member (|+-+) (a : int, v: Vector) =
     Vector(0.0, 0.0)
   static member ( ~~~ ) (v: Vector) =
     Vector(0.0, 0.0)
   static member ( + ) (v: Vector, v2: Vector) =
     Vector(0.0, 0.0)
   static member ( .. .. ) (start, step, finish) =
     Vector(0.0, 0.0)
   static member ( .. ) (start, finish) =
     Vector(0.0, 0.0)
   override this.ToString() =
     this.x.ToString() + " " + this.y.ToString()

let v1 = Vector(1.0, 2.0)
let v2 = v1 * 2.0
let v4 = - v2
let v5 = 1 |+-+ v2
let v7 = ~~~ v4

printfn "%s" (v1.ToString())
printfn "%s" (v2.ToString())
printfn "%s" (v4.ToString())

let v9 : ('T2 -> 'T3) -> ('T1 -> 'T2) -> ('T1 -> 'T3) = Operators.(<<)
let v10 : ('T2 -> 'T3) -> ('T1 -> 'T2) -> ('T1 -> 'T3) = Operators.(<<)

