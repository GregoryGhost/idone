namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    public class DtoGridQueryRolePermission:  AbstractGridQuery<DtoFilterById>
    {
        public DtoGridQueryRolePermission(DtoFilterById filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}