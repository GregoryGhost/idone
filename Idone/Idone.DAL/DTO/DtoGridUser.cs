namespace Idone.DAL.DTO
{
    using System.Collections.Generic;

    public class DtoGridUser : DtoGrid<DtoRowUser>
    {
        public DtoGridUser(IEnumerable<DtoRowUser> rows, int total)
            : base(rows, total)
        {
        }
    }
}