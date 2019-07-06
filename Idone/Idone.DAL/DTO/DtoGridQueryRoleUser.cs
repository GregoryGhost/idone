namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    /// <summary>
    /// DTO для формирования табличных записей.
    /// </summary>
    public class DtoGridQueryRoleUser : AbstractGridQuery<DtoFilterById>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="filter"> Фильтр на получение пользовалей роли. </param>
        /// <param name="pagination"> Настройки постраничного вывода пользователей роли. </param>
        public DtoGridQueryRoleUser(DtoFilterById filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}