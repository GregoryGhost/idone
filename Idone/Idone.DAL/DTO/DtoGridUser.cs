namespace Idone.DAL.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// DTO таблицы пользователей.
    /// </summary>
    public class DtoGridUser : DtoGrid<DtoRowUser>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="rows"> Строки. </param>
        /// <param name="total"> Общее количество записей. </param>
        public DtoGridUser(IEnumerable<DtoRowUser> rows, int total)
            : base(rows, total)
        {
        }
    }
}