namespace Idone.Security
{
    using Idone.DAL.Base;
    using Idone.DAL.DTO;

    public class DtoGridQueryUser : AbstractGridQuery<DtoUserFilter>
    {
        public DtoGridQueryUser(DtoUserFilter filter, Pagination pagination) 
            : base(filter, pagination)
        {
        }
    }
}