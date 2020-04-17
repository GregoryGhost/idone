namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    public class DtoGridQueryUserPermission :  AbstractGridQuery<DtoFilterById>
    {
        public DtoGridQueryUserPermission(DtoFilterById filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}