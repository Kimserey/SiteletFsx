namespace SiteletFsx

#load "References.fsx"
#load "SiteletPage.fsx"

open WebSharper
open WebSharper.Sitelets
open System.IO
open Common
open WebSharper.Resources

module Resources =
    
    type BootstrapResource() =
        inherit BaseResource("//maxcdn.bootstrapcdn.com/bootstrap/3.3.5", "css/bootstrap.min.css")

module Server =
    
    [<Rpc>]
    let getUsers (id: string): Async<string> =
        async { 
            return id + " hello"
        }

[<JavaScript>]
module Client =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client
    open Server

    [<Require(typeof<Resources.BootstrapResource>)>]
    let main() = 
        let input = Var.Create ""
        let json = Json.Serialize "json test"
        let username = Var.Create ""

        div [
            h2Attr [attr.``class`` "well"] [text "Hello world"]
            div [text "Write something here"]

            Doc.Input [] input
            div [Doc.TextView input.View]
            
            Doc.TextNode json
            
            Doc.Button "Get user rpc" [] (fun () -> 
                async {
                    let! user = getUsers "x"
                    do Var.Set username user
                } |> Async.Start)
            Doc.TextView username.View
        ]

module Site =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client

    let site =
        Sitelet.Content "test" "test"  (fun _ -> Content.Page(Title = "Hello", Body = [client <@ Client.main () @>]))

    
    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin
    open Common
    open System.Threading.Tasks
    open System.Collections.Generic

    type SiteMiddleware(next: Owin.AppFunc, sitelet, meta, rootDir, ?debug) =
        let exec =
            let siteletMw =
                WebSharper.Owin.SiteletMiddleware(
                    next, 
                    Options.Create(meta)
                        .WithServerRootDirectory(rootDir)
                        .WithDebug(defaultArg debug false),
                    sitelet)
            let staticFilesMw =
                Microsoft.Owin.StaticFiles.StaticFileMiddleware(
                    AppFunc siteletMw.Invoke,
                    StaticFileOptions(
                        FileSystem = PhysicalFileSystem(rootDir)))
            staticFilesMw.Invoke

        member this.Invoke(env: IDictionary<string, obj>) = exec env

    let main =

        let metadata = Common.WsCompiler.compileToWs @"C:\Projects\SiteletFsx\SiteletFsx\bin\Debug"

        let sitelet = site
//            Sitelet.Sum [
//                site
//                PageOneSite.site
//            ]

//        {  Route = "test"
//           Sitelet = sitelet
//           Metadata = metadata }


    
        use server = WebApp.Start("http://localhost:9091", fun appB ->

            appB.Use(Owin.MidFunc(fun next -> 
                let mw = SiteMiddleware(next, sitelet, metadata, @"C:\Projects\SiteletFsx\SiteletFsx\bin\Debug")
                Owin.AppFunc mw.Invoke)) |> ignore
                        
            appB.Use(fun ctx next ->
                    Task.Factory.StartNew(fun () ->
                        let s : Stream =
                            ctx .Set("owin.ResponseStatusCode", 404)
                                .Set("owin.ResponseReasonPhrase", "Not Found")
                                .Get("owin.ResponseBody")
                        use w = new StreamWriter(s)
                        w.Write("Page not found"))) |> ignore
        )
                
        stdout.WriteLine("Serving {0}", "http://localhost:9091")
        stdin.ReadLine() |> ignore