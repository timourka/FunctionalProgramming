open System
open System.IO
open System.Text.RegularExpressions

let mostCommonWord (path:string) =
    File.ReadAllText(path).ToLower()
    |> fun text -> Regex.Split(text, @"\W+")  
    |> Seq.filter (fun w -> w <> "")          
    |> Seq.countBy id                         
    |> Seq.maxBy snd                          
    |> fst 

let path = @".\History for ready reference, Volume 1, A-Elba by J. N. Larned.txt"
let result = mostCommonWord path
printfn "Самое часто встречающееся слово: %s" result

