module SignTest

open NUnit.Framework
open FsUnit
open SharpSign

[<TestFixture>]
type Test() =

    [<Test>] member x.
        ``should be line length + 4`` () =
            signWidth("abx") |> should equal 7

    [<Test>] member x.
        ``should wrap line`` () =
            wrapLine "abc" 8 |> should equal "| abc      |"