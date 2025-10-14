module Constructors

type MyClass(x0, y0, z0) =
    let mutable x = x0
    let mutable y = y0
    let mutable z = z0
    do
        printfn "Initialized object that has coordinates (%d, %d, %d)" x y z
    member this.X with get() = x and set(value) = x <- value
    member this.Y with get() = y and set(value) = y <- value
    member this.Z with get() = z and set(value) = z <- value
    new() = MyClass(0, 0, 0)

type MyClassObjectParameters(x0:string, y0, z0) =
    let mutable x = x0
    let mutable y = y0
    let mutable z = z0
    member this.X with get() = x and set(value) = x <- value
    member this.Y with get() = y and set(value) = y <- value
    member internal this.Z with get() = z and set(value) = z <- value
  
//    new() = MyClassObjectParameters("", 0, 0)
//    new(x0:string) = MyClassObjectParameters("", x0, x0)

type MyStruct =
    struct
       val X : int
       val Y : int
       val Z : int
       new(x, y, z) = { X = x; Y = y; Z = z }
    end

let myStructure1 = new MyStruct(1, 2, 3)


// Error Each argument of the primary constructor for a struct must be given a type, 
// for example 'type S(x1:int, x2: int) = ...'. 
// These arguments determine the fields of the struct

type MyStruct2 =
    struct
       [<DefaultValue>] 
       val mutable X : int

       [<DefaultValue>] 
       val mutable Y : int

       [<DefaultValue>] 
       val mutable Z : int
    end

let myStructure2 = new MyStruct2()



type MyClass3 =
    val a : int
    val b : int
    // The following version of the constructor is an error
    // because b is not initialized.
    // new (a0, b0) = { a = a0; }
    // The following version is acceptable because all fields are initialized.
    new(a0, b0) = { a = a0; b = b0; }


type MyClass3_1 (a0, b0)=
    let a : int = a0
    let b : int = b0
    //val c : int

type MyClass3_2 =
    val a : int
    member this.b : int = 19

type MyClass3_3() =
    [<DefaultValue>] val mutable internal a : int
    [<DefaultValue>] val mutable b : int    

type MyClass3_4 (a0, b0) =
    [<DefaultValue>] val mutable a : int
    [<DefaultValue>] val mutable b : int

let myClassObj = new MyClass3(35, 22)
printfn "%d %d" (myClassObj.a) (myClassObj.b)

// type MyStruct3 (a0, b0) =
// Each argument of the primary constructor for a struct must be given a type, 
// for example 'type S(x1:int, x2: int) = ...'. 
// These arguments determine the fields of the struct
type MyStruct33 (a0:int, b0:int) = 
    struct 
        [<DefaultValue>] val mutable a : int
        [<DefaultValue>] val mutable b : int

        new (a0:int) = MyStruct33(a0, 0)
        new (a0:int, b0:int, c0:int) = MyStruct33(a0, b0)
    end

let myStruct = new MyStruct33()
let myStruct2 = new MyStruct33(10, 15)

type MyStruct44 (a0:int, b0:int) = 
    struct 
        [<DefaultValue>] val mutable a : int
        [<DefaultValue>] val mutable b : int
    end


type MyStruct55 (a0:int, b0:int) = 
    struct 
        [<DefaultValue>] val mutable a : int
        [<DefaultValue>] val mutable b : int
        new (a0:int) = MyStruct55(34, 12) //then {this.a = 71}
    end

type MyStruct66 = 
    struct 
        val a : int
        val b : int
        new (a0:int) = {a = a0; b = 83}
    end

type MyStruct77 = 
    struct 
        [<DefaultValue>] val mutable a : int
        val b : int
//        new (a0:int) = {b = 83; a = 12} // doesn't work
        new (a0:int) = {b = 83}
    end

type MyStruct88 = 
    struct 
        [<DefaultValue>] val mutable a : int
        val b : int
//        new (a0:int) = {b = 83; a = 12} // doesn't work
        new (a0:int) = {b = 83}
        new (a0:int, b0:int) = {b = 87}
    end



type PetData = {
    name : string
    age : int
    animal : string
}

type Pet(name:string, age:int, animal:string) =
    let mutable age = age
    let mutable animal = animal

    new (name:string) =
        Pet(name, 5, "dog")

    new (data:PetData) =
        Pet(data.name, data.age, data.animal) then System.Console.WriteLine("Pet created from PetData")



