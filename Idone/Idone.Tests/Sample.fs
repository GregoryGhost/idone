namespace Idone.Tests

module Tests = 

    open Expecto
    open LanguageExt

    open Idone.Security
    open Idone.DAL.DTO
    open Idone.DAL.Dictionaries
    open Idone.DAL.Base
    open Idone.DAL.Base.Extensions
    open Idone.Tests.Extensions
    open Idone.Tests.Helpers
    open Idone.Tests.Helpers.IdoneApiHelper

    open Microsoft.Extensions.Configuration
    open Microsoft.Extensions.DependencyInjection


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
          let registratedUser = either {
           //TODO: нужно заменить DtoUserFilter на поиск по имени, а не мылу
           //TODO: возвращать после регистрации пользователя его id, а не мыло,
           //   так как id у всех сущностей должен быть одного типа.
            let! registratedUser =
                _security.RegistrateUserOnDomainUser SEARCH_DEFAULT_USER
            return! SEARCH_NAME_USER |> _security.FindRegistratedUser
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
