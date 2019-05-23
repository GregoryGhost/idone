namespace Idone.Security
{
    using System.Collections.Generic;

    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

    using LanguageExt;

    public interface ISecurityModule
    {
        Either<Error, DtoRegistratedUser> RegistrateUser(DtoRegistrateUser registrateUser);

        Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression);

        Either<Error, DtoGridUser> GetGridUser(DtoGridQueryUser gridQueryUser);

        //TODO:дополнить остальными методами для Ролей, Прав.
    }
}