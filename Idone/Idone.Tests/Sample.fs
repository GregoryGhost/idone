namespace Idone.Tests
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
        }
        
    let toDict (map : (_ * _) list) : Dictionary<_, _> =
        let t = map |> Map.ofList |> Map.toSeq |> dict
        t :?> Dictionary<_, _>
        
    let private getContainer (client : DockerClient) (image : string) (tag : string) : Async<ContainerResponse> =
         async {
             let hostPort = (new Random((int) DateTime.UtcNow.Ticks)).Next(10000, 12000)
             let imagesList = new ImagesListParameters()
             let imageMatchName = sprintf "%s:%s" image tag
             imagesList.MatchName <- imageMatchName
             let! images = client.Images.ListImagesAsync(imagesList, CancellationToken.None) |> Async.AwaitTask
             
             let pgImage = images.FirstOrDefault()
             if pgImage = null then
                let errorMsg = sprintf "Docker image for %s:%s not found" image tag
                raise <| Exception errorMsg
             let! container =
                let containerParameters = new CreateContainerParameters()
                containerParameters.User <- "postgres"
                containerParameters.Env <- ["POSTGRES_PASSWORD=password";"POSTGRES_DB=repotest";"POSTGRES_USER=postgres"] |> ResizeArray<string>
                containerParameters.ExposedPorts <- ["5432", new EmptyStruct()] |> toDict 
                let hostConfig = new HostConfig()
                let portBind = new PortBinding()
                portBind.HostIP <- "0.0.0.0"
                portBind.HostPort <- sprintf "%d" hostPort
                hostConfig.PortBindings <- ["5432", [portBind] |> ResizeArray<PortBinding>] |> toDict
                containerParameters.HostConfig <- hostConfig
                containerParameters.Image <- imageMatchName
                 
                client.Containers.CreateContainerAsync(containerParameters, CancellationToken.None) |> Async.AwaitTask
              
             let! isStartedContainerDb =
                 client.Containers.StartContainerAsync(
                     container.ID,
                     parameters = new ContainerStartParameters(DetachKeys = sprintf "d=%s" image),
                     cancellationToken = CancellationToken.None
                     ) |> Async.AwaitTask
             if not isStartedContainerDb then
                 let errorMsg = sprintf "Couldn't start container: %s" container.ID
                 raise <| Exception errorMsg
             
             let! waitInspectContainer =
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
                     return ()
                 }
                 
             return { Response = container; HostPort = sprintf "%d" hostPort }
         }
         

    let private initTestEnviroment() : TestEnviroment =
        //TODO: создать контейнер для AD сервера
        //TODO: зарегать тестового пользователя в AD
        let dockerClient =
            let url = new Uri("npipe://./pipe/docker_engine")
            let config = new DockerClientConfiguration(url, credentials = null, defaultTimeout = Unchecked.defaultof<TimeSpan>)
            config.CreateClient()
        let (containerResponse, hostPort) =
            let containerInfo = getContainer dockerClient "mssql" "TODO: tag image" |> Async.RunSynchronously
            (containerInfo.Response, containerInfo.HostPort)
        
        let connString = "TODO://"
        
        let services = new ServiceCollection()
        services.AddIdoneIdentity()
            .AddIdoneDb(connString)
            .AddSecurityDi() |> ignore
        let rootServiceProvider = services.BuildServiceProvider()
        use scope = rootServiceProvider.CreateScope()
        scope.ServiceProvider.GetRequiredService<AppContext>().InitTest()

        let dbEnv = TestEnviroment.create rootServiceProvider dockerClient containerResponse
        dbEnv
        
    let private tearDownAfterAllTest (docker : Docker) (test : Test) : Test =
        let removeDockerContainer =
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
        test


    [<Tests>]
    let tests =
      let testEnv = initTestEnviroment()
      let _servicesProvider = testEnv.ServiceProvider
      let _docker = testEnv.Docker
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
