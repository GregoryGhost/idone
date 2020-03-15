namespace Idone.Tests.Helpers.TypeWorkers

open Idone.DAL.DTO
open Idone.DAL.Dictionaries

open Idone.Tests.Types
open Idone.Tests.Extensions
open Idone.Tests.Helpers.IdoneApiHelper

open Idone.Security
open LanguageExt

type RoleWorker(secModule : ISecurityModule) =
    inherit BaseTypeWorker<
        Role,
        DtoRowRole,
        DtoRowPermission,
        DtoGridRole,
        DtoGridPermission,
        DtoRoleFilter,
        DtoGridQueryRole>()
    
    let _module = secModule
    
    override __.GetGridDepType (permId: #IIdentity) : Either<Error, DtoGridPermission> =
        let filter = toDefaultGridQueryRolePerm permId
        let grid = _module.GetGridRolePermissions filter
        grid
     
    override __.GetGridEntities (query : DtoGridQueryRole) : Either<Error, DtoGridRole> =
        _module.GetGridRoles query
        
    override __.ToGridQueryType  (role : Role) (pagination : Pagination): DtoGridQueryRole =
        let filter = new DtoRoleFilter(role.Name)
        let query = new DtoGridQueryRole(filter, pagination)
        query