open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Concurrent
open System.Threading.Tasks

/// Функция для поиска самого частого слова в файле
let mostFrequentWord (filePath: string) =
    let text = File.ReadAllText(filePath).ToLower()
    let words =
        Regex.Matches(text, @"\w+")
        |> Seq.cast<Match>
        |> Seq.map (fun m -> m.Value)

    let grouped =
        words
        |> Seq.countBy id
        |> Seq.sortByDescending snd
        |> Seq.tryHead

    match grouped with
    | Some (word, count) -> filePath, word, count
    | None -> filePath, "[нет слов]", 0

/// Главная функция
[<EntryPoint>]
let main argv =
    // Входные файлы (можно указать свою папку)
    let inputDir = "./texts"
    let outputFile = "result.txt"

    let files = Directory.GetFiles(inputDir, "*.txt")

    // Параллельная обработка
    let results = 
        files
        |> Array.Parallel.map mostFrequentWord

    // Формируем выходной файл
    use sw = new StreamWriter(outputFile, false)
    for (file, word, count) in results do
        sw.WriteLine($"Файл: {Path.GetFileName(file)} | Слово: '{word}' | Встречается: {count} раз")

    printfn $"Результаты сохранены в {outputFile}"
    0
