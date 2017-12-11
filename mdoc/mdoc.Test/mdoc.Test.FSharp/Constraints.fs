module Constraints

// Base Type Constraint
type Class1<'T when 'T :> System.Exception>() = 
    class end

// Interface Type Constraint
type Class2<'T when 'T :> System.IComparable>() = 
    class end

// Interface Type Constraint
type Class2_1<'T when 'T :> System.IComparable and 'T :> System.Exception>() = 
    class end

// Interface Type Constraint with recursion
type Class2_2<'T when 'T :> System.IComparable and 'T :> seq<'T>>() = 
    class end

// Null constraint
type Class3<'T when 'T : null>() =
    class end

// Member constraint with static member
type Class4<'T when 'T : (static member staticMethod1 : unit -> 'T) >() =
    class end

// Member constraint with instance member
type Class5<'T when 'T : (member Method1 : 'T -> int)>() =
    class end

// Member constraint with property
type Class6<'T when 'T : (member Property1 : int)>() =
    class end

// Constructor constraint
type Class7<'T when 'T : (new : unit -> 'T)>() =
    class end
    //member val Field = new 'T()

// Reference type constraint
type Class8<'T when 'T : not struct>() =
    class end

// Enumeration constraint with underlying value specified
type Class9<'T when 'T : enum<uint32>>() =
    class end

// 'T must implement IComparable, or be an array type with comparable
// elements, or be System.IntPtr or System.UIntPtr. Also, 'T must not have
// the NoComparison attribute.
type Class10<'T when 'T : comparison>() =
    class end

// 'T must support equality. This is true for any type that does not
// have the NoEquality attribute.
type Class11<'T when 'T : equality>() =
    class end

type Class12<'T when 'T : delegate<obj * System.EventArgs, unit>>() =
    class end

type Class13<'T when 'T : unmanaged>() =
    class end

// If there are multiple constraints, use the and keyword to separate them.
type Class14<'T,'U when 'T : equality and 'U : equality>() =
    class end

type Class15() = class
    // Member constraints with two type parameters
    // Most often used with static type parameters in inline functions
    static member inline add(value1 : ^T when ^T : (static member (+) : ^T * ^T -> ^T), value2: ^T) = 
        value1 + value2

    // ^T and ^U must support operator +
    static member inline heterogenousAdd(value1 : ^T when (^T or ^U) : (static member (+) : ^T * ^U -> ^T), value2 : ^U) = 
        value1 + value2
end

type Class16() = class
    static member inline method(value1 : ^T when ^T : (static member (+) : ^T * ^T -> ^T), value2: ^T) = ()
end

type Class17() = class
    static member method<'T when 'T : null>(value1 : 'T, value2: 'T) = ()
end

type Class18() = class
    static member method(value1 : ^T, value2: ^T) = ()
end