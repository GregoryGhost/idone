namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO фильтра по идентификатору сущности.
    /// </summary>
    public class DtoFilterById : Record<DtoFilterById>, IIdentity
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

        /// <summary>
        /// Создать DTO из данных БД.
        /// <remarks>
        ///     Исправляет ошибку
        ///     System.ArgumentException: Expression of type 'System.Nullable`1[System.Int32]' cannot be used for constructor parameter of type 'System.Int32
        /// </remarks>
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Возвращает DTO.</returns>
        public static DtoFilterById CreateFromDb(int id)
        {
            return new DtoFilterById(id);
        }
    }
}