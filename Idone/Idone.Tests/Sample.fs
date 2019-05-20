namespace Idone.Tests

module Tests = 

    open Expecto
    open Idone.Security
    open Idone.Tests.Extensions
    open LanguageExt

    let private _security = new EnterPoint() :> ISecurityModule

    [<Tests>]
    let tests =
      testList "Модуль админки" [       
        test "Option Some language-ext'а" {
          let security = new EnterPoint()
          let actual = security.RegistrateUser(new Test()) |> FSharp.fs

          Expect.isSome actual "Не совпали"
        }    
    
        test "Option None language-ext'а" {
          let security = new EnterPoint()
          let actual = security.RegistrateUser(null) |> FSharp.fs

          Expect.isNone actual "Не совпали"
        }

        test "Either Right language-ext'а" {
          let security = new EnterPoint()
          let actual = security.RegistrateUserEither(new Test())

          Expect.isRight actual "Не совпали"
        }    
    
        test "Either Left language-ext'а" {
          let security = new EnterPoint()
          let actual = security.RegistrateUserEither(null)

          Expect.isLeft actual "Не совпали"
        }    

        test "Either areEqual Records language-ext'а" {
          let security = new EnterPoint()
          let test = new Test(666)
          let actual = security.RegistrateUser2(test)
          let expected = new Test(666)

          Expect.areEqual actual expected "Не совпали"
        }

        test "Регистрация нового пользователя" {
            //1.Найти пользователя в домене (берем хардкод данные)
            //2.Получить данные требуемого пользователя у AD сервера
            //3.Передать данные пользователя на регистрацию в системе
            //4.Найти зареганного пользователя в системе (в гриде всех пользователей)
          
          let findDomainUser =
            _security.FindUserByDisplayName

          let fillUserCredentials userData =
            new DtoRegistrateUser(userData.Sid,
                userData.Surname,
                userData.Name,
                userData.Patronomyc,
                userData.Email)
          
          let registrateUser =
            _security.RegistrateUser

          let findRegistratedUser user =
            user.Email |> _security.GetUserGrid
            >> getUsers //DtoUser list
            >> List.filter (fun u -> u.Email = user.Email) 

          //user cases
          let searchExpression = "Кулаков*"
          let registratedUser = 
              searchExpression |> findDomainUser
              >> fillUserCredentials
              >> registrateUser
              >> findRegistratedUser

          Expect.isRight registratedUser "Пользователь не зарегистрирован"
        }
        //TODO: добавить тесты для проверки работы с ролями, правами, пользователями
      ]
