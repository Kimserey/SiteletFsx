namespace Fsx

#load "References.fsx"

open WebSharper
open WebSharper.Sitelets
open System.IO
open SelfHostSitelet
open WebSharper.UI.Next.Html
open WebSharper.Resources

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

module Site =
    open WebSharper.UI.Next.Server

    let pages = 
            [ Route "", client <@ ClientPage.main() @>
              Route "inspections", client <@ ClientPage.inspections() @> ]
    
    let features root = 
        CompiledWebParts.Compile(root, pages)