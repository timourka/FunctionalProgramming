open System.IO
open System.Text.RegularExpressions

let mostCommonWord (path:string) =
    let word = File.ReadAllText(path).ToLower()
            |> fun text -> Regex.Split(text, @"\W+")  
            |> Seq.filter (fun w -> w <> "")          
            |> Seq.countBy id                         
            |> Seq.maxBy snd                          
            |> fst 
    path, word

let inputDir = "./texts"
let outputFile = "result.txt"

let files = Directory.GetFiles(inputDir, "*.txt")

let results = 
    files
    |> Array.Parallel.map mostCommonWord

let sw = new StreamWriter(outputFile, false)
for (file, word) in results do
    sw.WriteLine($"Файл: {Path.GetFileName(file)} | Слово: '{word}'")
sw.Close()

printfn $"Результаты сохранены в {outputFile}"
