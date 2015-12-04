namespace Fsx

#load "References.fsx"

open WebSharper
open WebSharper.Sitelets
open SelfHostSitelet
open WebSharper.Resources

[<JavaScript>]
module PageOneClient =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client
    open WebSharper.JavaScript
    
    let main() = 
        h1 [text "This is page One"]

module PageOnePage =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.UI.Next.Client

    let page =
        Route "pageone", client <@ PageOneClient.main() @>