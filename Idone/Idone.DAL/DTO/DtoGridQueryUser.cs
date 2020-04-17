namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    /// <summary>
    /// DTO для формирования запроса табличных записей.
    /// </summary>
    public class DtoGridQueryUser : AbstractGridQuery<DtoUserFilter>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="filter"> Фильтр пользователей. </param>
        /// <param name="pagination"> Настройки постраничного вывода пользователей. </param>
        public DtoGridQueryUser(DtoUserFilter filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}