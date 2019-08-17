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
        Either<Error, Success> AllowRolePermissions(DtoLinkRolePermissions linkRolePermissions);

        /// <summary>
        /// Отнять права у роли.
        /// </summary>
        /// <param name="linkRolePermissions"> DTO отнимаемых прав у роли. </param>
        /// <returns> Возвращает монаду ?.</returns>
        Either<Error, Success> DenyRolePermissions(DtoLinkRolePermissions linkRolePermissions);

        /// <summary>
        /// Получить таблицу прав для роли.
        /// </summary>
        /// <param name="gridQueryRole"> Запрос на получение табличных записей прав, для указанной роли. </param>
        /// <returns> Возвращает монаду с табличными данными прав для роли. </returns>
        Either<Error, DtoGridRole> GetGridRolePermissions(DtoGridQueryRole gridQueryRole);

        /// <summary>
        /// Получить роли, для которых назначено право.
        /// </summary>
        /// <param name="gridQueryPermission"> Запрос на получение табличных записей ролей, для которых назначено указанное право. </param>
        /// <returns> Возвращает монаду с табличными данными ролей для права. </returns>
        Either<Error, DtoGridPermission> GetGridPermissionRoles(DtoGridQueryPermission gridQueryPermission);

        /// <summary>
        /// Назначить роли для пользователя.
        /// </summary>
        /// <param name="linkUserRoles"> DTO назначаемых ролей для пользователя. </param>
        /// <returns> Возвращает монаду ?</returns>
        Either<Error, Success> SetUserRoles(DtoLinkUserRoles linkUserRoles);

        /// <summary>
        /// Отнять роли у пользователя.
        /// </summary>
        /// <param name="linkUserRoles"> DTO отнимаемых ролей у пользователя. </param>
        /// <returns> Возвращает монаду ?</returns>
        Either<Error, Success> UnsetUserRoles(DtoLinkUserRoles linkUserRoles);

        /// <summary>
        /// Получить роли.
        /// </summary>
        /// <param name="gridQueryRole"> Запро на получение табличных записей ролей. </param>
        /// <returns> Возвращает монаду с табличными данными ролей. </returns>
        Either<Error, DtoGridRole> GetGridRoles(DtoGridQueryRole gridQueryRole);

        /// <summary>
        /// Получить права.
        /// </summary>
        /// <param name="gridQuerypermission"> Запрос на получение табличных записей прав. </param>
        /// <returns> Возвращает монаду с табличными данными прав. </returns>
        Either<Error, DtoGridPermission> GetGridPermissions(DtoGridQueryPermission gridQuerypermission);

        /// <summary>
        /// Получить права пользователя.
        /// </summary>
        /// <param name="gridQueryUserPermission"> Запрос на получение табличных записей прав пользователя. </param>
        /// <returns> Возвращает монадау с табличными данными прав пользователя. </returns>
        Either<Error, DtoGridPermission> GetGridUserPermissions(DtoGridQueryUser gridQueryUserPermission);

        /// <summary>
        /// Получить роли пользователя.
        /// </summary>
        /// <param name="gridQueryUserRole"> Запрос на получение табличных записей ролей пользователя. </param>
        /// <returns> Возвращает монаду с табличными данными ролей пользователя. </returns>
        Either<Error, DtoGridRole> GetGridUserRoles
            (DtoGridQueryUserRole gridQueryUserRole);

        /// <summary>
        /// Получить пользователей роли.
        /// </summary>
        /// <param name="gridQueryRoleUser"> Запрос на получение табличных записей пользователей роли. </param>
        /// <returns> Возвращает монаду с табличными данными пользователей роли. </returns>
        Either<Error, DtoGridUser> GetGridRoleUsers(DtoGridQueryRoleUser gridQueryRoleUser);
    }
}