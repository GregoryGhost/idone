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
        /// <param name="id"> Идентификатор пользователя. </param>
        public DtoRegistratedUser(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int Id { get; }
    }
}