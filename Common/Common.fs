namespace Common

open WebSharper
open WebSharper.Sitelets
    
type CompiledSitelet = {
    Route: string
    Sitelet: Sitelet<string>
}
