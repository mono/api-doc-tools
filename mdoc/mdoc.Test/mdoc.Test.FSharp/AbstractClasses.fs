module AbstractClasses

// An abstract class that has some methods and properties defined
// and some left abstract.
[<AbstractClass>]
type Shape2D(x0 : float, y0 : float) =
    let mutable x, y = x0, y0
    let mutable rotAngle = 0.0

    // These properties are not declared abstract. They
    // cannot be overriden.
    member this.CenterX with get() = x and set xval = x <- xval

    // These properties are abstract, and no default implementation
    // is provided. Non-abstract derived classes must implement these.
    abstract Area : float with get

    // This method is not declared abstract. It cannot be
    // overriden.
    member this.Move dx dy =
       x <- x + dx
       y <- y + dy

       
    // An abstract method that is given a default implementation
    // is equivalent to a virtual method in other .NET languages.
    // Rotate changes the internal angle of rotation of the square.
    // Angle is assumed to be in degrees.
    abstract member Rotate: float -> unit
    default this.Rotate(angle) = rotAngle <- rotAngle + angle
    
    abstract member Rotate2: float -> unit
    member this.Rotate3: float -> unit = fun(ff) -> ()



type Square(x, y, sideLengthIn) =
    inherit Shape2D(x, y)
    member this.SideLength = sideLengthIn
    override this.Area = this.SideLength * this.SideLength
    override this.Rotate2(angle) = ()

type Circle(x, y, radius) =
    inherit Shape2D(x, y)
    let PI = 3.141592654
    member this.Radius = radius
    override this.Area = PI * this.Radius * this.Radius
    // Rotating a circle does nothing, so use the wildcard
    // character to discard the unused argument and
    // evaluate to unit.
    override this.Rotate(_) = ()
    //override this.Name = "Circle"
    override this.Rotate2(angle) = ()

let square1 = new Square(0.0, 0.0, 10.0)
let circle1 = new Circle(0.0, 0.0, 5.0)
circle1.CenterX <- 1.0
square1.Move -1.0 2.0
square1.Rotate 45.0
circle1.Rotate 45.0

let shapeList : list<Shape2D> = [ (square1 :> Shape2D);
                                  (circle1 :> Shape2D) ]
List.iter (fun (elem : Shape2D) ->
              printfn "Area of %s: %f" (elem.ToString()) (elem.Area))
          shapeList