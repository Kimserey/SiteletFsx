namespace SiteletFsx

#load "References.fsx"

open WebSharper
open WebSharper.Sitelets
open Common

[<JavaScript>]
module PageOneClient =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client

    let main() = 
        div [
            h1 [text "This is page One"]
        ]

module PageOnePage =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client

    let page: Route * PageContent =
        "pageone", Content.Page(Title= "Page one", Body = [client <@ PageOneClient.main() @>])