namespace Idone.DAL.DTO
{
    using System.Collections.Generic;

    public class DtoGridRole : DtoGrid<DtoRowRole>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="rows"> Строки. </param>
        /// <param name="total"> Общее количество записей. </param>
        public DtoGridRole(IEnumerable<DtoRowRole> rows, int total)
            : base(rows, total)
        {
        }
    }
}