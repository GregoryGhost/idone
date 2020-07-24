namespace Idone.Tests

module Tests = 

    open Expecto
    open LanguageExt
    open LanguageExt.UnsafeValueAccess

    open Idone.Security
    open Idone.Security.Services
    open Idone.DAL.DTO
    open Idone.DAL.Dictionaries
    open Idone.DAL.Base
    open Idone.DAL.Base.Extensions
    open Idone.Tests.Constants
    open Idone.Tests.Extensions
    open Idone.Tests.Helpers
    open Idone.Tests.Helpers.IdoneApiHelper
    open Idone.Tests.Types

    open Microsoft.Extensions.DependencyInjection
    open Docker.DotNet
    open Docker.DotNet.Models
    open System
    open System.Linq
    open System.Collections.Generic
    open System.Threading
    
    type AppContext = Idone.DAL.Base.AppContext  
        
    let inline toDict (map : ('a * 'b) list) : IDictionary<'a, 'b> =
        map |> Map.ofList |> Map.toSeq |> dict
    
    let private pullImage (client : DockerClient) (image : string) (tag : string) : Async<unit> =
        async {
            let createParams = new ImagesCreateParameters()
            createParams.FromImage <- sprintf "%s:%s" image tag
            let cancelDownload = new CancellationTokenSource()
            let report = new Progress<JSONMessage>(fun msg ->
                printfn "%s|%s|%s" msg.Status msg.ProgressMessage msg.ErrorMessage)
            let! _ =
                client.Images.CreateImageAsync(createParams, new AuthConfig(), report, cancelDownload.Token)
                |> Async.AwaitTask    
            printfn "Ok pulled image %s to %s" image
            <| client.Configuration.EndpointBaseUri.ToString()
            
            ()
        }
        
    let private getImage (client : DockerClient)
                         (imagesList : ImagesListParameters) : Async<ImagesListResponse option> =
         async {
             let! images = client.Images.ListImagesAsync(imagesList, cancellationToken = CancellationToken.None) |> Async.AwaitTask
             let foundImage = images.FirstOrDefault() |> Option.ofObj
             
             return foundImage
         }
     
    let getDockerClient() : DockerClient =
