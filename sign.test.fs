module SignTest

open NUnit.Framework
open FsUnit
open SharpSign

[<TestFixture>]
type Test() =

    [<Test>] member x.
        ``signWidth`` () =
            StringOperations.signWidth("abx") |> should equal 7


    [<Test>] member x.
        ``wrapLine`` () =
            StringOperations.wrapLine "abc" 8 |> should equal "| abc      |"

[<TestFixture>]
type TestSplitTokenAtLimit() =
    let str = "abcdefqwerty"
    [<Test>] member x.
        ``no split`` () =
            StringOperations.splitTokenAtLimit 13 str |> should equal [str]

    [<Test>] member x.
        ``still no split`` () =
            StringOperations.splitTokenAtLimit 12 str |> should equal [str]

    [<Test>] member x.
        ``split last`` () =
            StringOperations.splitTokenAtLimit 11 str |> should equal ["abcdefqwert"; "y"]

    [<Test>] member x.
        ``split first`` () =
            StringOperations.splitTokenAtLimit 1 str |>
                should equal ["a"; "b"; "c"; "d"; "e"; "f"; "q"; "w"; "e"; "r"; "t"; "y"]

    [<Test>] member x.
        ``split zero`` () =
            (fun () -> StringOperations.splitTokenAtLimit 0 str |> ignore) |>
                should throw typeof<System.ArgumentException>

[<TestFixture>]
type TestMakeSignString() =
    [<Test>] member x.
        ``empty`` () =
            (fun () -> StringOperations.makeSignString 8 [] |> ignore) |>
                should throw typeof<System.ArgumentException>

    [<Test>] member x.
        ``empty string`` () =
            (fun () -> StringOperations.makeSignString 8 [""] |> ignore) |>
                should throw typeof<System.ArgumentException>

    [<Test>] member x.
        `` one line`` () =
            StringOperations.makeSignString 8 ["abcd"] |>
                should equal "+------+\n|      |\n| abcd |\n|      |\n+------+"

    [<Test>] member x.
        `` one line two tokens`` () =
            StringOperations.makeSignString 8 ["abcd"; "e"] |>
                should equal "+--------+\n|        |\n| abcd e |\n|        |\n+--------+"

    [<Test>] member x.
        `` two line three tokens`` () =
            StringOperations.makeSignString 8 ["abcd"; "e"; "qwerty"] |>
                should equal "+--------+\n|        |\n| abcd e |\n| qwerty |\n|        |\n+--------+"

    [<Test>] member x.
        `` three line three tokens long`` () =
            StringOperations.makeSignString 8 ["abcd"; "e"; "qwertypoiu"] |>
                should equal "+----------+\n|          |\n| abcd e   |\n| qwertypo |\n| iu       |\n|          |\n+----------+"
