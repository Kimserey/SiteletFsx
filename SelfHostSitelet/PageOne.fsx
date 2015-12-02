namespace Fsx

#load "References.fsx"

open WebSharper
open WebSharper.Sitelets
open Common
open SelfHostSitelet

[<JavaScript>]
module PageOneClient =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client
    open WebSharper.JavaScript

    let main() = 
        div [
            h1 [text "This is page One"]
            Doc.Button "Shell rpc" [] (fun () -> 
                async {
                    let! msg = SelfHostSitelet.Server.getUserName "from rpc" 
                    do JS.Alert msg
                } |> Async.Start)
        ]

module PageOnePage =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client

    let page: string * Doc =
        "pageone", client <@ PageOneClient.main() @>