module AlternativesToInheritance

open System

// In cases where a minor modification of a type is required, 
// consider using an object expression as an alternative to inheritance. 
// The following example illustrates the use of an object expression as an alternative 
// to creating a new derived type:
let object1 = { new Object() with
      override this.ToString() = "This overrides object.ToString()"
      }

printfn "%s" (object1.ToString())