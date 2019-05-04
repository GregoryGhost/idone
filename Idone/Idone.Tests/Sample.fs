module Tests

open Expecto
open Idone.Security
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
    //TODO: добавить функции для Expect - Expect.isRight, Expect.isLeft
    //  для того чтобы можно было работать с монадой Either
    //TODO: добавить тесты для проверки работы с ролями, правами, пользователями
  ]
