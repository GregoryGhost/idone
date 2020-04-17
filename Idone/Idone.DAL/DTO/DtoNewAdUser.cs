namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO нового AD-пользователя.
    /// </summary>
    public class DtoNewAdUser : Record<DtoNewAdUser>
    {
        /// <summary>
        /// Контруктор по умолчанию.
        /// </summary>
        /// <param name="surname"> Фамилия пользователя. </param>
        /// <param name="name"> Имя пользователя. </param>
        /// <param name="patronomic"> Отчество пользователя. </param>
        /// <param name="email"> Почта пользователя. </param>
        /// <param name="nickname">Nickname.</param>
        /// <param name="password">Пароль.</param>
        public DtoNewAdUser(string surname,
            string name,
            string patronomic,
            string email, 
            string nickname, string password)
        {
            Surname = surname;
            Name = name;
            Patronomic = patronomic;
            Email = email;
            Nickname = nickname;
            Password = password;
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
        /// Фамилия.
        /// </summary>
        public string Surname { get; }

        /// <summary>
        /// Nickname (Account name).
        /// </summary>
        public string Nickname { get; }
        
        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; }
    }
}