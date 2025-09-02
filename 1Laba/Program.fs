open System
open System.Numerics

// --- Вспомогательные функции ---
let digitsOfString (s:string) : int[] =
    s |> Seq.map (fun c -> int c - int '0') |> Seq.toArray

let digitsOfBigInt (n:bigint) : int[] =
    n.ToString() |> digitsOfString

let sumDigitsBigInt (n:bigint) : int =
    digitsOfBigInt n |> Array.sum

let productDigitsBigInt (n:bigint) : bigint =
    let ds = digitsOfBigInt n |> Array.map bigint
    if Array.isEmpty ds then 0I else Array.reduce (*) ds

let powBigInt (basev:bigint) (exp:int) : bigint =
    // простая итеративная реализация (подходит для учебных задач)
    let rec loop acc e =
        if e = 0 then acc else loop (acc * basev) (e - 1)
    loop 1I exp

let factorialInt (n:int) : bigint =
    if n <= 1 then 1I
    else
        let rec loop i acc =
            if i > n then acc
            else loop (i+1) (acc * bigint i)
        loop 1 1I

// --- Задания ---
// 1. Сумма чисел, кратных 3 и 5 (от 1 до 100)
let task1 : int = [1..100] |> List.filter (fun x -> x % 3 = 0 && x % 5 = 0) |> List.sum

// 2. Сумма чисел, кратных 4
let task2 : int = [1..100] |> List.filter (fun x -> x % 4 = 0) |> List.sum

// 3. Некратные 3 и кратные 5
let task3 : int = [1..100] |> List.filter (fun x -> x % 3 <> 0 && x % 5 = 0) |> List.sum

// 4. Числа с цифрой 1
let task4 : int = [1..100] |> List.filter (fun x -> x.ToString().Contains("1")) |> List.sum

// 5. Числа с цифрой 9
let task5 : int = [1..100] |> List.filter (fun x -> x.ToString().Contains("9")) |> List.sum

// 6. Разница между квадратом суммы и суммой квадратов
let task6 : int =
    let sumSquares = [1..100] |> List.sumBy (fun x -> x * x)
    let squareSum = ([1..100] |> List.sum) |> fun s -> s * s
    squareSum - sumSquares

// 7. Для заданного списка слов найти слова, содержащие не менее одной буквы 'Т'
let task7 (words:string list) : string list =
    words |> List.filter (fun (w:string) -> w.ToUpper().Contains("Т"))

// 8. Для заданного списка слов найти слова, не содержащие буквы 'Е'
let task8 (words:string list) : string list =
    words |> List.filter (fun (w:string) -> not (w.ToUpper().Contains("Е")))

// 9. Для заданного списка слов найти слова, содержащие не менее одной 'Т' и не более двух 'О'
let task9 (words:string list) : string list =
    words |> List.filter (fun (w:string) ->
        let upper = w.ToUpper()
        upper.Contains("Т") &&
        (upper |> Seq.filter (fun c -> c = 'О') |> Seq.length) <= 2
    )

// 10. Для заданного слова найти слова, которые могут быть построены из его букв
let task10 (baseWord:string) (words:string list) : string list =
    let charCount (s:string) : Map<char,int> =
        s.ToUpper() |> Seq.countBy id |> Map.ofSeq
    let baseMap = charCount baseWord
    let canBuild (w:string) =
        let m = charCount w
        m |> Map.forall (fun ch cnt ->
            baseMap.ContainsKey(ch) && baseMap.[ch] >= cnt
        )
    words |> List.filter canBuild

// 11. Для заданного 20-значного числа найти сумму его цифр
let task11 (num:string) : int =
    // ожидаем, что num имеет 20 символов и все цифры
    num |> Seq.sumBy (fun c -> int c - int '0')

// 12. Для заданной степени двойки найти сумму цифр соответствующего числа
let task12 (n:int) : int =
    let v = powBigInt 2I n
    sumDigitsBigInt v

// 13. Для заданной степени тройки найти сумму цифр соответствующего числа
let task13 (n:int) : int =
    let v = powBigInt 3I n
    sumDigitsBigInt v

// 14. Для заданной степени тройки найти произведение цифр соответствующего числа
let task14 (n:int) : bigint =
    let v = powBigInt 3I n
    productDigitsBigInt v

