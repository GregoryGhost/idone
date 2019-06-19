namespace Idone.Security
{
    using System.Collections.Generic;

    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

    using LanguageExt;

    /// <summary>
    /// Модуль "Администрирования".
    /// </summary>
    public interface ISecurityModule
    {
        /// <summary>
        /// Зарегистрировать пользователя.
        /// </summary>
        /// <param name="registrateUser"> Регистрационные данные пользователя. </param>
        /// <returns> Возращает монаду с данными зарегистрированного пользователя. </returns>
        Either<Error, DtoRegistratedUser> RegistrateUser(DtoRegistrateUser registrateUser);

        /// <summary>
        /// Найти пользователя по отображаемому имени.
        /// </summary>
        /// <param name="searchExpression"> Шаблон поиска. </param>
        /// <returns> Возращает монаду с найденными совпадениями пользователей по отображаемому имени. </returns>
        /// TODO: что-то не так с этим API.
        Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression);

        /// <summary>
        /// Получить таблицу пользователей.
        /// </summary>
        /// <param name="gridQueryUser"> Запрос на получение табличных записей. </param>
        /// <returns> Возвращает монаду с табличными данными пользователей. </returns>
        Either<Error, DtoGridUser> GetGridUser(DtoGridQueryUser gridQueryUser);

        /// <summary>
        /// Создать роль.
        /// </summary>
        /// <param name="newRole"> Новая роль. </param>
        /// <returns> Возвращает монаду с данными созданной роли.</returns>
        Either<Error, DtoCreatedRole> CreateRoles(DtoNewRole newRole);

        /// <summary>
        /// Назначить права для роли.
        /// </summary>
        /// <param name="linkRolePermissions"> DTO назначаемых прав для роли. </param>
        /// <returns> Возвращает монаду ?.</returns>
        Either<Error, ?> AllowRolePermissions(DtoLinkRolePermissions linkRolePermissions);

        /// <summary>
        /// Отнять права у роли.
        /// </summary>
        /// <param name="linkRolePermissions"> DTO отнимаемых прав у роли. </param>
        /// <returns> Возвращает монаду ?.</returns>
        Either<Error, ?> DenyRolePermissions(DtoLinkRolePermissions linkRolePermissions);

        /// <summary>
        /// Получить таблицу прав для роли.
        /// </summary>
        /// <param name="gridQueryRole"> Запрос на получение табличных записей прав, для указанной роли. </param>
        /// <returns> Возвращает монаду с табличными данными прав для роли. </returns>
        Either<Error, DtoGridRole> GetRolePermissions(DtoGridQueryRole gridQueryRole);

        /// <summary>
        /// Получить роли, для которых назначено право.
        /// </summary>
        /// <param name="gridQueryPermission"> Запрос на получение табличных записей ролей, для которых назначено указанное право. </param>
        /// <returns> Возвращает монаду с табличными данными ролей для права. </returns>
        Either<Error, DtoGridPermission> GetPermissionRoles(DtoGridQueryPermission gridQueryPermission);

        /// <summary>
        /// Назначить роли для пользователя.
        /// </summary>
        /// <param name="linkUserRoles"> DTO назначаемых ролей для пользователя. </param>
        /// <returns> Возвращает монаду ?</returns>
        Either<Error, ?> SetUserRoles(DtoLinkUserRoles linkUserRoles);

        /// <summary>
        /// Отнять роли у пользователя.
        /// </summary>
        /// <param name="linkUserRoles"> DTO отнимаемых ролей у пользователя. </param>
        /// <returns> Возвращает монаду ?</returns>
        Either<Error, ?> UnsetUserRoles(DtoLinkUserRoles linkUserRoles);

        /// <summary>
        /// Получить роли.
        /// </summary>
        /// <param name="gridQueryRole"> Запро на получение табличных записей ролей. </param>
        /// <returns> Возвращает монаду с табличными данными ролей. </returns>
        Either<Error, DtoGridRole> GetRoles(DtoGridQueryRole gridQueryRole);

        /// <summary>
        /// Получить права.
        /// </summary>
        /// <param name="gridQuerypermission"> Запрос на получение табличных записей прав. </param>
        /// <returns> Возвращает монаду с табличными данными прав. </returns>
        Either<Error, DtoGridPermission> GetPermissions(DtoGridQueryPermission gridQuerypermission);

        Either<Error, DtoGridPermission> GetUserPermissions(DtoGridQueryPermission gridQueryPermission);

        /// <summary>
        /// Получить роли пользователя.
        /// </summary>
        /// <param name="gridQueryRole"> Запрос на получение табличных записей ролей пользователя. </param>
        /// <returns> Возвращает монаду с табличными данными ролей пользователя. </returns>
        Either<Error, DtoGridRole> GetUserRoles(DtoGridQueryRole gridQueryRole);

        /// <summary>
        /// Получить пользователей роли.
        /// </summary>
        /// <param name="gridQueryUser"> Запрос на получение табличных записей пользователей роли. </param>
        /// <returns> Возвращает монаду с табличными данными пользователей роли. </returns>
        Either<Error, DtoGridRole> GetRoleUsers(DtoGridQueryRole gridQueryUser);
    }
}