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
        Either<Error, IEnumerable<DtoAdUser>> FindUsersByDisplayName(string searchExpression);

        /// <summary>
        /// Получить таблицу пользователей.
        /// </summary>
        /// <param name="gridQueryUser"> Запрос на получение табличных записей. </param>
        /// <returns></returns>
        Either<Error, DtoGridUser> GetGridUser(DtoGridQueryUser gridQueryUser);

        //TODO:дополнить остальными методами для Ролей, Прав.
    }
}