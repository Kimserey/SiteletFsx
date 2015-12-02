namespace SiteletFsx

#load "References.fsx"
#load "PageOne.fsx"
#load "Hello.fsx"

open WebSharper
open WebSharper.Sitelets
open System.IO
open Common
open WebSharper.Resources

module Site =

    let metadata = Common.WsCompiler.compileToWs __SOURCE_DIRECTORY__
        
    let pages =
        [ PageOnePage.page
          HelloPage.page ]

    let features = {
        Pages = pages
        Metadata = metadata
    }