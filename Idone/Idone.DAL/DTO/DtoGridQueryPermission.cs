namespace Idone.DAL.DTO
{
    using Idone.DAL.Base;

    /// <summary>
    /// DTO для получения табличных записей прав.
    /// </summary>
    public class DtoGridQueryPermission : AbstractGridQuery<DtoPermissionFilter>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Наименование права. </param>
        public DtoGridQueryPermission(DtoPermissionFilter filter, Pagination pagination)
            : base(filter, pagination)
        {
        }
    }
}