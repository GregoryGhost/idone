namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    /// <summary>
    /// DTO для получения табличных записей прав.
    /// </summary>
    public class DtoGridQueryPermission : AbstractGridQuery<DtoPermissionFilter>
    {
        /// <inheritdoc />
        public DtoGridQueryPermission(DtoPermissionFilter filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}