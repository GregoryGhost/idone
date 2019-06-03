namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO зарегистрированного пользователя.
    /// </summary>
    public class DtoRegistratedUser : Record<DtoRegistrateUser>
    {
        /// <summary>
        /// Контруктор по-умолчанию.
        /// </summary>
        /// <param name="email"> Электронная почта. </param>
        public DtoRegistratedUser(string email)
        {
            Email = email;
        }

        /// <summary>
        /// Электронная почта.
        /// </summary>
        public string Email { get; }
    }
}