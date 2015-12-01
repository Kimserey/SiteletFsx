namespace Common

open WebSharper
open WebSharper.UI.Next
open WebSharper.Sitelets
    
type CompiledSitelet = {
    Route: string
    Sitelet: Sitelet<string>
    Metadata: Core.Metadata.Info
}

//type SiteletPages = {
//    Pages: List<Route * PageContent>
//    Metadata: Core.Metadata.Info
//}
//and Route = string
//and PageContent = Async<Content<string>>

type Features = {
    Pages: List<string * Doc>
    Metadata: Core.Metadata.Info
}
