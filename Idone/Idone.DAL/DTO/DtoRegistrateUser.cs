namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO с регистрационными данными пользователя.
    /// </summary>
    public class DtoRegistrateUser : Record<DtoRegistrateUser>
    {
        /// <summary>
        /// Контруктор по умолчанию.
        /// </summary>
        /// <param name="sid"> Идентификатор безопасности пользователя в Active Directory. </param>
        /// <param name="surname"> Фамилия. </param>
        /// <param name="name"> Имя. </param>
        /// <param name="patronomic"> Отчество. </param>
        /// <param name="email"> Почта. </param>
        public DtoRegistrateUser(
            string sid,
            string surname,
            string name,
            string patronomic,
            string email)
        {
            Sid = sid;
            Surname = surname;
            Name = name;
            Patronomic = patronomic;
            Email = email;
        }

        /// <summary>
        /// Почта.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronomic { get; }

        /// <summary>
        /// Идентификатор безопасности пользователя в Active Directory.
        /// </summary>
        public string Sid { get; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Surname { get; }
    }
}