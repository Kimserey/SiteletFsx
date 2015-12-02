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

    let metadata = Common.WsCompiler.compileToWs @"C:\Projects\SiteletFsx\SelfHostSitelet"
        
    let pages =
        [ PageOnePage.page
          HelloPage.page ]

    let features = {
        Pages = pages
        Metadata = metadata
    }