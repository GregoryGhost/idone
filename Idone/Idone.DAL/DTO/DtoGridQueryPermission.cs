namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    /// <summary>
    /// DTO для получения табличных записей прав.
    /// </summary>
    public class DtoGridQueryPermission : AbstractGridQuery<DtoFilterById>
    {
        /// <inheritdoc />
        public DtoGridQueryPermission(DtoFilterById filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}