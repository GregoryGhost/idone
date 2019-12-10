﻿namespace Idone.Tests.Helpers

module IdoneApiHelper =
    open Idone.DAL.DTO
    open Idone.DAL.Dictionaries
    open Idone.Tests.Constants
    open Idone.Tests.Extensions
    open Idone.Tests.Types

    open LanguageExt

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
        List.fold (fun acc link -> link.Role :: acc) [] links

    let getPerms (links : PermRoleLink list) : Perm list = 
        List.fold (fun acc link -> link.Perm :: acc) [] links

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
        
    let toRoleDtos (roles : Role list) : DtoNewRole list =
        roles |> List.map (fun role -> new DtoNewRole(role.Name))
        
    let toDefaultGridQueryRole (role : Role) : DtoGridQueryRole =
        let filter = new DtoRoleFilter(role.Name)
        let query = new DtoGridQueryRole(filter, FIRST_PAGE)
        query

    let toEitherDefault x = toEither x Error.Exception

    let takeFirst (rows : 'a seq) : Either<Error, 'a> = 
        rows |> Seq.cast<'a> |> Seq.head |> LanguageExt.Prelude.Right<Error, 'a>

    let takeFirstRow (row : DtoGrid<'b>) : Either<Error, 'b> =
        row.Rows |> takeFirst