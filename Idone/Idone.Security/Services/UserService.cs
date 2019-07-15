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
            var optionFilter = gridQuery.Filter;
            
            optionFilter.Bind(filter => dbQuery = dbQuery.Where(user => user.DisplayName.Contains(filter.SearchName)));

            var rows = dbQuery.Paginate(gridQuery.Pagination).Select(user => new DtoRowUser(user.Email, user.DisplayName, user.Id));
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
                    var registratedUser = new DtoRegistratedUser(user.Id);
                    return Right<Error, DtoRegistratedUser>(registratedUser);
                });

            return result;
        }

        /// <summary>
        /// Создать роль.
        /// </summary>
        /// <param name="newRole"> DTO с данными для создания роли. </param>
        /// <returns> Возвращает монаду созданной роли. </returns>
        public Either<Error, DtoCreatedRole> CreateRoles(DtoNewRole newRole)
        {
            var result = Role.Create(newRole).Bind(
                role =>
                {
                    _appContext.Roles.Add(role);
                    return _appContext.TrySaveChanges().Bind<Role>(_ => role);
                }).Bind(
                role =>
                {
                    var createdRole = new DtoCreatedRole(role.Name);
                    return Right<Error, DtoCreatedRole>(createdRole);
                });

            return result;
        }

        /// <summary>
        /// Получить табличные данные ролей.
        /// </summary>
        /// <param name="gridQuery"> Запрос на формирование табличных записей. </param>
        /// <returns> Возвращает монаду табличных записей ролей. </returns>
        public Either<Error, DtoGridRole> GetGridRole(DtoGridQueryRole gridQuery)
        {
            var dbQuery = _appContext.Roles.AsQueryable();
            var optionFilter = gridQuery.Filter;

            optionFilter.Bind(filter => dbQuery = dbQuery.Where(role => role.Name.Contains(filter.Name)));

            var rows = dbQuery.Paginate(gridQuery.Pagination).Select(role => new DtoRowRole(role.Name, role.Id));
            var result = new DtoGridRole(rows, _appContext.Users.Count());

            return Right<Error, DtoGridRole>(result);
        }
    }
}