namespace Idone.Tests.Helpers

module IdoneApiHelper =
    open Idone.DAL.DTO


    let FIRST_PAGE = new Pagination(10, 1)


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