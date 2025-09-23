let A = [|1; 9; 3; 4; 6|]

let sum =
    A
    |> Array.mapi (fun i x -> if i + 1 = x then x else 0)
    |> Array.sum

printfn "Сумма элементов, равных своему индексу: %d" sum

let mutable sum2 = 0

// Проходим по массиву циклом
for i = 0 to A.Length - 1 do
    if A.[i] = i+1 then
        sum2 <- sum2 + A.[i]

// Выводим результат
printfn "Сумма элементов, равных своему индексу императивно: %d" sum2