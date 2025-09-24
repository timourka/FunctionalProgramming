open System

// ---------------- Класс Жителя ----------------
type Citizen(name: string, age: int, state: State) as this =
    let mutable hasPassport = false
    let mutable taxDebt = 1000
    let mutable isRegisteredToDoctor = false

    do state.AddCitizen(this)

    member this.Name = name
    member this.Age = age
    member this.State = state
    member this.HasPassport
        with get() = hasPassport
        and set(value) = hasPassport <- value

    member this.TaxDebt
        with get() = taxDebt
        and set(value) = taxDebt <- value

    member this.IsRegisteredToDoctor
        with get() = isRegisteredToDoctor
        and set(value) = isRegisteredToDoctor <- value

    override this.ToString() =
        sprintf "Житель %s, возраст %d, Паспорт: %b, Долг: %d %s, Запись к врачу: %b"
            this.Name this.Age this.HasPassport this.TaxDebt this.State.Valuta this.IsRegisteredToDoctor


// ---------------- Класс Государство ----------------
and State(stateName: string, Valuta: string) =
    let mutable citizens: Citizen list = []

    member this.Name = stateName
    member this.Valuta = Valuta
    member this.Citizens with get() = citizens

    member this.AddCitizen(citizen: Citizen) =
        citizens <- citizen :: citizens

    member this.FindCitizen(name: string) =
        citizens |> List.tryFind (fun c -> c.Name = name)

    override this.ToString() =
        sprintf "Государство %s, жителей: %d" this.Name citizens.Length

// ---------------- Интерфейс госуслуги ----------------
type IGovService =
    abstract member Name: string
    abstract member Process: string -> unit

// ---------------- Абстрактный класс госуслуги ----------------
[<AbstractClass>]
type GovService(state: State, serviceName: string) =
    abstract member PerformService: Citizen -> unit

    interface IGovService with
        member this.Name = serviceName
        member this.Process(citizenName: string) =
            match state.FindCitizen(citizenName) with
            | Some citizen -> this.PerformService(citizen)
            | None -> printfn "Житель %s не найден в государстве %s" citizenName state.Name

// ---------------- Конкретные госуслуги ----------------
type PassportService(state: State) =
    inherit GovService(state, "Оформление паспорта")
    override this.PerformService(citizen: Citizen) =
        if not citizen.HasPassport then
            citizen.HasPassport <- true
            printfn "📘 Паспорт успешно оформлен для гражданина %s" citizen.Name
        else
            printfn "❌ У гражданина %s уже есть паспорт" citizen.Name

type TaxService(state: State) =
    inherit GovService(state, "Уплата налогов")
    override this.PerformService(citizen: Citizen) =
        if citizen.TaxDebt > 0 then
            printfn "💰 Гражданин %s оплатил налог в размере %d" citizen.Name citizen.TaxDebt
            citizen.TaxDebt <- 0
        else
            printfn "✅ У гражданина %s нет налоговой задолженности" citizen.Name

type MedicalService(state: State) =
    inherit GovService(state, "Запись к в")
    override this.PerformService(citizen: Citizen) =
        if not citizen.IsRegisteredToDoctor then
            citizen.IsRegisteredToDoctor <- true
            printfn "🏥 Гражданин %s записан на приём к врачу" citizen.Name
        else
            printfn "📅 Гражданин %s уже был записан к врачу" citizen.Name



//-------------------------main------------------------------
let state = State("ФШарпания", "Шарпель")
let citizen = Citizen("Иван Иваныч", 45, state)

printfn "%s" (state.ToString())
printfn "%s" (citizen.ToString())
printfn "--------------------------------"

// Создаём список сервисов
let services: IGovService list =
    [ PassportService(state) :> IGovService;
    TaxService(state) :> IGovService;
    MedicalService(state) :> IGovService ]

// Житель пользуется всеми услугами
for s in services do
    printfn "Используем сервис: %s" s.Name
    s.Process("Иван Иваныч")
    printfn "%s" (citizen.ToString())
    printfn "---------------"
