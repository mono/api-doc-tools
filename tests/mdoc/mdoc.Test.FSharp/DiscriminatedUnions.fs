module DiscriminatedUnions

//The preceding code declares a discriminated union Shape, 
// which can have values of any of three cases: Rectangle, Circle, and Prism. 
// Each case has a different set of fields.
type Shape =
    | Rectangle of width : float * length : float
    | Circle of radius : float
    | Prism of width : float * float * height : float

let rect = Rectangle(length = 1.3, width = 10.0)
let circ = Circle (1.0)
let prism = Prism(5., 2.0, height = 3.0)


type SizeUnion = Small | Medium | Large         // union
type ColorEnum = Red=5 | Yellow=7 | Blue=9      // enum

