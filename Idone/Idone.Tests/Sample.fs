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


    let (>>=) (x : Either<'L,'R>) (f: ('R -> Either<'L, 'B>)) =
        x.Bind(f)


    [<Tests>]
    let tests =
      let _servicesProvider = initTestEnviroment()
      let _security = new EnterPoint(_servicesProvider) :> ISecurityModule

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
          clearUsers() |> ignore
        }
        //TODO: добавить тесты для проверки работы с ролями, правами, пользователями
      ]
