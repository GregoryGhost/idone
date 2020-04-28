namespace Idone.Tests.Constants

[<AutoOpen>]
module Constants =
    open Idone.DAL.DTO
    open Idone.Tests.Types
    open Docker.DotNet

    let FIRST_PAGE = new Pagination(10, 1)
    
    let ADMIN_AND_USER_ROLES : Role list =
        [
            { Name = "админ" }
            { Name = "пользователь" }
        ]

    let PERMISSIONS : Perm list = 
        [
            { Name = "тестовое_резрешение"; Description = "описание" }
        ]

    let PERMS_ROLES_LINKS : PermRoleLink list = [
        {
            Role = { Name = "админ" }
            Perm = { Name = "разрешение1"; Description = "описание" }
        };
        {
            Role = { Name = "пользователь" }
            Perm = { Name = "разрешение2"; Description = "описание" }
        }
    ]
        
    let SEARCH_DEFAULT_USER = "Кулаков*"

    let SEARCH_NAME_USER = "Кулаков"
    
    let private SA_PSWD = "<qweQWE1234>"
    let private DATABASE_TESTS = "idoneTests"
    let DEFAULT_DOCKER_DB_PORT = "1433"
    
    let makeDatabaseSettings (server : Address) : string =
        {
            Server = server
            Database = DATABASE_TESTS
            UserId = "SA"
            Pswd = SA_PSWD
            TrustedConnection = false
        }.toConnectionString
        
    let DOCKER_DB_ENV : ResizeArray<string> =
        {
            AcceptEula = true
            SaPswd = SA_PSWD
            Database = DATABASE_TESTS
        }.toEnv
    
    let makeDbContainerSettings (client : DockerClient) : ContainerSettings =
        {
            Client = client
            Image = "mcr.microsoft.com/mssql/server"
            Tag = "2019-CU3-ubuntu-18.04"
            Port = "1433"
            Env = DOCKER_DB_ENV
        }
        
    let SETTINGS_FILE_NAME = "appsettings.Development.json"
        
    let DOCKER_AD_ENV : ResizeArray<string> =
        {
            AcceptEula = true
            SaPswd = SA_PSWD
            Database = DATABASE_TESTS
        }.toEnv    
        
    let makeAdContainerSettings (client : DockerClient) : ContainerSettings =
        {
            Client = client
            Image = "osixia/openldap"
            Tag = "1.3.0"
            Port = "1434"
            Env = DOCKER_AD_ENV
        }
        
    let AD_LOGIN = "cn=admin,dc=example,dc=org"
    let AD_PASSWORD = "admin"
        