// 15. Количество лет XX века (1901..2000), первый день которых — понедельник
let task15 : int =
    [1901..2000] |> List.filter (fun y -> DateTime(y,1,1).DayOfWeek = DayOfWeek.Monday) |> List.length

// 16. Для заданного числа определить сумму цифр его факториала
let task16 (n:int) : int =
    factorialInt n |> sumDigitsBigInt

// 17. Найти номер числа Фибоначчи, содержащее заданное число цифр в своей записи
let task17 (digits:int) : int =
    if digits <= 1 then 1
    else
        let rec loop (a:bigint) (b:bigint) (idx:int) =
            if b.ToString().Length >= digits then idx
            else loop b (a + b) (idx + 1)
        loop 1I 1I 2

// 18. Сумма всех чисел, равных сумме факториалов их цифр (проход по разумному диапазону)
let task18 : int =
    let factorialSmall = [|0;1;1;2;6;24;120;720;5040;40320|] // 0..9
    let digitFactSum (n:int) =
        n.ToString() |> Seq.sumBy (fun c -> factorialSmall.[int c - int '0'])
    [10..99999] |> List.filter (fun n -> n = digitFactSum n) |> List.sum

// 19. Количество лет XX века, первый день которых — среда
let task19 : int =
    [1901..2000] |> List.filter (fun y -> DateTime(y,1,1).DayOfWeek = DayOfWeek.Wednesday) |> List.length

// 20. Количество лет XX века, первый день которых — воскресенье
let task20 : int =
    [1901..2000] |> List.filter (fun y -> DateTime(y,1,1).DayOfWeek = DayOfWeek.Sunday) |> List.length

// --- Примерные входные данные для задач, где требуется ввод ---
let sampleWords : string list = ["тест"; "программа"; "окно"; "компьютер"; "стол"; "торт"; "оооо"; "мороз"]
let baseWordFor10 = "тест"
let sample20Digit = "12345678901234567890"  // 20 цифр

// Примерные параметры для степеней / факториала / фибоначчи
let pow2_n = 15        // для task12 (2^15)
let pow3_n = 10        // для task13/14 (3^10)
let fact_n = 10        // для task16 (10!)
let fibDigits = 3      // для task17 (найти номер первого Фибоначчи с 3 цифрами)

// --- Main: выводим результаты всех 20 заданий ---
[<EntryPoint>]
let main argv =
    printfn "1) Сумма чисел от 1 до 100, кратных 3 и 5: %A" task1
    printfn "2) Сумма чисел от 1 до 100, кратных 4: %A" task2
    printfn "3) Сумма чисел от 1 до 100, некратных 3 и кратных 5: %A" task3
    printfn "4) Сумма чисел 1..100, в записи которых есть 1: %A" task4
    printfn "5) Сумма чисел 1..100, в записи которых есть 9: %A" task5
    printfn "6) Разница между квадратом суммы и суммой квадратов (1..100): %A" task6

    printfn "7) Слова с буквой 'Т' (из примера): %A" (task7 sampleWords)
    printfn "8) Слова без буквы 'Е' (из примера): %A" (task8 sampleWords)
    printfn "9) Слова с >=1 'Т' и <=2 'О' (из примера): %A" (task9 sampleWords)
    printfn "10) Слова, которые можно составить из букв '%s' (из примера): %A" baseWordFor10 (task10 baseWordFor10 sampleWords)

    printfn "11) Сумма цифр 20-значного числа %s: %d" sample20Digit (task11 sample20Digit)
    printfn "12) Сумма цифр 2^%d = %A -> %d" pow2_n (powBigInt 2I pow2_n) (task12 pow2_n)
    printfn "13) Сумма цифр 3^%d = %A -> %d" pow3_n (powBigInt 3I pow3_n) (task13 pow3_n)
    printfn "14) Произведение цифр 3^%d = %A -> %A" pow3_n (powBigInt 3I pow3_n) (task14 pow3_n)

    printfn "15) Количество лет XX века (1901..2000), начинающихся с понедельника: %d" task15
    printfn "16) Сумма цифр %d! = %d" fact_n (task16 fact_n)
    printfn "17) Номер числа Фибоначчи с %d цифрами: %d" fibDigits (task17 fibDigits)
    printfn "18) Сумма всех чисел, равных сумме факториалов их цифр (диапазон 10..99999): %d" task18
    printfn "19) Количество лет XX века, начинающихся со среды: %d" task19
    printfn "20) Количество лет XX века, начинающихся в воскресенье: %d" task20
