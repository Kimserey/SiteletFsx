open WebSharper
open WebSharper.Sitelets

open global.Owin
open Microsoft.Owin.Hosting
open Microsoft.Owin.StaticFiles
open Microsoft.Owin.FileSystems
open WebSharper.Owin
open SelfHostSitelet.Site

module OwinHost =

    [<EntryPoint>]
    let Main args =
        let rootDirectory, url =
            match args with
            | [| rootDirectory; url |] -> rootDirectory, url
            | [| url |] -> "..", url
            | [| |] -> "..", "http://localhost:9000/"
            | _ -> eprintfn "Usage: SelfHostSitelet ROOT_DIRECTORY URL"; exit 1
        use server = WebApp.Start(url, fun appB ->
            let (sitelet, meta, root) = sitelet
            appB.UseStaticFiles(StaticFileOptions(FileSystem = PhysicalFileSystem(root))) |> ignore
            appB.UseCustomSitelet(Options.Create(meta), sitelet) |> ignore
        )
        stdout.WriteLine("Serving {0}", url)
        stdin.ReadLine() |> ignore
        0
