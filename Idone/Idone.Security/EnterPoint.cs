﻿namespace Idone.Security
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
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
        /// Конструктор с инициализацией зависимостей модуля.
        /// </summary>
        /// <param name="serviceProvider"> Коллекция сервисов. </param>
        public EnterPoint(IServiceProvider serviceProvider)
        {
            _userService = serviceProvider.GetService<UserService>();
        }

        /// <inheritdoc />
        public Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression)
        {
            //TODO: выделить в метод сервиса AdService
            var ctx = new PrincipalContext(ContextType.Domain, "tomskasu");
            var query = new UserPrincipal(ctx)
            {
                DisplayName = searchExpression
            };

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

        /// <inheritdoc />
        public Either<Error, DtoGridUser> GetGridUser(DtoGridQueryUser gridQueryUser)
        {
            var result = _userService.GetGridUser(gridQueryUser);

            return result;
        }

        /// <inheritdoc />
        public Either<Error, DtoCreatedRole> CreateRoles(DtoNewRole newRole)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, Success> AllowRolePermissions(DtoLinkRolePermissions linkRolePermissions)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, Success> DenyRolePermissions(DtoLinkRolePermissions linkRolePermissions)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, DtoGridRole> GetRolePermissions(DtoGridQueryRole gridQueryRole)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, DtoGridPermission> GetPermissionRoles(DtoGridQueryPermission gridQueryPermission)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, Success> SetUserRoles(DtoLinkUserRoles linkUserRoles)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, Success> UnsetUserRoles(DtoLinkUserRoles linkUserRoles)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, DtoGridRole> GetRoles(DtoGridQueryRole gridQueryRole)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, DtoGridPermission> GetPermissions(DtoGridQueryPermission gridQuerypermission)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, DtoGridPermission> GetUserPermissions(DtoGridQueryUser gridQueryUserPermission)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, DtoGridRole> GetUserRoles(DtoGridQueryUser gridQueryUserRole)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, DtoGridRole> GetRoleUsers(DtoGridQueryRole gridQueryRoleUser)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Either<Error, DtoRegistratedUser> RegistrateUser(DtoRegistrateUser registrateUser)
        {
            var result = _userService.RegistrateUser(registrateUser);

            return result;
        }
    }
}