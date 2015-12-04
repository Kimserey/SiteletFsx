namespace SelfHostSitelet

open WebSharper
open WebSharper.UI.Next
open WebSharper.Sitelets
open System.Reflection

type CompiledSitelet = {
    Route: string
    Sitelet: Sitelet<string>
    Metadata: Core.Metadata.Info
}

type Features = {
    Pages: List<string * Doc>
    Metadata: Core.Metadata.Info
}

type CompiledWebParts = {
    WebParts: List<WebPart>
    Metadata: Core.Metadata.Info
} with
    static member Compile(httpRoot, webParts) = 
        let asm = Assembly.GetCallingAssembly()
        let metadata = WsCompiler.compileAndUnpack httpRoot
        { WebParts = webParts; Metadata = metadata }
and WebPart = Route * Doc
and Route = Route of string