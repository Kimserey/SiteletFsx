﻿namespace SiteletFsx

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
    let getUserName (id: string): Async<string> =
        async { 
            return "Test username"
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
            divAttr [attr.``class`` "box"] [text "Write something here"]

            Doc.Input [] input
            div [Doc.TextView input.View]
            
            Doc.TextNode json
            
            Doc.Button "Get user rpc" [] (fun () -> 
                async {
                    let! user = getUserName "id"
                    do Var.Set username user
                } |> Async.Start)
            Doc.TextView username.View
        ]

module Site =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client

    let site =
        Sitelet.Content "test" "test"  (fun _ -> 
            Content.Page(
                Title = "Hello", 
                Head = [ linkAttr [attr.rel "stylesheet"; attr.``type`` "text/css"; attr.href "style.css"] [] ],
                Body = [client <@ Client.main () @>]))

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