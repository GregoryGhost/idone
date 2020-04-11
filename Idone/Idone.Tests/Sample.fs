namespace Idone.Tests
open Microsoft.Extensions.DependencyInjection
open System.Threading

module Tests = 

    open Expecto
    open LanguageExt
    open LanguageExt.UnsafeValueAccess

    open Idone.Security
    open Idone.DAL.DTO
    open Idone.DAL.Dictionaries
    open Idone.DAL.Base
    open Idone.DAL.Base.Extensions
    open Idone.Tests.Constants
    open Idone.Tests.Extensions
    open Idone.Tests.Helpers
    open Idone.Tests.Helpers.IdoneApiHelper

    open Microsoft.Extensions.Configuration
    open Microsoft.Extensions.DependencyInjection
    open Docker.DotNet
    open Docker.DotNet.Models
    open System
    open System.Linq
    open System.Collections.Generic
    open System.Net;
    
    type AppContext = Idone.DAL.Base.AppContext
   
    type Docker =
        {
            Client : DockerClient
            ContainerResponse: CreateContainerResponse
        }
        
    type TestEnviroment =
        {
            ServiceProvider : ServiceProvider
            Docker : Docker
        }
        static member create (provider : ServiceProvider)
                      (dockerClient : DockerClient)
                      (containerResponse : CreateContainerResponse) : TestEnviroment =
            {
                ServiceProvider = provider
                Docker =
                    {
                        Client = dockerClient
                        ContainerResponse = containerResponse
                    }
            }
            
    type ContainerResponse =
        {
            Response : CreateContainerResponse
            HostPort : string
            Ip: string
        }
        
    type ContainerParams =
        {
            ContainerParameters : CreateContainerParameters
            LocalIp : string
        }
        
    let inline toDict (map : ('a * 'b) list) : IDictionary<'a, 'b> =
        map |> Map.ofList |> Map.toSeq |> dict
        
    let private getLocalIpAddress () : string =
        let hostName = Dns.GetHostName()
        let localIp = Dns.GetHostByName(hostName).AddressList.FirstOrDefault()
        if localIp = null then
            raise <| new Exception("Don't found local ip address")
        else
            localIp.ToString()
        
    let private getContainer (client : DockerClient) (image : string) (tag : string) : ContainerResponse =
         let hostPort = "1433"//(new Random(int <| DateTime.UtcNow.Ticks)).Next(10000, 12000)
         let imagesList = new ImagesListParameters()
         let imageMatchName = sprintf "%s:%s" image tag
         imagesList.MatchName <- imageMatchName
         
         let containerResponse() =
             async {
                 let! images = client.Images.ListImagesAsync(imagesList, cancellationToken = CancellationToken.None) |> Async.AwaitTask
                 
                 let pgImage = images.FirstOrDefault()
                 if pgImage = null then
                    let errorMsg = sprintf "Docker image for %s:%s not found" image tag
                    eprintfn "Exception: %s" errorMsg
                    return raise <| new Exception(errorMsg)
                    
                 let containerParameters : ContainerParams =
                    let cp = new CreateContainerParameters()
//                    cp.User <- "gregory"
                    cp.Env <- ["ACCEPT_EULA=Y"; "SA_PASSWORD=<qweQWE1234>";"Database=IdoneTests"] |> ResizeArray<string>
                    cp.ExposedPorts <- [hostPort, new EmptyStruct()] |> toDict 
                    let hostConfig = new HostConfig()
                    let portBind = new PortBinding()
                    let localIp = "0.0.0.0"//getLocalIpAddress()
                    portBind.HostIP <- localIp
                    portBind.HostPort <- sprintf "%s/tcp" hostPort
                    hostConfig.PortBindings <- [hostPort, [portBind] |> ResizeArray<PortBinding> :> IList<PortBinding> ] |> toDict
                    cp.HostConfig <- hostConfig
                    cp.Image <- imageMatchName
                    
                    { ContainerParameters = cp; LocalIp = localIp }
                     
                 let! container =
                     client.Containers.CreateContainerAsync(containerParameters.ContainerParameters, CancellationToken.None)
                     |> Async.AwaitTask
                     
                 let! isStartedContainerDb =
                     client.Containers.StartContainerAsync(
                         container.ID,
                         parameters = new ContainerStartParameters(DetachKeys = sprintf "d=%s" image),
                         cancellationToken = CancellationToken.None
                         ) |> Async.AwaitTask
                 if not isStartedContainerDb then
                     let errorMsg = sprintf "Couldn't start container: %s" container.ID
                     eprintfn "Exception: %s" errorMsg
                     return raise <| new Exception(errorMsg)
                 let portBinding =
                     async {
                         let mutable count = 10
                         Thread.Sleep(5000)
                         let getContainerResponse() = client.Containers.InspectContainerAsync(container.ID, CancellationToken.None)|> Async.AwaitTask
                         let! response = getContainerResponse()
                         let mutable containerStat = response
                         while not containerStat.State.Running && count > 0 do
                            count <- count - 1
                            Thread.Sleep(1000)
                            let! response = getContainerResponse()
                            containerStat <- response
                            
                         printfn "debug: %s" <| Newtonsoft.Json.JsonConvert.SerializeObject containerStat
                         return (containerStat.HostConfig.PortBindings.TryGetValue(hostPort) |> takeFirst |> (fun x -> x.ValueUnsafe().First()))
                     } |> Async.RunSynchronously
                     
                 return { Response = container; HostPort = hostPort; Ip = "172.17.0.4" }
                 //TODO: переписать на получение адреса и порта контейнера БД от докера
         }
         Async.RunSynchronously <| containerResponse()
         
    let private removeDockerContainer (docker : Docker) : Async<unit> =
            async {
                let! stopContainerResult =
                    docker.Client.Containers.StopContainerAsync(docker.ContainerResponse.ID,
                                                               new ContainerStopParameters(),
                                                               CancellationToken.None) |> Async.AwaitTask
                if stopContainerResult then
                     return! docker.Client.Containers.RemoveContainerAsync(docker.ContainerResponse.ID,
                                                                       new ContainerRemoveParameters(),
                                                                       CancellationToken.None) |> Async.AwaitTask
                if docker.Client <> null then
                    docker.Client.Dispose()
            }
   
    let private initDi (connString : string) : ServiceProvider =
        let services = new ServiceCollection()
        services.AddIdoneIdentity()
            .AddIdoneDb(connString)
            .AddSecurityDi() |> ignore
        let rootServiceProvider = services.BuildServiceProvider()
        use scope = rootServiceProvider.CreateScope()
        scope.ServiceProvider.GetRequiredService<AppContext>().InitTest()
        
        rootServiceProvider
     
    let private initTestEnviroment() : TestEnviroment =
        //TODO: создать контейнер для AD сервера
        //TODO: зарегать тестового пользователя в AD
        let dockerClient =
