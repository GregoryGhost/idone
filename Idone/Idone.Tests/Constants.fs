namespace Idone.Tests.Constants

open Idone.DAL.DTO
open Idone.Tests.Types

[<AutoOpen>]
module Constants =
    let FIRST_PAGE = new Pagination(10, 1)
    
    let ADMIN_AND_USER_ROLES : Role list =
        [
            { Name = "админ" }
            { Name = "пользователь" }
        ]

    let PERMISSIONS : Perm list = 
        [
            { Name = "" }
        ]

    let PERMS_ROLES_LINKS : PermRoleLink list = [
        {
            Role = { Name = "админ" }
            Perm = { Name = "разрешение1" }
        };
        {
            Role = { Name = "пользователь" }
            Perm = { Name = "разрешение2" }
        }
    ]
        
    let SEARCH_DEFAULT_USER = "Кулаков*"

    let SEARCH_NAME_USER = "Кулаков"