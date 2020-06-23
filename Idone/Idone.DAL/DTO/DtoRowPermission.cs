namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO табличная запись права.
    /// </summary>
    public class DtoRowPermission : Record<DtoRowPermission>, IIdentity
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Наименование права. </param>
        /// <param name="id">Идентификатор права.</param>
        public DtoRowPermission(string name, int id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Идентификатор права.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Наименование права.
        /// </summary>
        public string Name { get; }

        public static DtoRowPermission CreateFromDb(string name, int id)
        {
            return new DtoRowPermission(name, id);
        }
    }
}