module FlexibleTypes

let iterate1 (f : unit -> seq<int>) =
    for e in f() do printfn "%d" e
let iterate2 (f : unit -> #seq<int>) =
    for e in f() do printfn "%d" e
let iterate3<'T when 'T :> seq<int>> (f : unit -> 'T) = ()
let iterate4<'T when 'T :> Customers.ICustomer> (f : unit -> 'T) = ()


