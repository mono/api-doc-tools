module OperatorGlobalLevel

// You can also define operators at the global level. 
// The following code defines an operator +?.
let inline (+?) (x: int) (y: int) = x + 2*y

let i = (+?)

printf "%d" (10 +? 1)