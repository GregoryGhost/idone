namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO фильтра по идентификатору сущности.
    /// </summary>
    public class DtoFilterById : Record<DtoFilterById>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="id"> Идентификатор сущности. </param>
        public DtoFilterById(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        public int Id { get; }
    }
}