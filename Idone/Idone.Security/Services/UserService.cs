namespace Idone.Security.Services
{
    using System.Linq;

    using Idone.DAL.Base;
    using Idone.DAL.Base.Extensions;
    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;
    using Idone.DAL.Entities;

    using LanguageExt;

    using static LanguageExt.Prelude;

    /// <summary>
    /// Сервис по работе с пользователями.
    /// </summary>
    internal class UserService
    {
        /// <summary>
        /// Контекст БД.
        /// </summary>
        public readonly AppContext _appContext;

        /// <summary>
        /// Инициализировать зависимости.
        /// </summary>
        /// <param name="appContext"> Контекст БД. </param>
        public UserService(AppContext appContext)
        {
            _appContext = appContext;
        }

        /// <summary>
        /// Получить табличные данные пользователей.
        /// </summary>
        /// <param name="gridQuery"> Запрос на формирование табличных записей. </param>
        /// <returns> Возвращает монаду табличных записей пользователей. </returns>
        public Either<Error, DtoGridUser> GetGridUser(DtoGridQueryUser gridQuery)
        {
            var dbQuery = _appContext.Users.AsQueryable();
            var filter = gridQuery.Filter;

            if (!string.IsNullOrWhiteSpace(filter.Email))
                dbQuery = dbQuery.Where(user => user.Email == filter.Email);

            var rows = dbQuery.Paginate(gridQuery.Pagination).Select(user => new DtoRowUser(user.Email, user.DisplayName));
            var result = new DtoGridUser(rows, _appContext.Users.Count());

            return Right<Error, DtoGridUser>(result);
        }

        /// <summary>
        /// Зарегистрировать пользователя.
        /// </summary>
        /// <param name="registrateUser"> Регистрационные данные пользователя. </param>
        /// <returns> Возвращает монаду зарегистрированного пользователя. </returns>
        public Either<Error, DtoRegistratedUser> RegistrateUser(DtoRegistrateUser registrateUser)
        {
            //TODO: выглядит как-то не очень,
            // выделить Add у DbSet'а в метод расширения AddEither :: TEntity -> Either<Error, TEntity>
            var result = User.Create(registrateUser).Bind(
                user =>
                {
                    _appContext.Users.Add(user);
                    return _appContext.TrySaveChanges().Bind<User>(_ => user);
                }).Bind(
                user =>
                {
                    var registratedUser = new DtoRegistratedUser(user.Email);
                    return Right<Error, DtoRegistratedUser>(registratedUser);
                });

            return result;
        }
    }
}