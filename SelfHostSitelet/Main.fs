namespace SelfHostSitelet

open WebSharper.Html.Server
open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open Common

module Server =
    [<Rpc>]
    let getUserName (id: string): Async<string> =
        async { 
            return "Hello" + id
        }

[<JavaScript>]
module Client =
    open WebSharper.UI.Next.Client
    open WebSharper.JavaScript
    
    let main1() =
        divAttr [attr.style "background-color: blue;"] [
            Doc.Button "Click" [] (fun () -> 
                async { 
                    let! msg = Server.getUserName "kim" 
                    do JS.Alert msg
                } |> Async.Start)
        ]

module Site =
    open WebSharper.UI.Next.Server
    open System.Collections.Generic

    type MainTemplate = Templating.Template<"Main.html">

    let sitelet httproot =
        let compiledPages = FsiExec.evaluateFsx<Features> "Pages.fsx" (sprintf "Fsx.Site.features \"%s\"" httproot)
        match compiledPages with
        | FsiExec.Success compiled -> 
            let sitelet =
                compiled.Pages
                |> List.map (fun (route, page) -> route, Content.Page(MainTemplate.Doc(title = route, body = [ client <@ Client.main1() @>; page ])))
                |> List.map (fun (route, page) -> Sitelet.Content route route (fun _ -> page))
                |> Sitelet.Sum
                    
            sitelet, compiled.Metadata

        | _ -> failwith "couldnt compile fsx"