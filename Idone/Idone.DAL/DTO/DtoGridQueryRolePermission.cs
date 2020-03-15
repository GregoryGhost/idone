namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    public class DtoGridQueryRolePermission:  AbstractGridQuery<IIdentity>
    {
        public DtoGridQueryRolePermission(IIdentity filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}