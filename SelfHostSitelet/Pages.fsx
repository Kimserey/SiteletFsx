namespace Fsx

#load "References.fsx"

open WebSharper
open WebSharper.Sitelets
open System.IO
open SelfHostSitelet
open WebSharper.Resources
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open System.Reflection

module ServerPage =
    [<Rpc>]
    let test () =
        async {
            return "solas"
        }

[<JavaScript>]
module ClientPage =
    open WebSharper.JavaScript
    open WebSharper.UI.Next.Client
    
    let showAlert() = 
        async {
            let! msg = ServerPage.test()
            do JS.Alert msg
        } 

    let main() =
        div [ div [text "solas"]
              div [ Doc.Button "Test" [] (fun () -> showAlert() |> Async.Start) ] ]

    let inspections() =
        div [text "Inspections"]
    
module ScriptRoot =
    open WebSharper.UI.Next.Server
    
    let pages = 
        [ Route "", client <@ ClientPage.main() @>
          Route "inspections", client <@ ClientPage.inspections() @> ]

    let compiledWebParts httpRoot = 
        let metadata = WsCompiler.compileAndUnpack httpRoot
        { WebParts = pages; Metadata = metadata }