//            let url = new Uri("npipe://./pipe/docker_engine") for windows
            let url = new Uri("unix:///var/run/docker.sock") //for unix
            let config = new DockerClientConfiguration(url, credentials = null, defaultTimeout = TimeSpan.FromSeconds 10000.)
            config.CreateClient()
        let (containerResponse, hostPort, dbIp) =
            let containerInfo = getContainer dockerClient "mcr.microsoft.com/mssql/server" "2019-CU3-ubuntu-18.04"
            (containerInfo.Response, containerInfo.HostPort, containerInfo.Ip)
        
        let connString = sprintf "Server=%s,%s;Database=IdoneTests;User ID=SA;Password=<qweQWE1234>;Trusted_Connection=False;" dbIp hostPort
       //TODO: все данные для подключения нужно загнать в константы
        try
            let di = initDi connString
            let dbEnv = TestEnviroment.create di dockerClient containerResponse
            dbEnv
        with
            | :? Exception as ex ->
                let docker = { Client = dockerClient; ContainerResponse = containerResponse  }
                removeDockerContainer docker
                eprintf "connString: %s" connString
                raise ex
                
    let private initTestEnviroment2() : ServiceProvider =
        let connString = sprintf "Server=%s,%s;Database=IdoneTests;User ID=SA; Password=<qweQWE1234>;Trusted_Connection=False;" "172.17.0.4" "1433"
       
        try
            initDi connString
        with
            | :? Exception as ex ->
                eprintf "connString: %s" connString
                raise ex
        
    let private tearDownAfterAllTest (docker : Docker) (test : Test) : Test =
        removeDockerContainer docker |> Async.RunSynchronously
        test


    [<Tests>]
    let tests =
//      let testEnv2 = initTestEnviroment2()
      let testEnv = initTestEnviroment()
      let _servicesProvider = testEnv.ServiceProvider
      let _docker = testEnv.Docker
