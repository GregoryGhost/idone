namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    public class DtoGridQueryPermissionRoles:  AbstractGridQuery<IIdentity>
    {
        public DtoGridQueryPermissionRoles(IIdentity filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}