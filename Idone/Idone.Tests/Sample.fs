namespace Idone.Tests

module Tests = 

    open Expecto
    open Idone.Security
    open Idone.Tests.Extensions
    open LanguageExt

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
        //TODO: добавить тесты для проверки работы с ролями, правами, пользователями
      ]
