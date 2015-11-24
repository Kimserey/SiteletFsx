namespace SiteletFsx

#load "References.fsx"

open WebSharper
open WebSharper.Sitelets

[<JavaScript>]
module PageOneClient =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client

    let main() = 
        div [
            h1 [text "This is page One"]
        ]

module PageOneSite =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client

    let site =
        Sitelet.Content "one" "one" (fun ctx -> Content.Page(Title= "Page one", Body = [client <@ PageOneClient.main() @>]))
  