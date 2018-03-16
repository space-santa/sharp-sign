module SharpSign

open System

let getConsoleWidth =
    let p = new System.Diagnostics.Process();
    p.StartInfo.FileName <- "tput"
    p.StartInfo.Arguments <- "cols"
    p.StartInfo.RedirectStandardOutput <- true
    p.StartInfo.UseShellExecute <- false
    p.Start() |> ignore
    p.StandardOutput.ReadToEnd() |> int

let signWidth (line:String) =
    line.Length + 4

let maxLineLength =
    getConsoleWidth - 4

let rec splitTokenAtLimit (token:string) =
    if token.Length < maxLineLength then
        [token]
    else
        let first, second = Seq.toList token |> List.splitAt maxLineLength
        let mutable retval = [String.Concat first]

        if second.Length > maxLineLength then
            retval <- String.Concat second |> splitTokenAtLimit |> List.append retval
        else
            retval <- [String.Concat second] |> List.append retval

        retval

/// Maps tokens to a list of strings with each element shorter than length.
let makeLimitedStrings tokens =
    let tokenList = List.collect splitTokenAtLimit tokens
    let mutable s = ""
    let mutable newS = ""
    let mutable retval = []

    for t in tokenList do
        if s.Length > 0 then
            newS <- s + " " + t
        else
            newS <- t

        if newS.Length <= maxLineLength then
            s <- newS
        else
            retval <- List.append retval [s]
            s <- t

    retval <- List.append retval [s]

    retval

let longestLine (lines:string list) =
    let mutable length = 0

    for line in lines do
        if line.Length > length then
            length <- line.Length

    length

let wrapLine line length =
    let retval = "| " + line
    let multi = length - retval.Length + 2
    retval + String.replicate multi " " + " |"

let topBottomBorder length =
    "+" + String.replicate (length + 2) "-" + "+"

let makeSignString (args:string list) =
    let lines = makeLimitedStrings args
    let length = longestLine lines
    let mutable retval = topBottomBorder length + "\n"
    retval <- retval + wrapLine " " length + "\n"

    for line in lines do
        retval <- retval + wrapLine line length + "\n"

    retval <- retval + wrapLine " " length + "\n"
    retval + topBottomBorder length

[<EntryPoint>]
let main args =
    printfn "%s" <| makeSignString (Seq.toList args)
    0
