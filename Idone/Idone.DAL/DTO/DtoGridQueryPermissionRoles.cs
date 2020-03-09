namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    public class DtoGridQueryPermissionRoles:  AbstractGridQuery<DtoFilterById>
    {
        public DtoGridQueryPermissionRoles(DtoFilterById filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}