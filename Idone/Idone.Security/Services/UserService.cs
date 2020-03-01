namespace Idone.Security.Services
{
    using System.Linq;

    using Idone.DAL.Base;
    using Idone.DAL.Base.Extensions;
    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;
    using Idone.DAL.Entities;

    using LanguageExt;

    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using static LanguageExt.Prelude;

    using Success = Idone.DAL.Dictionaries.Success;

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
        public Either<Error, DtoRole> CreateRoles(DtoNewRole newRole)
        {
            var result = Role.Create(newRole).Bind(
                role =>
                {
                    _appContext.Roles.Add(role);
                    return _appContext.TrySaveChanges().Bind<Role>(_ => role);
                }).Bind(
                role =>
                {
                    var createdRole = new DtoRole(role.Name);
                    return Right<Error, DtoRole>(createdRole);
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

        public Either<Error, Success> SetUserRoles(DtoLinkUserRoles linkUserRoles)
        {
            var roles = linkUserRoles.RoleIds.Select(role => _appContext.Roles.Find<Role>(role));
            var result = _appContext.Users.Find<User>(linkUserRoles.UserId).Bind(
                user =>
                {
                    roles.Select(
                        role => 
                        {
                            UserRole.Create(user, role).Bind<EntityEntry<UserRole>>(link =>
                                _appContext.UserRoles.Add(link));
                            return Right<Error, Role>(role);
                        });
                    var t = _appContext.TrySaveChanges().Bind<Success>(_ => Success.ItsSuccess);
                    return t.ToOption();//TODO: костыль, конечно))
                });

            return result.ToEither(Error.Exception);//TODO: костыль, конечно))
        }

        public Either<Error, DtoGridRole> GetGridUserRoles(DtoGridQueryUserRole gridQuery)
        {
            var dbQuery = _appContext.UserRoles.AsQueryable();
            var optionFilter = gridQuery.Filter;

            optionFilter.Bind(filter => dbQuery = dbQuery.Where(userRole => userRole.User.Id == filter.Id));

            var rows = dbQuery.Paginate(gridQuery.Pagination).Select(userRole =>
                _appContext.Roles.Find(userRole.Role.Id)).Where(role => role != null).Select(role => new DtoRowRole(role.Name, role.Id));
            var result = new DtoGridRole(rows, _appContext.Users.Count());

            return Right<Error, DtoGridRole>(result);
        }

        public Either<Error, DtoGridUser> GetGridRoleUsers(DtoGridQueryRoleUser gridQuery)
        {
            var dbQuery = _appContext.UserRoles.AsQueryable();
            var optionFilter = gridQuery.Filter;
            
            optionFilter.Bind(filter => dbQuery = dbQuery.Where(userRole => userRole.Role.Id == filter.Id));

            var rows = dbQuery.Paginate(gridQuery.Pagination).Select(userRole =>
                _appContext.Users.Find(userRole.User.Id)).Where(user => user != null).Select(user => new DtoRowUser(user.Email, user.DisplayName, user.Id));
            var result = new DtoGridUser(rows, _appContext.Users.Count());

            return Right<Error, DtoGridUser>(result);
        }

        public Either<Error, DtoRole> GetRoleById(int roleId)
        {
            throw new System.NotImplementedException();
        }

        public Either<Error, DtoGridPermission> GetRolePermissions(DtoGridQueryRolePermission gridQuery)
        {
            var dbQuery = _appContext.RolePermissions.AsQueryable();
            var optionFilter = gridQuery.Filter;

            optionFilter.Bind(filter => dbQuery = dbQuery.Where(rolePermission => rolePermission.Role.Id == filter.Id));

            var rows = dbQuery.Paginate(gridQuery.Pagination).Select(rolePermission =>
                _appContext.Permissions.Find(rolePermission.Permission.Id)).Where(permission => permission != null).Select(permission => new DtoRowPermission(permission.Name, permission.Id));
            var result = new DtoGridPermission(rows, _appContext.Permissions.Count());

            return Right<Error, DtoGridPermission>(result);
        }
    }
}