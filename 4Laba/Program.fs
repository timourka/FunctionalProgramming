type IGovService =
    abstract member Name: string
    abstract member Process: string -> unit

[<AbstractClass>]
type GovService(serviceName: string) =
    abstract member PerformService: string -> unit

    interface IGovService with
        member this.Name = serviceName
        member this.Process(citizen: string) =
            this.PerformService(citizen)

type PassportService() =
    inherit GovService("Оформление паспорта")
    override this.PerformService(citizen: string) =
        printfn "505егор: Не удалось оформить паспорт для гражданина %s" citizen

type TaxService() =
    inherit GovService("Уплата налогов")
    override this.PerformService(citizen: string) =
        printfn "505егор: Налог уплачен гражданином %s, вы должны заплатить ещё" citizen

type MedicalService() =
    inherit GovService("Запись к врачу")
    override this.PerformService(citizen: string) =
        printfn "505егор: Гражданин %s не записан на приём к врачу, вам вызваны санитары психбольницы" citizen


//-----------------------------------------------main--------------------------------------------------

let services: IGovService list =
    [ PassportService() :> IGovService;   
    TaxService() :> IGovService;
    MedicalService() :> IGovService ]

let citizen = "Иван Иваныч"

for s in services do
    printfn "Используем сервис: %s" s.Name
    s.Process(citizen)
    printfn "---------------"