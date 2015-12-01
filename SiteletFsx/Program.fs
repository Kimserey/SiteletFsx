﻿namespace SiteletFsx

open System
open System.IO
open System.Text
open System.Threading.Tasks
open System.Collections.Generic
open Microsoft.FSharp.Compiler.Interactive.Shell
open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Html

module SelfHostedServer =

    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin
    open Common

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


    [<EntryPoint>]
    let Main = function
        | [| rootDirectory; url |] ->

            let compiledPages = FsiExec.evaluateFsx<Features> "Pages.fsx" "SiteletFsx.Site.features"

            match compiledPages with
            | FsiExec.Success pages -> 
                use server = WebApp.Start(url, fun appB ->

                    let sitelet = 
                        pages.Pages
                        |> List.map (fun (route, page) -> Sitelet.Content route route  (fun _ -> Content.Page(Body = [ div [text "Some nav bar here"] :> Doc; page ])))
                        |> Sitelet.Sum
                        
                    appB.Use(Owin.MidFunc(fun next -> 
                        let mw = SiteMiddleware(next, sitelet, pages.Metadata, @"C:\Projects\SiteletFsx\SiteletFsx\bin\Debug")
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
                
                stdout.WriteLine("Serving {0}", url)
                stdin.ReadLine() |> ignore
                0
            | _ -> eprintfn "Failed to initialise sitelet"
                   1
        | _ ->
            eprintfn "Usage: SiteletFsx ROOT_DIRECTORY URL"
            1
