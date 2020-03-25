namespace Idone.Security.Services
{
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;

    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

    using LanguageExt;

    using static LanguageExt.Prelude;

    using Success = Idone.DAL.Dictionaries.Success;

    /// <summary>
    /// Сервис по работе с AD-пользователями.
    /// </summary>
    internal class AdService
    {
        private const string DOMAIN = "tomskasu";

        /// <summary>
        /// Найти пользователей по отображаемому имени.
        /// </summary>
        /// <param name="searchExpression"> Шаблон поиска. </param>
        /// <returns> Возращает монаду с найденными совпадениями пользователей по отображаемому имени. </returns>
        public Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression)
        {
            //TODO: вынести домен в настройки
            using (var ctx = new PrincipalContext(ContextType.Domain, DOMAIN))
            using (var query = new UserPrincipal(ctx)
            {
                DisplayName = searchExpression
            })
            {
                var foundUsers = new PrincipalSearcher(query).FindAll().Where(user => user.DisplayName != null).Select(
                    userData =>
                    {
                        var user = userData as UserPrincipal;
                        return new DtoAdUser(
                            user?.Sid.Value,
                            user?.Surname,
                            user?.GivenName,
                            user?.MiddleName ?? string.Empty,
                            user?.EmailAddress);
                    });

                return Right<Error, IEnumerable<DtoAdUser>>(foundUsers);
            }
        }

        /// <summary>
        /// Создать AD-пользователя.
        /// </summary>
        /// <param name="newUser">Данные нового пользователя.</param>
        /// <returns>Результат операции.</returns>
        public Either<Error, Success> CreateUser(DtoNewAdUser newUser)
        {
            using (var ctx = new PrincipalContext(ContextType.Domain, DOMAIN))
            using (var query = new UserPrincipal(ctx))
            {
                query.SamAccountName = newUser.Nickname;
                query.EmailAddress = newUser.Email;
                query.SetPassword(newUser.Password);
                query.DisplayName = $"{newUser.Surname} {newUser.Name} {newUser.Patronomic}";
                query.GivenName = newUser.Name;
                query.Surname = newUser.Surname;
                query.MiddleName = newUser.Patronomic;
                query.Enabled = true;
                query.ExpirePasswordNow();
                query.Save();
            }

            return Right<Error, Success>(Success.ItsSuccess);
        }
    }
}