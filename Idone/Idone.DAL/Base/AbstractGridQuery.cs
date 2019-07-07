namespace Idone.DAL.Base
{
    using Idone.DAL.DTO;

    using LanguageExt;

    /// <summary>
    /// Базовый класс для запроса на формирование таблицы записей.
    /// </summary>
    /// <typeparam name="TFilter"> Тип фильтра записей. </typeparam>
    public abstract class AbstractGridQuery<TFilter> : Record<AbstractGridQuery<TFilter>>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="filter"> Фильтр. </param>
        /// <param name="pagination"> Настройки постраничного вывода. </param>
        protected AbstractGridQuery(TFilter filter, Pagination pagination)
        {
            Filter = filter;
            Pagination = pagination;
        }

        /// <summary>
        /// Фильтр.
        /// </summary>
        public virtual Option<TFilter> Filter { get; private set; }

        /// <summary>
        /// Пагинация.
        /// </summary>
        public virtual Pagination Pagination { get; private set; }
    }
}