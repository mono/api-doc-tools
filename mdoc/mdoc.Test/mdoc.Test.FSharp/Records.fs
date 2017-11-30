module Records

type MyRecord = {
    X: int;
    Y: int;
    Z: int
    }

let myRecord1 = { X = 1; Y = 2; Z = 3; }

type Car = {
    Make : string
    Model : string
    mutable Odometer : int
    }
let myCar = { Make = "Fabrikam"; Model = "Coupe"; Odometer = 108112 }
myCar.Odometer <- myCar.Odometer + 21