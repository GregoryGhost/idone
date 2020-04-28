namespace Idone.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;
    using Idone.Security.Services;

    using LanguageExt;

    using Microsoft.Extensions.DependencyInjection;

    using static LanguageExt.Prelude;

    /// <summary>
    /// Точка входа модуля "Администрирование".
    /// </summary>
    public sealed class EnterPoint : ISecurityModule
    {
        /// <summary>
        /// Сервис по работе с пользователями.
        /// </summary>
        private readonly UserService _userService;

        /// <summary>
        /// Сервис по работе с AD-пользователя.
        /// </summary>
        private readonly AdService _adService;

        /// <summary>
        /// Конструктор с инициализацией зависимостей модуля.
        /// </summary>
        /// <param name="serviceProvider"> Коллекция сервисов. </param>
        public EnterPoint(IServiceProvider serviceProvider)
        {
            _userService = serviceProvider.GetService<UserService>();
            _adService = serviceProvider.GetService<AdService>();
        }

        /// <inheritdoc />
        public Either<Error, Success> AllowRolePermissions(DtoLinkRolePermissions linkRolePermissions)
        {
            var result = _userService.AllowRolePermissions(linkRolePermissions);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoPermission> CreatePermissions(DtoNewPermission newPermission)
        {
            var result = _userService.CreatePermissions(newPermission);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DAL.DTO.DtoRole> CreateRole(DtoNewRole newRole)
        {
            //TODO: не стыковка API - в описании модуля создание одной роли, в методе юзер сервиса написано создание ролей, но создает все равно одну роль.
            var result = _userService.CreateRoles(newRole);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, Success> DenyRolePermissions(DtoLinkRolePermissions linkRolePermissions)
        {
            var result = _userService.DenyRolePermissions(linkRolePermissions);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression)
        {
            var result = _adService.FindUsersByDisplayName(searchExpression);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoGridRole> GetGridPermissionRoles(DtoGridQueryPermissionRoles gridQueryPermission)
        {
            var result = _userService.GetGridPermissionRoles(gridQueryPermission);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoGridPermission> GetGridPermissions(DtoGridQueryPermission gridQueryPermission)
        {
            var result = _userService.GetGridPermissions(gridQueryPermission);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoGridPermission> GetGridRolePermissions(DtoGridQueryRolePermission gridQueryRole)
        {
            var result = _userService.GetGridRolePermissions(gridQueryRole);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoGridRole> GetGridRoles(DtoGridQueryRole gridQueryRole)
        {
            var result = _userService.GetGridRole(gridQueryRole);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoGridUser> GetGridRoleUsers(DtoGridQueryRoleUser gridQueryRoleUser)
        {
            var result = _userService.GetGridRoleUsers(gridQueryRoleUser);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoRole> GetRoleById(int roleId)
        {
            var result = _userService.GetRoleById(roleId);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoPermission> GetPermissionById(int permId)
        {
            var result = _userService.GetPermissionById(permId);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoGridUser> GetGridUser(DtoGridQueryUser gridQueryUser)
        {
            var result = _userService.GetGridUser(gridQueryUser);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoGridPermission> GetGridUserPermissions(DtoGridQueryUserPermission gridQueryUserPermission)
        {
            var result = _userService.GetGridUserPermissions(gridQueryUserPermission);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoGridRole> GetGridUserRoles(DtoGridQueryUserRole gridQueryUserRole)
        {
            var result = _userService.GetGridUserRoles(gridQueryUserRole);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoRegistratedUser> RegistrateUser(DtoRegistrateUser registrateUser)
        {
            var result = _userService.RegistrateUser(registrateUser);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, Success> SetUserRoles(DtoLinkUserRoles linkUserRoles)
        {
            var result = _userService.SetUserRoles(linkUserRoles);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, Success> UnsetUserRoles(DtoLinkUserRoles linkUserRoles)
        {
            var result = _userService.UnsetUserRoles(linkUserRoles);

            return result;
        }
    }
}