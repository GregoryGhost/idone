namespace Idone.Tests.Helpers

open Microsoft.Extensions.DependencyInjection

open LanguageExt

open Idone.DAL.DTO
open Idone.DAL.Dictionaries

open Idone.Tests.Extensions
open Idone.Tests.Helpers.IdoneApiHelper

open Idone.Security
open LanguageExt


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

    member __.FindRegistratedUser (searchName : string) : Either<Error, DtoRowUser> =
        let gridQueryUser = searchName |> fillGridQueryUser
        gridQueryUser |> _module.GetGridUser >>= takeFirstRow
        
    member __.RegistrateUserOnDomainUser (searchExpression : string) : Either<Error, DtoRegistratedUser> =
        either {
            let! firstDomainUser = __.FindFirstDomainUser searchExpression
            return! firstDomainUser |> (fillUserCredentials >> __.RegistrateUser)
        }
        
    member __.SetRolesForUser (roles : Role list, user : DtoRegistratedUser) : Either<Error, Success> =
        either {
            let roleIds = __.GetRoleIds roles
            let linkUserRoles = new DtoLinkUserRoles(user.Id, roleIds)
            
            return! _module.SetUserRoles linkUserRoles
        }     
        
    member __.FoundUsersOfRoles (roles : Role list) : DtoGridUser list =
        roles 
        |> List.map (fun x -> either {
            let! gridRoles = x |> toDefaultGridQueryRole |> __.GetGridRoles
            let! firstRow = gridRoles |> takeFirstRow
            return! firstRow |> __.GetUsersOfRoles }) 
        |> reduceAllRights |> Seq.toList
        
    member __.GetGridRoles (gridQueryRole : DtoGridQueryRole) : Either<Error, DtoGridRole> =
        _module.GetGridRoles gridQueryRole
        
    member __.GetGridUserRoles (gridQueryUserRole : DtoGridQueryUserRole) : Either<Error, DtoGridRole> =
        _module.GetGridUserRoles gridQueryUserRole

    member __.GetUsersOfRoles (rowRole : DtoRowRole) : Either<Error, DtoGridUser> =
        rowRole 
        |> fillGridQueryRoleUser
        |> _module.GetGridRoleUsers

    member __.CreateRoles (newRoles : Role list) : DtoCreatedRole list =
        newRoles |> prepareRoleData
        |> List.fold (fun acc role -> either {
                return! (_module.CreateRoles role) :: acc}) []
        |> reduceAllRights
        |> Seq.toList

    member __.FindRole (role : Role) : Either<Error, DtoRowRole> =
        role |> toDefaultGridQueryRole |> __.GetGridRoles >>= takeFirstRow

    member __.GetRoleIds (roles : Role list) : int seq =
        ([], roles) ||> List.fold (fun acc role -> 
            either {
                let foundRole = __.FindRole role
                return! foundRole :: acc
            })
        |> reduceAllRights 
        |> Seq.fold (fun acc role -> role.Id :: acc) [] 
        |> List.toSeq