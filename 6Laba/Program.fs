open System
open System.Drawing
open System.Drawing.Imaging

type Rule = char * string
type Grammar = Rule list

let findSubst (c: char) (gr: Grammar) =
    match List.tryFind (fun (x, _) -> x = c) gr with
    | Some (_, s) -> s
    | None -> string c

let apply (gr: Grammar) (str: string) =
    str |> Seq.map (fun c -> findSubst c gr) |> String.concat ""

let rec nApply n gr str =
    if n = 0 then str
    else nApply (n-1) gr (apply gr str)

let drawLSystem (commands: string) (step: float) (angle: float) (fileName: string) =
    let bmp = new Bitmap(1000, 1000)
    let g = Graphics.FromImage(bmp)
    g.Clear(Color.White)
    use pen = new Pen(Color.Black, 1.0f)

    let mutable x, y = 500.0, 500.0
    let mutable dir = -90.0 // вверх
    let stack = new System.Collections.Generic.Stack<(float*float*float)>()

    for c in commands do
        match c with
        | 'F' ->
            let rad = Math.PI * dir / 180.0
            let x2 = x + step * cos rad
            let y2 = y + step * sin rad
            g.DrawLine(pen, float32 x, float32 y, float32 x2, float32 y2)
            x <- x2; y <- y2
        | '+' -> dir <- dir + angle
        | '-' -> dir <- dir - angle
        | '[' -> stack.Push(x, y, dir)
        | ']' ->
            let (sx, sy, sd) = stack.Pop()
            x <- sx; y <- sy; dir <- sd
        | _ -> ()

    bmp.Save(fileName, ImageFormat.Png)
    bmp.Dispose()
    g.Dispose()

[<EntryPoint>]
let main argv =
    // Аксиома
    let axiom = "[-FF+FF+FF-]"
    // Правила
    let grammar : Grammar = [ ('F', "F-F-F-F-F") ]

    let iterations = 5
    let step = 100.0
    let angle = 90.0

    for i in 0 .. iterations do
        let res = nApply i grammar axiom
        let fileName = $"lsystem_iter{i}.png"
        drawLSystem res step angle fileName
        printfn $"Итерация {i}: результат сохранён в {fileName}"

    0
