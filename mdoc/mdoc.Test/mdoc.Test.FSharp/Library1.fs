namespace mdoc.Test.FSharp

type Class1() = 
    member this.X = "F#"

    member this.T = Functions.function6(1, "32")

type ClassPipes() = 
    let cylinderVolume (radius : float) (length : float) : float = radius * length
    let smallPipeRadius = 2.0
    let bigPipeRadius = 3.0

    // These define functions that take the length as a remaining
    // argument:

    let smallPipeVolume = cylinderVolume(smallPipeRadius)
    let bigPipeVolume = cylinderVolume bigPipeRadius

    let length1 = 30.0
    let length2 = 40.0
    member this.smallPipeVol1 = smallPipeVolume length1
    member this.smallPipeVol2 = smallPipeVolume length2
    member this.bigPipeVol1 = bigPipeVolume length1
    member this.bigPipeVol2 = bigPipeVolume length2
    member this.ff = Quotations.Var.Global("", typedefof<ClassPipes>)
    //member this.ff2 = Quotations.Var.Global "" typedefof<ClassPipes>
    member this.ff3 = Array.blit 
    //member this.ff4 = Array.averageBy None None

