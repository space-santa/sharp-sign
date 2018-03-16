module SignTest

open NUnit.Framework
open FsUnit

[<TestFixture>]
type Test() =

    [<Test>] member x.
        ``One is One`` () =
            1 |> should equal 1

    [<Test>] member x.
        ``One is not Two`` () =
            1 |> should not' (equal 2)