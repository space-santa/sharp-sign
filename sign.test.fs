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
