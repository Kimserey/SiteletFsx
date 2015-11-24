namespace SiteletFsx

#load "References.fsx"
#load "SiteletPage.fsx"

open WebSharper
open WebSharper.Sitelets
open System.IO
open Common

[<JavaScript>]
module Client =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client


    let main() = 
        let input = Var.Create ""
        let json = Json.Serialize "json test"

        div [
            h2 [text "Hello world"]
            div [text "Write something here"]
            Doc.Input [] input
            div [Doc.TextView input.View]
            Doc.TextNode json
        ]

module Site =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client

    let site =
        Sitelet.Content "test" "test"  (fun _ -> Content.Page(Title = "Hello", Body = [client <@ Client.main () @>]))

    let main =

        let metadata = Common.WsCompiler.compileToWs @"C:\Projects\SiteletFsx\SiteletFsx\bin\Debug"

        let sitelet = 
            Sitelet.Sum [
                site
                PageOneSite.site
            ]

        {  Route = "test"
           Sitelet = sitelet
           Metadata = metadata }
        
  