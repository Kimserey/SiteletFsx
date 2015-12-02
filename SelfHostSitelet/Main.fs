namespace SelfHostSitelet

open WebSharper.Html.Server
open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open Common

open global.Owin
open Microsoft.Owin.Hosting
open Microsoft.Owin.StaticFiles
open Microsoft.Owin.FileSystems
open WebSharper.Owin


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

    type MainTemplate = Templating.Template<"Main.html">

    let main (appB: Owin.IAppBuilder) =
        
        let compiledPages = FsiExec.evaluateFsx<Features> "Pages.fsx" "SiteletFsx.Site.features"
        match compiledPages with
        | FsiExec.Success pages -> 
            let sitelet =
                pages.Pages
                |> List.map (fun (route, page) -> 
                    route, 
                    Content.Page(
                        MainTemplate.Doc(title = route, 
                                         body = [ 
                                            client <@ Client.main1() @>
                                            page ]))
                    )
                |> List.map (fun (route, page) -> Sitelet.Content route route (fun _ -> page))
                |> Sitelet.Sum

            let root = @"C:\Projects\SiteletFsx\SelfHostSitelet"
        
            appB.UseStaticFiles(StaticFileOptions(FileSystem = PhysicalFileSystem(root)))
                .UseCustomSitelet(Options.Create(pages.Metadata).WithDebug(true), sitelet) |> ignore

        | _ -> failwith "couldnt compile fsx"

