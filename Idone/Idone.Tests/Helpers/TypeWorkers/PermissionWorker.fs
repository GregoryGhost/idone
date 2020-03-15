namespace Idone.Tests.Helpers.TypeWorkers

open Idone.DAL.DTO
open Idone.DAL.Dictionaries

open Idone.Tests.Types
open Idone.Tests.Extensions
open Idone.Tests.Helpers.IdoneApiHelper

open Idone.Security

open LanguageExt

type PermissionWorker(secModule : ISecurityModule) =
    inherit BaseTypeWorker<
        Perm,
        DtoRowPermission,
        DtoRowRole,
        DtoGridPermission,
        DtoGridRole,
        DtoPermissionFilter,
        DtoGridQueryPermission>()
       
    let _module = secModule
    
    override __.GetGridDepType (roleId: #IIdentity) : Either<Error, DtoGridRole> =
        let filter = toDefaultGridQueryPermRole roleId
        let grid = _module.GetGridPermissionRoles filter
        grid
        
    override __.GetGridEntities (query : DtoGridQueryPermission) : Either<Error, DtoGridPermission> =
        _module.GetGridPermissions query
        
    override __.ToGridQueryType  (perm : Perm) (pagination : Pagination): DtoGridQueryPermission =
        let filter = new DtoPermissionFilter(perm.Name)
        let query = new DtoGridQueryPermission(filter, pagination)
        query