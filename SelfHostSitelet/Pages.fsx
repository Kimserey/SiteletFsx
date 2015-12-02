namespace Fsx

#load "References.fsx"
#load "PageOne.fsx"
#load "Hello.fsx"

open WebSharper
open WebSharper.Sitelets
open System.IO
open SelfHostSitelet
open WebSharper.Resources

module Site =

    let pages =
        [ PageOnePage.page
          HelloPage.page ]

    let features root = 
        let metadata = WsCompiler.compileToWs root
        { Pages = pages
          Metadata = metadata }