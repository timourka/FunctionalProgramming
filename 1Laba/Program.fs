open System

printfn "Сумма всех чисел, равных сумме факториалов их цифр (диапазон 10..99999): %d"
    (
    let factorialSmall = [|0;1;1;2;6;24;120;720;5040;40320|] // 0..9
    let digitFactSum (n:int) =
        n.ToString() |> Seq.sumBy (fun c -> factorialSmall.[int c - int '0'])
    [10..99999] |> List.filter (fun n -> n = digitFactSum n) |> List.sum
    )
