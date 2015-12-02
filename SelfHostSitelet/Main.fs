namespace SelfHostSitelet

open WebSharper.Html.Server
open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open Common

module Site =
    
    let main =
        
        let compiledPages = FsiExec.evaluateFsx<Features> "Pages.fsx" "SiteletFsx.Site.features"
        
        match compiledPages with
        | FsiExec.Success pages -> printfn "%A" pages.Metadata
        | _ -> ()

        Application.SinglePage (fun ctx -> Content.Page(Title = "Hello", Body = [h1 [text "HEHE" ]]))


module SelfHostedServer =

    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin

    [<EntryPoint>]
    let Main args =
        let rootDirectory, url =
            match args with
            | [| rootDirectory; url |] -> rootDirectory, url
            | [| url |] -> "..", url
            | [| |] -> "..", "http://localhost:9000/"
            | _ -> eprintfn "Usage: SelfHostSitelet ROOT_DIRECTORY URL"; exit 1
        use server = WebApp.Start(url, fun appB ->
            appB.UseStaticFiles(
                    StaticFileOptions(
                        FileSystem = PhysicalFileSystem(rootDirectory)))
                .UseSitelet(rootDirectory, Site.main)
            |> ignore)
        stdout.WriteLine("Serving {0}", url)
        stdin.ReadLine() |> ignore
        0
