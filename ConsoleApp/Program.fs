namespace ConsoleApp

open System
// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
[<ReflectedDefinition>]
module CApp =
    type Test = {
        N: string
        L: string
    }

[<ReflectedDefinition>]
module Client =
    open CApp
    let main() = 
        let t = { N = "ne"; L = "l" }
        t

module Main =

    [<EntryPoint>]
    let main argv = 
        Common.WsCompiler.produceScripts @"C:\Projects\SiteletFsx\ConsoleApp"
        printfn "%A" argv
        0 // return an integer exit code