type public MyType =
  val private myvar: int
  val private myvar2: string

  new () = 
    for i in 1 .. 10 do
      printfn "Before field assignments %i" i
    { myvar = 1; myvar2 = "test" } 
    then 
      for i in 1 .. 10 do
        printfn "After field assignments %i" i




//A primary constructor in a class can execute code in a do binding. 
// However, what if you have to execute code in an additional constructor, without a do binding? 
// To do this, you use the then keyword.

 // Executing side effects in the primary constructor and
// additional constructors.
type Person(nameIn : string, idIn : int) =
    let mutable name = nameIn
    let mutable id = idIn
    do printfn "Created a person object."
    member this.Name with get() = name and set(v) = name <- v
    member this.ID with get() = id and set(v) = id <- v
    new() =
        Person("Invalid Name", -1)
        then
            printfn "Created an invalid person object."
    new(person : Person) =
        Person(person.Name, person.ID)
        then
            printfn "Created a copy of person object."

let person1 = new Person("Humberto Acevedo", 123458734)
let person2 = new Person()
let person3 = new Person(person1)




// Self Identifiers in Constructors
// In other members, you provide a name for the current 
// object in the definition of each member. 
// You can also put the self identifier on the first line of the class definition 
// by using the as keyword immediately following the constructor parameters. 
// The following example illustrates this syntax.+
type MyClass1(x) as this =
    // This use of the self identifier produces a warning - avoid.
    let x1 = this.X
    // This use of the self identifier is acceptable.
    do printfn "Initializing object with X =%d" this.X
    member this.X = x



// In additional constructors, you can also define a self identifier 
// by putting the as clause right after the constructor parameters. 
// The following example illustrates this syntax.
type MyClass2(x : int) =
    member this.X = x
    new() as this = MyClass2(0) then printfn "Initializing with X = %d" this.X



// Assigning Values to Properties at Initialization
// You can assign values to the properties of a class object in the initialization code 
// by appending a list of assignments of the form property = value 
// to the argument list for a constructor. This is shown in the following code example.
type Account() =
    let mutable balance = 0.0
    let mutable number = 0
    let mutable firstName = ""
    let mutable lastName = ""
    member this.AccountNumber
       with get() = number
       and set(value) = number <- value
    member this.FirstName
       with get() = firstName
       and set(value) = firstName <- value
    member this.LastName
       with get() = lastName
       and set(value) = lastName <- value
    member this.Balance
       with get() = balance
       and set(value) = balance <- value
    member this.Deposit(amount: float) = this.Balance <- this.Balance + amount
    member this.Withdraw(amount: float) = this.Balance <- this.Balance - amount


let account1 = new Account(AccountNumber=8782108,
                           FirstName="Darren", LastName="Parker",
                           Balance=1543.33)




// The following version of the previous code illustrates the combination 
// of ordinary arguments, optional arguments, and property settings in one constructor call.
type Account2(accountNumber : int, ?first: string, ?last: string, ?bal : float) =
   let mutable balance = defaultArg bal 0.0
   let mutable number = accountNumber
   let mutable firstName = defaultArg first ""
   let mutable lastName = defaultArg last ""
   member this.AccountNumber
      with get() = number
      and set(value) = number <- value
   member this.FirstName
      with get() = firstName
      and set(value) = firstName <- value
   member this.LastName
      with get() = lastName
      and set(value) = lastName <- value
   member this.Balance
      with get() = balance
      and set(value) = balance <- value
   member this.Deposit(amount: float) = this.Balance <- this.Balance + amount
   member this.Withdraw(amount: float) = this.Balance <- this.Balance - amount


let account2 = new Account2(8782108, bal = 543.33,
                          FirstName="Raman", LastName="Iyer")



// Constructors and Inheritance
type MyClassBase2(x: int) =
   let mutable z = x * x
   do for i in 1..z do printf "%d " i


type MyClassDerived2(y: int) =
   inherit MyClassBase2(y * 2)
   do for i in 1..y do printf "%d " i


// In the case of multiple constructors, the following code can be used. 
// The first line of the derived class constructors is the inherit clause, 
// and the fields appear as explicit fields that are declared with the val keyword. 
// For more information, see Explicit Fields: The val Keyword.+
type BaseClass =
    val string1 : string
    new (str) = { string1 = str }
    new () = { string1 = "" }

type DerivedClass =
    inherit BaseClass

    val string2 : string
    new (str1, str2) = { inherit BaseClass(str1); string2 = str2 }
    new (str2) = { inherit BaseClass(); string2 = str2 }

let obj1 = DerivedClass("A", "B")
let obj2 = DerivedClass("A")
