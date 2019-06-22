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
    
    member __.RegistrateUser (registrateUser : DtoRegistrateUser) : Either<Error, DtoRegistratedUser> = 
        _module.RegistrateUser

    member __.FindUsersByDisplayName (searchExpression : string) : Either<Error, DtoAdUser seq> =
        _module.FindUsersByDisplayName

    member __.GetGridUser (DtoGridQueryUser gridQueryUser) : Either<Error, DtoGridUser> =
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
        
    member __.RegistrateUserOnDomainUser (searchExpression : string) : Either<Error, DtoRegistratedUser> =
        either {
            let! firstDomainUser = __.FindFirstDomainUser searchExpression
            return! firstDomainUser |> (fillUserCredentials >> __.RegistrateUser)
        }
        
    member __.SetRolesForUser (roles : Role list, user : DtoRegistratedUser) : Either<Error, Success> =
        //TODO: реализовать
        
    member __.FoundUsersOfRoles (roles : Role list) : Either<Error, DtoGridUser> =
        roles |> __.GetGridRole >>= __.GetUsersOfRoles
        
    member __.GetGridRole () : =
    
    member __.GetUsersOfRoles () : =