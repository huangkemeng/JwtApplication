module Tests

open System
open Xunit
open System.Net.Http


[<Fact>]
let SuccessTest () =
    let client = new HttpClient()
    client.PostAsync("http://localhost:54385/api/token/Test", new HttpContent()) |> ignore
    0
