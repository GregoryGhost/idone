namespace Idone.Tests

module Tests = 

    open Expecto
    open Idone.Security
    open Idone.Tests.Extensions
    open LanguageExt

    open Idone.DAL.DTO
    open Idone.DAL.Dictionaries
    open Idone.DAL.Base
    open Idone.DAL.Base.Extensions

    open Microsoft.Extensions.Configuration
    open Microsoft.Extensions.DependencyInjection
    open Idone.Tests.Helpers
    open Idone.Tests.Helpers.IdoneApiHelper


    let private initTestEnviroment() =
        let config = new ConfigurationBuilder()
        config.AddJsonFile("appsettings.Development.json") |> ignore
        let connString = config.Build().GetConnectionString("default")

        let services = new ServiceCollection()
        services.AddIdoneIdentity()
            .AddIdoneDb(connString)
            .AddSecurityDi() |> ignore
        let rootServiceProvider = services.BuildServiceProvider()
        use scope = rootServiceProvider.CreateScope()
        scope.ServiceProvider.GetRequiredService<AppContext>().InitTest()

        rootServiceProvider


    [<Tests>]
    let tests =
      let _servicesProvider = initTestEnviroment()
      let _security = new SecurityModuleWrapper(_servicesProvider)

      let clearUsers() =
        let dbContext = _servicesProvider.GetService<AppContext>()
        dbContext.Users.Clear()
        dbContext.SaveChanges()

      testList "Модуль админки" [       
        test "Регистрация нового пользователя" {
            //1.Найти пользователя в домене (берем хардкод данные)
            //2.Получить данные требуемого пользователя у AD сервера
            //3.Передать данные пользователя на регистрацию в системе
            //4.Найти зареганного пользователя в системе (в гриде всех пользователей)

          //use cases
          let SEARCH_EXPRESSION = "Кулаков*"
          let registratedUser = either {
            let! firstDomainUser = _security.FindFirstDomainUser SEARCH_EXPRESSION
            let! registratedUser = 
                firstDomainUser |> (fillUserCredentials >> _security.RegistrateUser)
            return! registratedUser |> _security.FindRegistratedUser
          }

          Expect.isRight registratedUser "Пользователь не зарегистрирован"
          clearUsers() |> ignore
        }

        test "Назначение ролей пользователю" {
            //1. Зарегистрировать пользователя
            //2. Создать роли
            //3. Назначить роли пользователю
            //4. Получить роли пользователя
            //5. Получить пользователя из всех назначенных ролей
            //let registratedUser : Either<Error, DtoRegistratedUser> = 
            //    registrateUserOnDomainUser SEARCH_EXPRESSION
                
            //let userRoles : Either<Error, DtoGridRole> = 
            //    let createdRoles : Either<Error, DtoGridRole> =
            //        ROLES |> prepareRoleData >>= createRoles
            //    createdRoles >>= setRolesForUser >>= getRolesOfUser

            //let flattenDuplicates (records : Record<'a> seq) : Record<'a> option =
            //    records
            //    |> Seq.distinct
            //    |> Seq.tryExactlyOne

            //let foundUsersOfRoles : Either<Error, DtoGridUser> =
            //    let getUsersOfRoles (roles : DtoGridRole) : Either<Error, DtoRowUser> =
            //        roles 
            //        |> Seq.map getUsersByRole
            //        |> flattenDuplicates
            //    ROLES |> getGridRole >>= getUsersOfRoles

            Expect.equal 1 1
        }

        test "Назначение прав для роли" {
            //1. Создать роли
            //2. Назначить права для роли
            //3. Получить права роли
            //4. Получить роль из всех назначенных прав
            Expect.equal 1 1
        }

        test "Назначены права для пользователя(через роли)" {
            //1. Зарегистрировать пользователя
            //2. Создать роли
            //3. Назначить права для роли
            //3. Назначить роли пользователю
            //4. Получить роли пользователя
            //5. Получить права ролей
            //6. Получить права пользователя
            //7. Получить пользователей для права
            Expect.equal 1 1
        }
        //TODO: добавить тесты для проверки работы с ролями, правами, пользователями
      ]
