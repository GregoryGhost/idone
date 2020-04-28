namespace Idone.Tests

module AdTests =
    open Expecto
    
    open LanguageExt.UnsafeValueAccess
    
    open Idone.Security.Services
    open Idone.DAL.DTO
    
    open Idone.Tests.Extensions
    open Idone.Tests.Constants
    
    open Newtonsoft.Json
    
    let tests =
        let domain = "172.17.0.4"
        testSequencedGroup "Последовательное выполнение тестов по работе с OpenLDAP" 
        <| testList "Проверка работы с АД" [
//            test "Создание пользователя" {
//                let ad = new AdService(domain, AD_LOGIN, AD_PASSWORD)
//                let newUser = new DtoNewAdUser("Кулаков", "Григорий", "Викторович", "test@mail.ru", "gregory", "qweQWE1234")
//                let createdUser = ad.CreateUser newUser
//                
//                Expect.isRight createdUser
//            }
            test "Получения пользователя" {
                let ad = new AdService(domain, AD_LOGIN, AD_PASSWORD)
                
                let filterDisplayName = "Кулаков*"
                let foundUsers = ad.FindUsersByDisplayName filterDisplayName
                
                printfn "found users %s" <| JsonConvert.SerializeObject(foundUsers)
                
                Expect.isRight foundUsers
                if foundUsers.ValueUnsafe() |> Seq.isEmpty then
                    let errorMsg = sprintf "Not found AD-user by filter %s" filterDisplayName
                    raise <| new System.Exception(errorMsg)
            }
        ]
