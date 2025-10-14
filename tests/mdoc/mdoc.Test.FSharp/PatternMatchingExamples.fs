module PatternMatching.PatternMatchingExamples

// Patterns are rules for transforming input data. They are used throughout the F# language 
// to compare data with a logical structure or structures, decompose data into constituent parts, 
// or extract information from data in various ways.1



// Constant Patterns
// The following example demonstrates the use of literal patterns, and also uses a variable pattern and an OR pattern.+
[<Literal>]
let Three = 3

let filter123 x =
    match x with
    // The following line contains literal patterns combined with an OR pattern.
    | 1 | 2 | Three -> printfn "Found 1, 2, or 3!"
    // The following line contains a variable pattern.
    | var1 -> printfn "%d" var1

for x in 1..10 do filter123 x

// Another example of a literal pattern is a pattern based on enumeration constants. 
// You must specify the enumeration type name when you use enumeration constants.
type Color =
    | Red = 0
    | Green = 1
    | Blue = 2

let printColorName (color:Color) =
    match color with
    | Color.Red -> printfn "Red"
    | Color.Green -> printfn "Green"
    | Color.Blue -> printfn "Blue"
    | _ -> ()

printColorName Color.Red
printColorName Color.Green
printColorName Color.Blue



// Identifier Patterns
// The option type is a discriminated union that has two cases, Some and None. One case (Some) has a value, 
// but the other (None) is just a named case. Therefore, Some needs to have a variable for the value associated 
// with the Some case, but None must appear by itself. In the following code, the variable var1 is given the value 
// that is obtained by matching to the Some case.
let printOption (data : int option) =
    match data with
    | Some var1  -> printfn "%d" var1
    | None -> ()

// In the following example, the PersonName discriminated union contains a mixture of strings 
// and characters that represent possible forms of names. The cases of the discriminated union are FirstOnly, 
// LastOnly, and FirstLast.
type PersonName =
    | FirstOnly of string
    | LastOnly of string
    | FirstLast of string * string

let constructQuery personName =
    match personName with
    | FirstOnly(firstName) -> printf "May I call you %s?" firstName
    | LastOnly(lastName) -> printf "Are you Mr. or Ms. %s?" lastName
    | FirstLast(firstName, lastName) -> printf "Are you %s %s?" firstName lastName

// For discriminated unions that have named fields, you use the equals sign (=) to extract the value of a named field. 
// For example, consider a discriminated union with a declaration like the following.+
type Shape =
    | Rectangle of height : float * width : float
    | Circle of radius : float

// You can use the named fields in a pattern matching expression as follows.
let matchShape shape =
    match shape with
    | Rectangle(height = h) -> printfn "Rectangle with length %f" h
    | Circle(r) -> printfn "Circle with radius %f" r

// When you specify multiple fields, use the semicolon (;) as a separator.+
let matchShape2 shape =
    match shape with
    | Rectangle(height = h; width = w) -> printfn "Rectangle with height %f and width %f" h w
    | _ -> ()



// Variable Patterns
// The following example demonstrates a variable pattern within a tuple pattern.+
let function1 x =
    match x with
    | (var1, var2) when var1 > var2 -> printfn "%d is greater than %d" var1 var2
    | (var1, var2) when var1 < var2 -> printfn "%d is less than %d" var1 var2
    | (var1, var2) -> printfn "%d equals %d" var1 var2

function1 (1,2)
function1 (2, 1)
function1 (0, 0)



// as Pattern
let (var1, var2) as tuple1 = (1, 2)
printfn "%d %d %A" var1 var2 tuple1

// OR Pattern
let detectZeroOR point =
    match point with
    | (0, 0) | (0, _) | (_, 0) -> printfn "Zero found."
    | _ -> printfn "Both nonzero."
detectZeroOR (0, 0)
detectZeroOR (1, 0)
detectZeroOR (0, 10)
detectZeroOR (10, 15)

