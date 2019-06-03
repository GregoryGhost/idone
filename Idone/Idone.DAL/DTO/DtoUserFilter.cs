namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO фильтра пользователей.
    /// </summary>
    public class DtoUserFilter : Record<DtoUserFilter>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="email"> Почта пользователя. </param>
        public DtoUserFilter(string email)
        {
            Email = email;
        }

        /// <summary>
        /// Почта пользователя.
        /// </summary>
        public string Email { get; private set; }
    }
}