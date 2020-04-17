namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO, описывающее пагинацию таблицы.
    /// </summary>
    public class Pagination : Record<Pagination>
    {
        /// <summary>
        /// Количество элементов на странице.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Номер страницы.
        /// </summary>
        public int Page { get; private set; }

        public Pagination(int count, int page)
        {
            Count = count;
            Page = page;
        }
    }
}