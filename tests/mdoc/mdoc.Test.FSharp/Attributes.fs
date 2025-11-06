module Attributes

open System


type OwnerAttribute(name : string) =
    inherit System.Attribute()

type CompanyAttribute(name : string) =
    inherit System.Attribute()


[<Owner("Jason Carlson")>]
[<Company("Microsoft")>]
type SomeType1 = class end



[<AttributeUsage(AttributeTargets.Event ||| AttributeTargets.Module ||| AttributeTargets.Delegate, AllowMultiple = false)>]
type TypeWithFlagAttribute = class
    member this.X = "F#"
end