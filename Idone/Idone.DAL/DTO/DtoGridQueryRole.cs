namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    /// <summary>
    /// DTO для формирования табличных записей.
    /// </summary>
    public class DtoGridQueryRole : AbstractGridQuery<DtoRoleFilter>
    {
        /// <inheritdoc />
        public DtoGridQueryRole(DtoRoleFilter filter, Pagination pagination)
            : base(filter, pagination)
        {
        }

        /// <inheritdoc />
        public DtoGridQueryRole(Pagination pagination)
            : base(pagination)
        {
        }
    }
}