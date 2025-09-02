open System
open System.IO

let countVowels (path:string) =
    let text = File.ReadAllText(path).ToUpper()

    // Множество гласных букв русского алфавита
    let vowels = set ['A'; 'E'; 'Y'; 'U'; 'I'; 'O'; ]

    // Считаем количество каждой гласной
    let vowelCounts =
        text
        |> Seq.filter (fun c -> vowels.Contains c)
        |> Seq.countBy id
        |> Map.ofSeq

    vowelCounts

// Пример вызова:
let path = @"C:\Users\bingo\source\repos\FunctionalProgramming\2Laba\History for ready reference, Volume 1, A-Elba by J. N. Larned.txt"
let res = countVowels path

// Вывод результатов
res |> Map.iter (fun vowel count -> printfn "%c: %d" vowel count)

