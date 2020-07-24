namespace Idone.DAL.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

    using LanguageExt;

    using Microsoft.AspNetCore.Identity;

    using static LanguageExt.Prelude;

    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User : IdentityUser<int>, IIdentity
    {
        /// <summary>
        /// Конструктор для EF Core.
        /// </summary>
        private User()
        {
        }

        /// <summary>
        /// Отображаемое имя пользователя (сокращенное).
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Полное имя пользователя.
        /// </summary>
        public FullName FullName { get; private set; }

        /// <summary>
        /// Роли пользователя.
        /// </summary>
        public IEnumerable<UserRole> UserRoles { get; private set; }

        /// <summary>
        /// Создать пользователя.
        /// </summary>
        /// <param name="registrateUser"> DTO с регистрационными данными пользователя. </param>
        /// <returns> Возвращает результат создания. </returns>
        public static Either<Error, User> Create(DtoRegistrateUser registrateUser)
        {
            var user = new User
            {
                DisplayName = GetDisplayName(registrateUser),
                Email = registrateUser.Email,
                UserName = registrateUser.Email,
                FullName = new FullName(registrateUser.Surname, registrateUser.Name, registrateUser.Patronomic)
            };

            return Right<Error, User>(user);
        }

        /// <summary>
        /// Сравнение пользователей.
        /// </summary>
        /// <param name="x"> Первый пользователь. </param>
        /// <param name="y"> Второй пользователь. </param>
        /// <returns> Возвращает результат сравнения. </returns>
        public static bool operator ==(User x, User y)
        {
            return RecordType<User>.EqualityTyped(x, y);
        }

        /// <summary>
        /// Сравнение пользователей.
        /// </summary>
        /// <param name="x"> Первый пользователь. </param>
        /// <param name="y"> Второй пользователь. </param>
        /// <returns> Возвращает результат сравнения. </returns>
        public static bool operator !=(User x, User y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Сформировать отображаемое имя пользователя.
        /// </summary>
        /// <param name="registrateUser"> Регистрационные данные. </param>
        /// <returns> Возвращает отображаемое имя пользователя. </returns>
        private static string GetDisplayName(DtoRegistrateUser registrateUser)
        {
            var patronomic = string.IsNullOrEmpty(registrateUser.Patronomic)
                ? string.Empty
                : $"{registrateUser.Patronomic.First()}.";
            return $"{registrateUser?.Surname} {registrateUser?.Name.First()}. {patronomic}";
        }
    }
}