module Literals

[<Literal>]
let Literal1 = "a" + "b"

[<Literal>]
let FileLocation =   __SOURCE_DIRECTORY__ + "/" + __SOURCE_FILE__

[<Literal>]
let Literal2 = 1 ||| 64

[<Literal>]
let Literal3 = System.IO.FileAccess.Read ||| System.IO.FileAccess.Write

let someSbyte = 86y

let someByte = 86uy

let someBigint = 9999999999999999999999999999I

let public someDecimal = 0.7833M

let public someChar = 'a'

let public someString = "text\n"


