namespace SiteletFsx

open System
open System.IO
open System.Text
open Microsoft.FSharp.Compiler.Interactive.Shell
open WebSharper
open WebSharper.Sitelets

module SelfHostedServer =

    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin
    open Common

    [<EntryPoint>]
    let Main = function
        | [| rootDirectory; url |] ->

            let value = FsiExec.evaluateFsx<CompiledSitelet> "Sitelet.fsx" "SiteletFsx.Site.main"


            match value with
            | FsiExec.Success sitelet -> 
                use server = WebApp.Start(url, fun appB ->
                    let binDir = @"C:\Projects\SiteletFsx\SiteletFsx\bin\Debug"
                    appB.UseStaticFiles(StaticFileOptions(FileSystem = PhysicalFileSystem(binDir))) |> ignore
                    let options = WebSharper.Owin.Options.Create(sitelet.Metadata).WithServerRootDirectory(binDir).WithDebug(true)
                    appB.UseCustomSitelet(options, sitelet.Sitelet) |> ignore
                )
                
                stdout.WriteLine("Serving {0}", url)
                stdin.ReadLine() |> ignore
                0
            | _ -> eprintfn "Failed to initialise sitelet"
                   1
        | _ ->
            eprintfn "Usage: SiteletFsx ROOT_DIRECTORY URL"
            1
