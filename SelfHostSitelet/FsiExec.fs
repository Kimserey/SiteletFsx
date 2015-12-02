namespace SelfHostSitelet

open System
open System.IO
open System.Text
open Microsoft.FSharp.Compiler.Interactive.Shell

module FsiExec =
    module Paths =
            let getBaseDirDll name =
                Path.Combine (AppDomain.CurrentDomain.BaseDirectory, name + ".dll")

    [<NoComparison>]
    type EvaluateFsxResult<'t> =
    | Success of Object : 't
    | Failure of Message : string
    | Fatal of Exception : exn * FsiOutput : string * FsiError : string

    type FsiEvaluationSession with
        member x.ReferenceDll filename = x.EvalInteraction("#r @\"" + filename + "\"")
        member x.ReferenceDlls filenames = filenames |> Seq.iter x.ReferenceDll

    let evaluateFsx<'expected> filename evaluate =
        let sbOut = new StringBuilder()
        let sbErr = new StringBuilder()
        use inStream = new StringReader("")
        use outStream = new StringWriter(sbOut)
        use errStream = new StringWriter(sbErr)
        
        try
            let fsiArgs = [| "--noninteractive"; "--nologo"; "--gui-"; "--define:HOSTED" |]
            let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
            use fsiSession = FsiEvaluationSession.Create(fsiConfig, fsiArgs, inStream, outStream, errStream)

            // Reference essential assemblies.
            fsiSession.ReferenceDlls [
                Paths.getBaseDirDll "WebSharper.Core"
                Paths.getBaseDirDll "WebSharper.UI.Next"
                Paths.getBaseDirDll "WebSharper.Main"
                Paths.getBaseDirDll "WebSharper.Web"
                Paths.getBaseDirDll "WebSharper.Sitelets"
                Paths.getBaseDirDll "Common"
            ]

            fsiSession.EvalScript(filename)

            // Now eval the expression to, presumably, retrieve some result object that the main script computed.
            match fsiSession.EvalExpression(evaluate) with
            | Some v -> Success (v.ReflectionValue :?> 'expected)
            | None -> Failure ("Expression could not be evaluated with this FSX script.")
        with
        | x -> Fatal (x, string sbOut, string sbErr)
