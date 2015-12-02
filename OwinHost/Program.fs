open WebSharper
open WebSharper.Sitelets

open global.Owin
open Microsoft.Owin.Hosting
open Microsoft.Owin.StaticFiles
open Microsoft.Owin.FileSystems
open WebSharper.Owin

open SelfHostSitelet

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
            Site.main appB
        )
        stdout.WriteLine("Serving {0}", url)
        stdin.ReadLine() |> ignore
        0
