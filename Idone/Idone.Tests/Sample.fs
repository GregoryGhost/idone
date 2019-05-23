namespace Idone.Tests

module Tests = 

    open Expecto
    open Idone.Security
    open Idone.Tests.Extensions
    open LanguageExt

    let private _security = new EnterPoint() :> ISecurityModule

    let (>>=) (x : Either<'L,'R>) (f: ('R -> Either<'L, 'B>)) =
        x.Bind(f)


    [<Tests>]
    let tests =
      testList "Модуль админки" [       
        test "Регистрация нового пользователя" {
            //1.Найти пользователя в домене (берем хардкод данные)
            //2.Получить данные требуемого пользователя у AD сервера
            //3.Передать данные пользователя на регистрацию в системе
            //4.Найти зареганного пользователя в системе (в гриде всех пользователей)
            
          let findFirstDomainUser searchExpression =
            let takeFirstUser = Seq.cast<DtoAdUser> >> Seq.head >> LanguageExt.Prelude.Right<Error, DtoAdUser>
            searchExpression |> _security.FindUsersByDisplayName
            >>= takeFirstUser

          let fillUserCredentials (userData : DtoAdUser) : DtoRegistrateUser =
            new DtoRegistrateUser(
                userData.Sid,
                userData.Surname,
                userData.Name,
                userData.Patronomic,
                userData.Email)
          
          let registrateUser =
            _security.RegistrateUser

          let getUsers (gridUser : DtoGridUser) : DtoRowUser seq = 
            gridUser.Rows

          let FIRST_PAGE = new Pagination(10, 1)

          let fillGridQueryUser (user : DtoUserFilter) : DtoGridQueryUser =
            let filter = new DtoUserFilter(user.Email)
            let query = new DtoGridQueryUser(filter, FIRST_PAGE)
            query

          let toEither (x : 'R option) (error : 'L) : Either<'L, 'R> =
            match x with
            | Some y -> LanguageExt.Prelude.Right<'L, 'R>(y)
            | None -> LanguageExt.Prelude.Left<'L, 'R>(error)
          let findRegistratedUser (user : DtoRegistratedUser) : Either<Error, DtoRowUser> =
            let convert x = toEither x Error.Exception
            let findUser = getUsers >> Seq.tryFind (fun u -> u.Email = user.Email) >> convert
            let gridQueryUser = new DtoUserFilter(user.Email) |> fillGridQueryUser
            gridQueryUser |> _security.GetGridUser >>= findUser

          //user cases
          let searchExpression = "Кулаков*"
          let registratedUser : Either<Error, DtoRowUser> = 
            searchExpression 
            |> findFirstDomainUser 
            >>= (fillUserCredentials >> registrateUser) 
            >>= findRegistratedUser

          Expect.isRight registratedUser "Пользователь не зарегистрирован"
        }
        //TODO: добавить тесты для проверки работы с ролями, правами, пользователями
      ]
