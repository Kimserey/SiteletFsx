namespace SiteletFsx

#load "References.fsx"

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
            h1 [text "hello from my JS in my script sitelet"]
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
        Sitelet.Content "test" "test"  (fun _ ->
                Content.Page(
                    Body = [
                        h1 [text "hello from my script sitelet"] :> Doc
                        client <@ Client.main () @>
                    ],
                    Title = "Hello"
            ))

    let main =

        Common.WsCompiler.produceScripts @"C:\Projects\SiteletFsx\SiteletFsx\bin\Debug"

        {  Route = "test"
           Sitelet = site }
        
  