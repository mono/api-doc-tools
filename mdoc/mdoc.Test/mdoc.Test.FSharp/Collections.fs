module Collections
open FSharp.Collections
open System
open System.Collections
open System.Collections.Generic

let f (x:Map<int, int>) = 0

let f2 (x:seq<int>) = 0

type MDocInterface<'key> = interface
end

type MDocTestMap<'Key, 'Value> = class
    interface MDocInterface<KeyValuePair<'Key, 'Value>>


end