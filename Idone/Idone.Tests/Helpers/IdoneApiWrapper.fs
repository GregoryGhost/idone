namespace Idone.Tests.Helpers

open Microsoft.Extensions.DependencyInjection

open LanguageExt

open Idone.DAL.DTO
open Idone.DAL.Dictionaries

open Idone.Tests.Extensions
open Idone.Tests.Helpers.IdoneApiHelper

open Idone.Security


type SecurityModuleWrapper(servicesProvider : ServiceProvider) =
    let _module = new EnterPoint(servicesProvider) :> ISecurityModule
  
    
    member __.RegistrateUser = 
        _module.RegistrateUser

    member __.FindUsersByDisplayName =
        _module.FindUsersByDisplayName

    member __.GetGridUser =
        _module.GetGridUser

    member __.FindFirstDomainUser (searchExpression : string) : Either<Error, DtoAdUser> =
        let takeFirstUser = Seq.cast<DtoAdUser> >> Seq.head >> LanguageExt.Prelude.Right<Error, DtoAdUser>
        searchExpression |> _module.FindUsersByDisplayName
        >>= takeFirstUser

    member __.FindRegistratedUser (user : DtoRegistratedUser) : Either<Error, DtoRowUser> =
        let convert x = toEither x Error.Exception
        let findUser = getUsers >> Seq.tryFind (fun u -> u.Email = user.Email) >> convert
        let gridQueryUser = new DtoUserFilter(user.Email) |> fillGridQueryUser
        gridQueryUser |> _module.GetGridUser >>= findUser
