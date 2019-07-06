namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    /// <summary>
    /// DTO для формирования табличных записей.
    /// </summary>
    public class DtoGridQueryUserRole : AbstractGridQuery<DtoFilterById>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="filter"> Фильтр по получение ролей пользователя. </param>
        /// <param name="pagination"> Настройки постраничного вывода ролей пользователя. </param>
        public DtoGridQueryUserRole(DtoFilterById filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}