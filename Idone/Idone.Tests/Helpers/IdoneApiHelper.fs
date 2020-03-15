namespace Idone.Tests.Helpers
open Idone.DAL.Base

module IdoneApiHelper =
    open Idone.DAL.DTO
    open Idone.DAL.Dictionaries
    open Idone.Tests.Constants
    open Idone.Tests.Extensions
    open Idone.Tests.Types

    open LanguageExt
    open Idone.Security

    let fillUserCredentials (userData : DtoAdUser) : DtoRegistrateUser =
        new DtoRegistrateUser(
            userData.Sid,
            userData.Surname,
            userData.Name,
            userData.Patronomic,
            userData.Email)

    let getUsers (gridUser : DtoGridUser) : DtoRowUser seq = 
        gridUser.Rows

    let getRoles (links : PermRoleLink list) : Role list = 
        List.map (fun link -> link.Role) links

    let getPerms (links : PermRoleLink list) : Perm list = 
        List.map (fun link -> link.Perm) links

    let fillGridQueryUser (searchName : string) : DtoGridQueryUser =
        let filter = new DtoUserFilter(searchName)
        let query = new DtoGridQueryUser(filter, FIRST_PAGE)
        query

    let fillGridQueryUserRole (registratedUser : DtoRegistratedUser) : DtoGridQueryUserRole =
        let filter = new DtoFilterById(registratedUser.Id)
        let query = new DtoGridQueryUserRole(filter, FIRST_PAGE)
        query    
        
    let fillGridQueryRoleUser (rowRole : DtoRowRole) : DtoGridQueryRoleUser =
        let filter = new DtoFilterById(rowRole.Id)
        let query = new DtoGridQueryRoleUser(filter, FIRST_PAGE)
        query
        
    let flattenDuplicates (records : Record<'a> seq) : Record<'a> option =
        records
        |> Seq.distinct
        |> Seq.tryExactlyOne
        
    let prepareRoleData (roles : Role list) : DtoNewRole list =
        roles |> List.map (fun role -> new DtoNewRole(role.Name))
       
    let preparePermData (perms : Perm list) : DtoNewPermission list =
        perms |> List.map (fun perm -> new DtoNewPermission(perm.Name, perm.Description))
        
    let toDefaultGridQueryRolePerm (filter : #IIdentity) : DtoGridQueryRolePermission =
        let query = new DtoGridQueryRolePermission(filter, FIRST_PAGE)
        query
        
    let toDefaultGridQueryPermRole (filter : #IIdentity) : DtoGridQueryPermissionRoles =
        let query = new DtoGridQueryPermissionRoles(filter, FIRST_PAGE)
        query
        
    let inline bindData
                 (entity1 : #IIdentity list)
                 (entity2 : #IIdentity list)
                 : DtoLinkRolePermissions list =
         (entity1, entity2) 
         ||> List.map2 (fun e1 e2 ->
             new DtoLinkRolePermissions(e1.Id, [e2.Id] |> List.toSeq))

    let toRoleDtos (roles : Role list) : DtoNewRole list =
        roles |> List.map (fun role -> new DtoNewRole(role.Name))
       
    let toEitherDefault x = toEither x Error.Exception

    let takeFirst (rows : 'a seq) : Either<Error, 'a> = 
        rows |> Seq.cast<'a> |> Seq.head |> LanguageExt.Prelude.Right<Error, 'a>

    let takeFirstRow (row : DtoGrid<'b>) : Either<Error, 'b> =
        row.Rows |> takeFirst