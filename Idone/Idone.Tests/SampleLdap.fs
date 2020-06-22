namespace Idone.Tests

module AdTests =
    open Expecto
    
    open LanguageExt.UnsafeValueAccess
    
    open Idone.Security.Services
    open Idone.DAL.DTO
    
    open Idone.Tests.Extensions
    open Idone.Tests.Constants
    open Idone.Tests
    open Idone.Tests.Types
    
    open Newtonsoft.Json
    
    type Deps =
        {
            Docker : Docker
            AdContainerInfo : ContainerResponse
        }
    
    let initDeps() : Deps =
        let dockerClient = Tests.getDockerClient()
        let info = Tests.getContainer <| makeAdContainerSettings dockerClient
        let docker = { Client = dockerClient; Containers = [info.Response] }
        
        { Docker = docker; AdContainerInfo = info }
    
//    [<Tests>]
    let tests =
        let _deps = initDeps()
        
        testSequencedGroup "Последовательное выполнение тестов по работе с OpenLDAP" 
        <| testList "Проверка работы с АД" [
            test "Создание и получение пользователя" {
                let domain = _deps.AdContainerInfo.Ip
                let ad = new AdService(domain, AD_LOGIN, AD_PASSWORD)
                
                let newUser = new DtoNewAdUser("Кулаков", "Григорий", "Викторович", "test@mail.ru", "gregory", "qweQWE1234")
                let createdUser = ad.CreateUser newUser
                Expect.isRight createdUser "Ожидалось создать AD пользователя"
                
                let filterDisplayName = "Кулаков*"
                let foundUsers = ad.FindUsersByDisplayName filterDisplayName
                
                System.Console.WriteLine("found users " + JsonConvert.SerializeObject(foundUsers))
                
                Expect.isRight foundUsers "Ожидалось найти зарегистрированного AD пользователя"
                if foundUsers.ValueUnsafe() |> Seq.isEmpty then
                    let errorMsg = sprintf "Not found AD-user by filter %s" filterDisplayName
                    raise <| new System.Exception(errorMsg)
            }
            testAsync "Удаление докер контейнеров" {
                return! Tests.removeDockerContainer _deps.Docker
            }
        ]
