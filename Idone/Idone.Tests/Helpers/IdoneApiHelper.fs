namespace Idone.Tests.Helpers

module IdoneApiHelper =
    open Idone.DAL.DTO
    
    open LanguageExt


    let FIRST_PAGE = new Pagination(10, 1)
    
    let ADMIN_AND_USER_ROLES =
        [
            { Name = "админ" }
            { Name = "пользователь" }
        ]
        
    let SEARCH_DEFAULT_USER = "Кулаков*"


    let fillUserCredentials (userData : DtoAdUser) : DtoRegistrateUser =
        new DtoRegistrateUser(
            userData.Sid,
            userData.Surname,
            userData.Name,
            userData.Patronomic,
            userData.Email)

    let getUsers (gridUser : DtoGridUser) : DtoRowUser seq = 
        gridUser.Rows

    let fillGridQueryUser (user : DtoUserFilter) : DtoGridQueryUser =
        let filter = new DtoUserFilter(user.Email)
        let query = new DtoGridQueryUser(filter, FIRST_PAGE)
        query
        
    let flattenDuplicates (records : Record<'a> seq) : Record<'a> option =
        records
        |> Seq.distinct
        |> Seq.tryExactlyOne
        
    let prepareRoleData (roles : Role list) : DtoNewRole list =
        roles |> List.map (fun role -> new DtoNewRole(role.Name))
        
    let getUsersOfRoles (roles : DtoGridRole) : Either<Error, DtoRowUser> =
        roles 
        |> Seq.map getUsersByRole
        |> flattenDuplicate
    
    let toDefaultGridQuery (user : DtoRegistratedUser) : DtoGridQueryUser =
        let filter = new DtoUserFilter(user.Email)
        let query = new DtoGridQueryUser(filter, FIRST_PAGE)
        query
        
    let toDtos (roles : Role list) : DtoNewRole list =
        roles |> List.map (fun role -> new DtoNewRole(role.Name))