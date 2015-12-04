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

module Server =
    [<Rpc>]
    let test () =
        async {
            return "solas"
        }


[<JavaScript>]
module Client =
    open WebSharper.JavaScript
    open WebSharper.UI.Next.Client
    
    let showAlert() = 
        async {
            let! msg = Server.test()
            do JS.Alert msg
        } 

    let main() =
        div [ div [text "solas"]
              div [ Doc.Button "Test" [] (fun () -> showAlert() |> Async.Start) ] ]

    let inspections() =
        div [text "Inspections"]
    
module ScriptRoot =
    open WebSharper.UI.Next.Server

    /// Compile the assembly to WebSharper and unpack its scripts, contents
    /// and return the metadata
    let compiledWebParts httpRoot = 
    
//        CompiledWebParts.Compile(
//            httpRoot, 
//            [
//                Route "", client <@ Client.main() @> 
//                Route "inspections", client <@ Client.inspections() @> 
//            ])

        let metadata = WsCompiler.compileAndUnpack httpRoot
        { WebParts = [ Route "", client <@ Client.main() @>; Route "inspections", client <@ Client.inspections() @> ]; Metadata = metadata }