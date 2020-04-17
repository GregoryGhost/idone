namespace Idone.Security.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Idone.DAL.Base.Extensions;
    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;
    using Idone.DAL.Entities;

    using LanguageExt;
    using LanguageExt.UnsafeValueAccess;

    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using static LanguageExt.Prelude;

    using AppContext = Idone.DAL.Base.AppContext;
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
                    var createdRole = new DtoRole(role.Id, role.Name);
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
            var result = _appContext.Users.FindEither(linkUserRoles.UserId).Bind(
                user =>
                {
                    linkUserRoles.RoleIds.Select(role => _appContext.Roles.Find<Role>(role)).Select(
                        role => 
                        {
                            UserRole.Create(user, role).Bind<EntityEntry<UserRole>>(link =>
                                _appContext.UserRoles.Add(link));
                            return Right<Error, Role>(role);
                        });
                    var t = _appContext.TrySaveChanges().Bind<Success>(_ => Success.ItsSuccess);
                    return t;
                });

            return result;
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

        public Either<Error, DtoGridPermission> GetGridRolePermissions(DtoGridQueryRolePermission gridQuery)
        {
            var dbQuery = _appContext.RolePermissions.AsQueryable();
            var optionFilter = gridQuery.Filter;

            optionFilter.Bind(filter => dbQuery = dbQuery.Where(rolePermission => rolePermission.Role.Id == filter.Id));

            var rows = dbQuery.Paginate(gridQuery.Pagination).Select(rolePermission =>
                _appContext.Permissions.Find(rolePermission.Permission.Id)).Where(permission => permission != null).Select(permission => new DtoRowPermission(permission.Name, permission.Id));
            var result = new DtoGridPermission(rows, _appContext.Permissions.Count());

            return Right<Error, DtoGridPermission>(result);
        }

        public Either<Error, DtoGridPermission> GetGridUserPermissions(DtoGridQueryUserPermission gridQuery)
        {
            var result = gridQuery.Filter.ToEither(() => Error.Exception).Bind(filter => GetGridUserRoles(new DtoGridQueryUserRole(filter, gridQuery.Pagination))).Bind(
                    gridRoles =>
                    {
                        var (errors, permissions) = gridRoles.Rows.Select(
                            role =>
                            {
                                var filter = new DtoFilterById(role.Id);
                                var query = new DtoGridQueryRolePermission(
                                    filter,
                                    gridQuery.Pagination);
                                var perms = GetGridRolePermissions(query);

                                return perms;
                            }).Partition();

                        if (errors.Any())
                            return Left<Error, DtoGridPermission>(errors.First());//TODO: нужна конвертацию списка ошибок в одну аггрегирующую ошибку, убрать errors.First!
                        
                        var pagedPermissions = permissions.SelectMany(x => x.Rows).AsQueryable().Paginate(gridQuery.Pagination);//TODO: костыль, потому что будет все записи вытягивать из БД, а потом применять пагинацию
                        var grid = new DtoGridPermission(pagedPermissions, permissions.Count());

                        return Right<Error, DtoGridPermission>(grid);
                    });

            return result;
        }

        public Either<Error, DtoGridRole> GetGridPermissionRoles(DtoGridQueryPermissionRoles gridQuery)
        {
            var dbQuery = _appContext.RolePermissions.AsQueryable();
            var optionFilter = gridQuery.Filter;

            optionFilter.Bind(filter => dbQuery = dbQuery.Where(permissionRole => permissionRole.Permission.Id == filter.Id));

            var rows = dbQuery.Paginate(gridQuery.Pagination).Select(permissionRole =>
                _appContext.Roles.Find(permissionRole.Role.Id)).Where(role => role != null).Select(role => new DtoRowRole(role.Name, role.Id));
            var result = new DtoGridRole(rows, _appContext.Roles.Count());

            return Right<Error, DtoGridRole>(result);
        }

        public Either<Error, DtoGridPermission> GetGridPermissions(DtoGridQueryPermission gridQuery)
        {
            var dbQuery = _appContext.Permissions.AsQueryable();
            var optionFilter = gridQuery.Filter;

            optionFilter.Bind(filter => dbQuery = dbQuery.Where(permission => permission.Name.Contains(filter.Name)));

            var rows = dbQuery.Paginate(gridQuery.Pagination).Select(role => new DtoRowPermission(role.Name, role.Id));
            var result = new DtoGridPermission(rows, _appContext.Roles.Count());

            return Right<Error, DtoGridPermission>(result);
        }
        
        public Either<Error, DtoRole> GetRoleById(int roleId)
        {
            var dbQuery = _appContext.Roles.AsQueryable().FirstOrDefault(role => role.Id == roleId) ?? Left<Error, Role>(Error.NotFoundRecord);

            var result = dbQuery.Bind<DtoRole>(x => new DtoRole(x.Id, x.Name));

            return result;
        }

        public Either<Error, Success> UnsetUserRoles(DtoLinkUserRoles link)
        {
            var dbQuery = _appContext.UserRoles.AsQueryable();
            var foundUser = dbQuery.FirstOrDefault(x => x.Id == link.UserId.Id) ?? Left<Error, UserRole>(Error.NotFoundRecord);

            var result = foundUser.Bind(userRole =>
            {
                _appContext.UserRoles.Remove(userRole);
                var t = _appContext.TrySaveChanges().Bind<Success>(_ => Success.ItsSuccess);
                return t;
            });
            
            return result;
        }

        public Either<Error, DtoPermission> GetPermissionById(int permId)
        {
            var dbQuery = _appContext.Permissions.AsQueryable().FirstOrDefault(permission => permission.Id == permId) ?? Left<Error, Permission>(Error.NotFoundRecord);

            var result = dbQuery.Bind<DtoPermission>(x => new DtoPermission(x.Id, x.Name));

            return result;
        }

        public Either<Error, Success> DenyRolePermissions(DtoLinkRolePermissions link)
        {
            var dbQuery = _appContext.RolePermissions.AsQueryable();
            var foundUser = dbQuery.FirstOrDefault(x => x.Id == link.RoleId) ?? Left<Error, RolePermission>(Error.NotFoundRecord);

            var result = foundUser.Bind(rolePermission =>
            {
                _appContext.RolePermissions.Remove(rolePermission);
                var t = _appContext.TrySaveChanges().Bind<Success>(_ => Success.ItsSuccess);
                return t;
            });
            
            return result;
        }

        public Either<Error, DtoPermission> CreatePermissions(DtoNewPermission newPermission)
        {
            //TODO: опять не состыковочка в названии метода, проверить с тестами, может имеет смысл исправить интерфейс метода.
            var result = Permission.Create(newPermission).Bind(
                permission =>
                {
                    _appContext.Permissions.Add(permission);
                    return _appContext.TrySaveChanges().Bind<Permission>(_ => permission);
                }).Bind(
                permission =>
                {
                    var createdPermission = new DtoPermission(permission.Id, permission.Name);
                    return Right<Error, DtoPermission>(createdPermission);
                });

            return result;
        }

        public Either<Error, Success> AllowRolePermissions(DtoLinkRolePermissions link)
        {
            var result = _appContext.Roles.FindEither(link.RoleId).Bind(
                role =>
                {
                    link.PermissionIds.Select(permission => _appContext.Permissions.Find<Permission>(permission)).Select(
                        permission => 
                        {
                            RolePermission.Create(permission, role).Bind<EntityEntry<RolePermission>>(x => _appContext.RolePermissions.Add(x));
                            return Right<Error, Permission>(permission);
                        });
                    var t = _appContext.TrySaveChanges().Bind<Success>(_ => Success.ItsSuccess);
                    return t;
                });

            return result;
        }
    }
}