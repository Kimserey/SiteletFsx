#if HOSTED
#else
#I "../packages/Owin/lib/net40"
#I "../packages/Microsoft.Owin/lib/net45"
#I "../packages/Microsoft.Owin.Hosting/lib/net45"
#I "../packages/Microsoft.Owin.StaticFiles/lib/net45"
#I "../packages/Microsoft.Owin.FileSystems/lib/net45"
#I "../packages/Microsoft.Owin.Host.HttpListener/lib/net45"
#I "../packages/Microsoft.Owin.Hosting/lib/net45"
#I "../packages/WebSharper/lib/net40"
#I "../packages/WebSharper.Compiler/lib/net40"
#I "../packages/WebSharper.Owin/lib/net45"
#I "../packages/WebSharper.UI.Next/lib/net40"

#r "Owin.dll"
#r "Mono.Cecil.dll"


#r "Microsoft.Owin.dll"
#r "Microsoft.Owin.Hosting.dll"
#r "Microsoft.Owin.StaticFiles.dll"
#r "Microsoft.Owin.FileSystems.dll"


#r "IntelliFactory.Core.dll"
#r "WebSharper.Compiler.dll"
#r "WebSharper.Collections.dll"
#r "WebSharper.Control.dll"
#r "WebSharper.Core.dll"
#r "WebSharper.Core.JavaScript.dll"
#r "WebSharper.JavaScript.dll"
#r "WebSharper.JQuery.dll"
#r "WebSharper.Main.dll"
#r "WebSharper.Sitelets.dll"
#r "WebSharper.Web.dll"
#r "WebSharper.Owin.dll"


#r "WebSharper.UI.Next.dll"
#r "WebSharper.UI.Next.Templating.dll"

#r "./bin/SelfHostSitelet.dll"

#endif