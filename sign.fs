module SharpSign

open System

module ConsoleIO =
    let getConsoleWidth =
        let p = new System.Diagnostics.Process();
        p.StartInfo.FileName <- "tput"
        p.StartInfo.Arguments <- "cols"
        p.StartInfo.RedirectStandardOutput <- true
        p.StartInfo.UseShellExecute <- false
        p.Start() |> ignore
        p.StandardOutput.ReadToEnd() |> int


module StringOperations =
    let signWidth (line:String) =
        line.Length + 4

    let rec splitTokenAtLimit maxLength (token:string) =
        if maxLength = 0 then
            invalidArg "maxLength" "can't be zero"

        if token.Length < maxLength then
            [token]
        else
            let first, second = Seq.toList token |> List.splitAt maxLength
            let mutable retval = [String.Concat first]

            if second.Length > maxLength then
                retval <- String.Concat second |> splitTokenAtLimit maxLength |> List.append retval
            else if second.Length > 0 then
                retval <- [String.Concat second] |> List.append retval

            retval

    /// Maps tokens to a list of strings with each element shorter than length.
    let makeLimitedStrings maxLength tokens =
        let tokenList = List.collect (splitTokenAtLimit maxLength) tokens
        let mutable s = ""
        let mutable newS = ""
        let mutable retval = []

        for t in tokenList do
            if s.Length > 0 then
                newS <- s + " " + t
            else
                newS <- t

            if newS.Length <= maxLength then
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

    let makeSignString maxLength (args:string list) =
        let lines = makeLimitedStrings maxLength args
        let length = longestLine lines
        let mutable retval = topBottomBorder length + "\n"
        retval <- retval + wrapLine " " length + "\n"

        for line in lines do
            retval <- retval + wrapLine line length + "\n"

        retval <- retval + wrapLine " " length + "\n"
        retval + topBottomBorder length


module Combinator =
    let maxLineLength =
        ConsoleIO.getConsoleWidth - 4

    let makeSign (args:string list) =
        StringOperations.makeSignString maxLineLength args


[<EntryPoint>]
let main args =
    printfn "%s" <| Combinator.makeSign (Seq.toList args)
    0