//          let url = new Uri("npipe://./pipe/docker_engine") for windows
        let url = new Uri("unix:///var/run/docker.sock") //for unix
        let config = new DockerClientConfiguration(url, credentials = null, defaultTimeout = TimeSpan.FromSeconds 10000.)
        
        config.CreateClient()
        
    let getContainer (containerSettings: ContainerSettings) : ContainerResponse =
         let imagesList = new ImagesListParameters()
         let (client, image, tag, env, port) =
             (containerSettings.Client,
              containerSettings.Image,
              containerSettings.Tag,
              containerSettings.Env,
              containerSettings.Port)
         let imageMatchName = sprintf "%s:%s" image tag
         imagesList.MatchName <- imageMatchName
         
         let containerResponse() : Async<ContainerResponse> =
             async {                 
                 let! foundImage = getImage client imagesList
                 if foundImage.IsNone then
                    printf "Docker image for %s:%s not found, try to pull" image tag
                    //TODO: проблема с тем, что скачивает образ несколько раз разных версий =)
                    let! _ = pullImage client image tag
                    ()
                    
                 let containerParameters : ContainerParams =
                    let cp = new CreateContainerParameters()
                    cp.Env <- env 
                    cp.ExposedPorts <- [port, new EmptyStruct()] |> toDict 
                    let hostConfig = new HostConfig()
                    let portBind = new PortBinding()
                    let localIp = "0.0.0.0"
                    portBind.HostIP <- localIp
                    portBind.HostPort <- sprintf "%s/tcp" port
                    hostConfig.PortBindings <- [port, [portBind] |> ResizeArray<PortBinding> :> IList<PortBinding> ] |> toDict
                    cp.HostConfig <- hostConfig
                    cp.Image <- imageMatchName
                    
                    { ContainerParameters = cp; LocalIp = localIp }
                     
                 let! container =
                     client.Containers.CreateContainerAsync(containerParameters.ContainerParameters,
                                                            CancellationToken.None)
                     |> Async.AwaitTask
                     
                 let! isStartedContainer =
                     client.Containers.StartContainerAsync(
                         container.ID,
                         parameters = new ContainerStartParameters(DetachKeys = sprintf "d=%s" image),
                         cancellationToken = CancellationToken.None
                         ) |> Async.AwaitTask
                 if not isStartedContainer then
                     let errorMsg = sprintf "Couldn't start container: %s" container.ID
                     eprintfn "Exception: %s" errorMsg
                     
                     return raise <| new Exception(errorMsg)
                     
                 let! ipAddress =
                     async {
                         let! _ = Async.Sleep 10000
                         let! containerStat =
                             client.Containers.InspectContainerAsync(container.ID, CancellationToken.None)
                             |> Async.AwaitTask
                            
                         let ip =
                             containerStat.NetworkSettings.Networks
                                 .TryGetValue("bridge").ValueUnsafe()
                                 .IPAddress
                         
                         return ip
                      }
                     
                 return { Response = container; HostPort = port; Ip = ipAddress }
             }
         Async.RunSynchronously <| containerResponse()
         
    let removeDockerContainer (docker : Docker) : Async<unit> =
            async {
                for container in docker.Containers do
                    let! stopContainerResult =
                        docker.Client.Containers.StopContainerAsync(container.ID,
                                                                   new ContainerStopParameters(),
                                                                   CancellationToken.None) |> Async.AwaitTask
                    if stopContainerResult then
                         return! docker.Client.Containers.RemoveContainerAsync(container.ID,
                                                                           new ContainerRemoveParameters(),
                                                                           CancellationToken.None) |> Async.AwaitTask
                if docker.Client <> null then
                    docker.Client.Dispose()
            }
   
    let private initDi (connString : string) (domain : string) : ServiceProvider =
        let services = new ServiceCollection()
        services.AddIdoneIdentity()
            .AddIdoneDb(connString)
            .AddSecurityDi(domain, AD_LOGIN, AD_PASSWORD) |> ignore
        let rootServiceProvider = services.BuildServiceProvider()
        use scope = rootServiceProvider.CreateScope()
        scope.ServiceProvider.GetRequiredService<AppContext>().InitTest()
        
        rootServiceProvider
     
    let private initTestEnviroment() : TestEnvironment =
        let dockerClient = getDockerClient()
        let (dbContainer, hostPort, dbIp) =
            let containerInfo = getContainer <| makeDbContainerSettings dockerClient
            (containerInfo.Response, containerInfo.HostPort, containerInfo.Ip)
        let (adContainer, adIp) =
            let containerInfo = getContainer <| makeAdContainerSettings dockerClient
            (containerInfo.Response, containerInfo.Ip)
        let containers = [dbContainer; adContainer]
        let connString = makeDatabaseSettings { IpAddress = dbIp; Port = hostPort }
        
        let di = 
            try
               initDi connString adIp
            with
                | :? Exception as ex ->
                    let docker = { Containers = containers; Client = dockerClient }
                    eprintf "connstring = %s" connString
                    eprintf "ad domain = %s" adIp
                    removeDockerContainer docker |> Async.RunSynchronously
                    raise ex
        
        let dbEnv = TestEnvironment.create di dockerClient containers
        
        dbEnv

    [<Tests>]
    let tests =
      let testEnv = initTestEnviroment()
      let _servicesProvider = testEnv.ServiceProvider
      let _docker = testEnv.Docker
      
      let _security = new SecurityModuleWrapper(_servicesProvider)

      let clearAll() =
        let dbContext = _servicesProvider.GetService<AppContext>()
        dbContext.RolePermissions.Clear()
        dbContext.UserRoles.Clear()
        dbContext.Users.Clear()
        dbContext.Roles.Clear()
        dbContext.SaveChanges()
      
      testSequencedGroup "Последовательное выполнение тестов по работе с БД" 
        <| testList "Модуль админки" [
        test "Подготовка пользователей OpenLDAP" {
            let ad = _servicesProvider.GetService<AdService>()
            let newUser = new DtoNewAdUser("Кулаков", "Григорий", "Викторович", "test@mail.ru", "gregory", "qweQWE1234")
            let createdUser = ad.CreateUser newUser
            
            Expect.isRight createdUser "Не удалось создать Active Directory пользователя"
        }
        
        test "Регистрация нового пользователя" {
            //1.Найти пользователя в домене (берем хардкод данные)
            //2.Получить данные требуемого пользователя у AD сервера
            //3.Передать данные пользователя на регистрацию в системе
            //4.Найти зареганного пользователя в системе (в гриде всех пользователей)
          clearAll() |> ignore
          //use cases
          let registratedUser = either {
            let! registratedUser =
                _security.RegistrateUserOnDomainUser SEARCH_DEFAULT_USER
            return! SEARCH_NAME_USER |> _security.FindRegistratedUser
          }

          Expect.isRight registratedUser "Пользователь не зарегистрирован"
          clearAll() |> ignore
        }

        test "Назначение ролей пользователю" {
            //1. Зарегистрировать пользователя
            //2. Создать роли
            //3. Назначить роли пользователю
            //4. Получить роли пользователя
            //5. Получить пользователя из всех назначенных ролей
            clearAll() |> ignore
                          
            let userRoles : Either<Error, DtoGridRole> = either {
                let! registratedUser = 
                    _security.RegistrateUserOnDomainUser SEARCH_DEFAULT_USER
                let roles =
                    ADMIN_AND_USER_ROLES |> _security.CreateRoles
                let! resultSettedRoles = 
                    _security.SetRolesForUser(ADMIN_AND_USER_ROLES, registratedUser)

                return! registratedUser |> fillGridQueryUserRole |> _security.GetGridUserRoles
            }

            Expect.isRight userRoles "Не найдены пользовательские роли"
            clearAll() |> ignore
        }
        
        test "Назначение прав для роли" {
            //1. Создать роли
            //2. Назначить права для ролей
            //3. Получить права ролей
            //4. Получить роли из всех назначенных прав
            clearAll() |> ignore
          
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
                
            printfn "role perm links: %A" rolePermLinks
            
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
            
            clearAll() |> ignore
        }
        
        test "Создание ролей" {
            clearAll() |> ignore
            
            let createdRoles =
                ADMIN_AND_USER_ROLES |> _security.CreateRoles
            let rolesLength = List.length ADMIN_AND_USER_ROLES
            Expect.hasLength <||| (createdRoles,
                                    rolesLength,
                                    "Не удалось создать роли")
            
            let gotRoles =
                fillGridQueryRoleFirstPage |> _security.GetGridRoles
            Expect.isRight gotRoles "Не найдены созданные роли"
            let unexpectedBehaviorMsg = sprintf "Не состыковочка с созданными ролями, gotRoles: %A" <| gotRoles.ValueUnsafe().Rows.ToList()
            Expect.hasLength <||| (gotRoles.ValueUnsafe().Rows,
                                  rolesLength,
                                  unexpectedBehaviorMsg)
            clearAll() |> ignore
        }
        
        test "Поиск ролей по ид" {
            clearAll() |> ignore
            
            let createdRoles =
                ADMIN_AND_USER_ROLES |> _security.CreateRoles
            let rolesLength = List.length ADMIN_AND_USER_ROLES
            Expect.hasLength <||| (createdRoles,
                                    rolesLength,
                                    "Не удалось создать роли")

            let gotRoles =
                createdRoles |> _security.GetRolesByIds
            Expect.isNonEmpty gotRoles "Не найдены созданные роли по идам"
            let unexpectedBehaviorMsg = sprintf "Не состыковочка с созданными ролями, gotRoles: %A" <| gotRoles
            Expect.hasLength <||| (gotRoles,
                                  rolesLength,
                                  unexpectedBehaviorMsg)
            clearAll() |> ignore
        }

        test "Назначены права для пользователя(через роли)" {
            //Создать роли
            //Создать права
            //Назначить права для роли
            //Зарегистрировать пользователя
            //Назначить роли пользователю
            //Получить права пользователя
            //Сравнить назначенныые права и полученные права
            clearAll() |> ignore
            
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
            
            let userPerms = either {
                let! registratedUser =
                    _security.RegistrateUserOnDomainUser SEARCH_DEFAULT_USER
                let! resultSettedUserRoles =
                    _security.SetRolesForUser(ADMIN_AND_USER_ROLES, registratedUser)
                return! registratedUser |> fillGridQueryUserPerms |> _security.GetUserPermissions
            }
            
            Expect.isRight userPerms "Не найдены пользовательские права"
            
            let actualUserPerms = userPerms.ValueUnsafe().Rows
            Expect.hasLength actualUserPerms linksLength "Не найдены пользовательские права"
            
            clearAll() |> ignore
        }
        
        testAsync "Удаление докер контейнеров" {
            return! removeDockerContainer _docker
        }
      ]
