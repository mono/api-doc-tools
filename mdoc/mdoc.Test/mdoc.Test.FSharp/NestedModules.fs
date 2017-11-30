module NestedModules

// Modules can be nested. Inner modules must be indented as far as outer module declarations 
// to indicate that they are inner modules, not new modules. 
module Y =
    let y = 1

    [<AutoOpen>]
    module Z =
        let z = 5

module X =
    let x = 2