//      let _servicesProvider = testEnv2
      
      let _security = new SecurityModuleWrapper(_servicesProvider)

      let clearUsers() =
        let dbContext = _servicesProvider.GetService<AppContext>()
        dbContext.Users.Clear()
        dbContext.SaveChanges()

      let clearRolesPerms() =
        let dbContext = _servicesProvider.GetService<AppContext>()
        dbContext.RolePermissions.Clear()
        dbContext.Roles.Clear()
        dbContext.Permissions.Clear()
        dbContext.SaveChanges()

      let clearUserRoles() =
        let dbContext = _servicesProvider.GetService<AppContext>()
        dbContext.UserRoles.Clear()
        dbContext.Users.Clear()
        dbContext.Roles.Clear()
        dbContext.SaveChanges()
      
      tearDownAfterAllTest _docker <<
      testSequencedGroup "Последовательное выполнение тестов по работе с БД" 
        <| testList "Модуль админки" [       
//        test "Регистрация нового пользователя" {
//            //1.Найти пользователя в домене (берем хардкод данные)
//            //2.Получить данные требуемого пользователя у AD сервера
//            //3.Передать данные пользователя на регистрацию в системе
//            //4.Найти зареганного пользователя в системе (в гриде всех пользователей)
//
//          //use cases
//          let registratedUser = either {
//            let! registratedUser =
//                _security.RegistrateUserOnDomainUser SEARCH_DEFAULT_USER
//            return! SEARCH_NAME_USER |> _security.FindRegistratedUser
//          }
//
//          Expect.isRight registratedUser "Пользователь не зарегистрирован"
//          clearUsers() |> ignore
//        }
//
//        test "Назначение ролей пользователю" {
//            //1. Зарегистрировать пользователя
//            //2. Создать роли
//            //3. Назначить роли пользователю
//            //4. Получить роли пользователя
//            //5. Получить пользователя из всех назначенных ролей
//                
//            let userRoles : Either<Error, DtoGridRole> = either {
//                let! registratedUser = 
//                    _security.RegistrateUserOnDomainUser SEARCH_DEFAULT_USER
//                let roles =
//                    ADMIN_AND_USER_ROLES |> _security.CreateRoles
//                let! resultSettedRoles = 
//                    _security.SetRolesForUser(ADMIN_AND_USER_ROLES, registratedUser)
//
//                return! registratedUser |> fillGridQueryUserRole |> _security.GetGridUserRoles
//            }
//
//            Expect.isRight userRoles "Не найдены пользовательские роли"
//            clearUserRoles() |> ignore
//        }
        
        test "Назначение прав для роли" {
            //1. Создать роли
            //2. Назначить права для ролей
            //3. Получить права ролей
            //4. Получить роли из всех назначенных прав

            let startRoles = PERMS_ROLES_LINKS |> getRoles
            let startPerms = PERMS_ROLES_LINKS |> getPerms
            let linksLength = List.length PERMS_ROLES_LINKS

            let createdRoles = 
                    startRoles |> _security.CreateRoles
            Expect.hasLength <||| (createdRoles,
                                    linksLength,
                                    "Не удалось создать роли")

            let createdPerms =
                startPerms |> _security.CreatePermissions
            Expect.hasLength <||| (createdPerms,
                                    linksLength, 
                                    "Не удалось создать права")
            let rolePermLinks =
                bindData createdRoles createdPerms
            let result = 
                _security.SetPermissionsForRole(rolePermLinks)
            Expect.hasLength <||| (result,
                            linksLength,
                            "Не удалось назначить права для ролей")
                
            let perms =
                _security.GetRolesPermissions(startRoles)
            Expect.hasLength <||| (perms,
                linksLength,
                "Не удалось получить назначенные права для ролей")

            let roles =
                _security.GetPermissionsRoles(startPerms)
            Expect.hasLength <||| (roles,
                linksLength,
                "Не найдены роли, назначенных прав")
            
            clearRolesPerms() |> ignore
        }

//        test "Назначены права для пользователя(через роли)" {
//            //Создать роли
//            //Создать права
//            //Назначить права для роли
//            //Зарегистрировать пользователя
//            //Назначить роли пользователю
//            //Получить права пользователя
//            //Сравнить назначенныые права и полученные права
//            let startRoles = PERMS_ROLES_LINKS |> getRoles
//            let startPerms = PERMS_ROLES_LINKS |> getPerms
//            let linksLength = List.length PERMS_ROLES_LINKS
//
//            let createdRoles = 
//                    startRoles |> _security.CreateRoles
//            Expect.hasLength <||| (createdRoles,
//                                    linksLength,
//                                    "Не удалось создать роли")
//
//            let createdPerms =
//                startPerms |> _security.CreatePermissions
//            Expect.hasLength <||| (createdPerms,
//                                    linksLength, 
//                                    "Не удалось создать права")
//            let rolePermLinks =
//                bindData createdRoles createdPerms
//            let result = 
//                _security.SetPermissionsForRole(rolePermLinks)
//            Expect.hasLength <||| (result,
//                            linksLength,
//                            "Не удалось назначить права для ролей")
//            
//            let userPerms = either {
//                let! registratedUser =
//                    _security.RegistrateUserOnDomainUser SEARCH_DEFAULT_USER
//                let! resultSettedUserRoles=
//                    _security.SetRolesForUser(ADMIN_AND_USER_ROLES, registratedUser)
//                return! registratedUser |> fillGridQueryUserPerms |> _security.GetUserPermissions
//            }
//            
//            Expect.isRight userPerms "Не найдены пользовательские права"
//            
//            let actualUserPerms = userPerms.ValueUnsafe().Rows
//            Expect.hasLength actualUserPerms linksLength "Не найдены пользовательские права"
//            
//            clearRolesPerms() |> ignore
//        }
      ]