// AND Pattern
let detectZeroAND point =
    match point with
    | (0, 0) -> printfn "Both values zero."
    | (var1, var2) & (0, _) -> printfn "First value is 0 in (%d, %d)" var1 var2
    | (var1, var2)  & (_, 0) -> printfn "Second value is 0 in (%d, %d)" var1 var2
    | _ -> printfn "Both nonzero."
detectZeroAND (0, 0)
detectZeroAND (1, 0)
detectZeroAND (0, 10)
detectZeroAND (10, 15)

// Cons Pattern
let list1 = [ 1; 2; 3; 4 ]
// This example uses a cons pattern and a list pattern.
let rec printList l =
    match l with
    | head :: tail -> printf "%d " head; printList tail
    | [] -> printfn ""

printList list1

//List Pattern
// This example uses a list pattern.
let listLength list =
    match list with
    | [] -> 0
    | [ _ ] -> 1
    | [ _; _ ] -> 2
    | [ _; _; _ ] -> 3
    | _ -> List.length list

printfn "%d" (listLength [ 1 ])
printfn "%d" (listLength [ 1; 1 ])
printfn "%d" (listLength [ 1; 1; 1; ])
printfn "%d" (listLength [ ] )

// Array Pattern
// This example uses array patterns.
let vectorLength vec =
    match vec with
    | [| var1 |] -> var1
    | [| var1; var2 |] -> sqrt (var1*var1 + var2*var2)
    | [| var1; var2; var3 |] -> sqrt (var1*var1 + var2*var2 + var3*var3)
    | _ -> failwith "vectorLength called with an unsupported array size of %d." (vec.Length)

printfn "%f" (vectorLength [| 1. |])
printfn "%f" (vectorLength [| 1.; 1. |])
printfn "%f" (vectorLength [| 1.; 1.; 1.; |])
printfn "%f" (vectorLength [| |] )

// Parenthesized Pattern
let countValues list value =
    let rec checkList list acc =
       match list with
       | (elem1 & head) :: tail when elem1 = value -> checkList tail (acc + 1)
       | head :: tail -> checkList tail acc
       | [] -> acc
    checkList list 0

let result = countValues [ for x in -10..10 -> x*x - 4 ] 0
printfn "%d" result

let countValues2<'a when 'a : equality> : List<'a> -> 'a -> int = countValues

// Tuple Pattern
let detectZeroTuple point =
    match point with
    | (0, 0) -> printfn "Both values zero."
    | (0, var2) -> printfn "First value is 0 in (0, %d)" var2
    | (var1, 0) -> printfn "Second value is 0 in (%d, 0)" var1
    | _ -> printfn "Both nonzero."
detectZeroTuple (0, 0)
detectZeroTuple (1, 0)
detectZeroTuple (0, 10)
detectZeroTuple (10, 15)

// Record Pattern
// This example uses a record pattern.

type MyRecord = { Name: string; ID: int }

let IsMatchByName record1 (name: string) =
    match record1 with
    | { MyRecord.Name = nameFound; MyRecord.ID = _; } when nameFound = name -> true
    | _ -> false

let recordX = { Name = "Parker"; ID = 10 }
let isMatched1 = IsMatchByName recordX "Parker"
let isMatched2 = IsMatchByName recordX "Hartono"


// Wildcard Pattern
//Patterns That Have Type Annotations
let detect1 x =
    match x with
    | 1 -> printfn "Found a 1!"
    | (var1 : int) -> printfn "%d" var1
detect1 0
detect1 1


// Type Test Pattern
// open System.Windows.Forms
// 
// let RegisterControl(control:Control) =
//     match control with
//     | :? Button as button -> button.Text <- "Registered."
//     | :? CheckBox as checkbox -> checkbox.Text <- "Registered."
//     | _ -> ()

// Null Pattern
let ReadFromFile (reader : System.IO.StreamReader) =
    match reader.ReadLine() with
    | null -> printfn "\n"; false
    | line -> printfn "%s" line; true

let fs = System.IO.File.Open("..\..\Program.fs", System.IO.FileMode.Open)
let sr = new System.IO.StreamReader(fs)
while ReadFromFile(sr) = true do ()
sr.Close()