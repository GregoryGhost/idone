namespace Idone.DAL.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// DTO таблицы прав.
    /// </summary>
    public class DtoGridPermission : DtoGrid<DtoRowPermission>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="rows"> Строки. </param>
        /// <param name="total"> Общее количество записей. </param>
        public DtoGridPermission(IEnumerable<DtoRowPermission> rows, int total)
            : base(rows, total)
        {
        }
    }
}