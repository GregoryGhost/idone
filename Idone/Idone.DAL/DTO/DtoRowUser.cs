namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO строки пользователя в таблице пользователей.
    /// </summary>
    public class DtoRowUser : Record<DtoRowUser>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="email"> Почта. </param>
        /// <param name="displayName"> Отображаемое имя пользователя. </param>
        /// <param name "id"> Идентификатор пользователя. </param>
        public DtoRowUser(string email, string displayName, int id)
        {
            Email = email;
            DisplayName = displayName;
            Id = id;
        }

        /// <summary>
        /// Отображаемое имя пользователя.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Почта пользователя.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int Id { get; }
    }
}