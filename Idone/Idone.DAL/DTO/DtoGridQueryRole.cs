namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    /// <summary>
    /// DTO для формирования табличных записей.
    /// </summary>
    public class DtoGridQueryRole : AbstractGridQuery<DtoRoleFilter>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="filter"> Фильтр ролей. </param>
        /// <param name="pagination"> Настройки постраничного вывода ролей. </param>
        public DtoGridQueryRole(DtoRoleFilter filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}