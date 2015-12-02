namespace Common

open WebSharper
open WebSharper.UI.Next
open WebSharper.Sitelets
    
type CompiledSitelet = {
    Route: string
    Sitelet: Sitelet<string>
    Metadata: Core.Metadata.Info
}

type Features = {
    Pages: List<string * Doc>
    Metadata: Core.Metadata.Info
}
