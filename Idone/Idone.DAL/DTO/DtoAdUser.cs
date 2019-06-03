namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO пользователя из Active Directory.
    /// </summary>
    public class DtoAdUser : Record<DtoAdUser>
    {
        /// <summary>
        /// Контруктор по умолчанию.
        /// </summary>
        /// <param name="sid"> SID пользователя. </param>
        /// <param name="surname"> Фамилия пользователя. </param>
        /// <param name="name"> Имя пользователя. </param>
        /// <param name="patronomic"> Отчество пользователя. </param>
        /// <param name="email"> Почта пользователя. </param>
        public DtoAdUser(
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
        /// Электронная почта.
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
        /// Идентификатор безопасности пользователя.
        /// </summary>
        public string Sid { get; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Surname { get; }
    }
}