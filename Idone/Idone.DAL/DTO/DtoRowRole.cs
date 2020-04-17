namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO строки роли в таблице ролей.
    /// </summary>
    public class DtoRowRole : Record<DtoRowRole>, IIdentity
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Название роли. </param>
        /// <param name="id"> Идентификатор роли. </param>
        public DtoRowRole(string name, int id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; }
    }
}