namespace Idone.DAL.DTO
{
    /// <summary>
    /// DTO строки пользователя в таблице пользователей.
    /// </summary>
    public class DtoRowUser
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="email"> Почта. </param>
        /// <param name="displayName"> Отображаемое имя пользователя. </param>
        public DtoRowUser(string email, string displayName)
        {
            Email = email;
            DisplayName = displayName;
        }

        /// <summary>
        /// Отображаемое имя пользователя.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Почта пользователя.
        /// </summary>
        public string Email { get; }
    }
}