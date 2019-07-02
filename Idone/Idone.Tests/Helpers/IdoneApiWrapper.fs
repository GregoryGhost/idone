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
        _module.RegistrateUser registrateUser

    member __.FindUsersByDisplayName (searchExpression : string) : Either<Error, DtoAdUser seq> =
        _module.FindUsersByDisplayName searchExpression

    member __.GetGridUser (gridQueryUser : DtoGridQueryUser) : Either<Error, DtoGridUser> =
        _module.GetGridUser gridQueryUser

    member __.FindFirstDomainUser (searchExpression : string) : Either<Error, DtoAdUser> =
        let takeFirstUser = takeFirst<DtoAdUser>
        searchExpression |> __.FindUsersByDisplayName
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
        either {
            let findRoleId role =
                role |> toDefaultGridQueryRole |> __.GetGridRoles >>= (fun x -> x.Rows |> Seq.head)
            let roleIds =
                roles |> List.reduce (fun acc role -> either {
                        let! foundRole = findRoleId role
                        acc + foundRole.Id })
            let! foundUser = user |> __.GetGridUser >>= List.head 
            let linkUserRoles = new DtoLinkUserRoles(roleIds, foundUser.Id)
            
            return! __.SetUserRoles linkUserRoles
        }     
        
    member __.FoundUsersOfRoles (roles : Role list) : Either<Error, DtoGridUser> =
        roles |> toRoleDtos |> __.GetGridRoles >>= __.GetUsersOfRoles
        
    member __.GetGridRoles (gridQueryRole : DtoGridQueryRole) : Either<Error, DtoGridRole> =
        _module.GetGridRoles gridQueryRole
        
    member __.GetGridUserRoles (gridQueryRoleUser : DtoGridQueryUser) : Either<Error, DtoGridRole> =
        _module.GetGridRoleUsers gridQueryRoleUser

    member __.GetUsersOfRoles (roles : DtoGridRole) : Either<Error, DtoGridRole> =
        roles
        |> Seq.map __.GetGridUserRoles
        |> flattenDuplicates