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
        /// Инициализировать запрос только с постраничным выводом.
        /// </summary>
        /// <param name="pagination">Настройки постраничного вывода.</param>
        protected AbstractGridQuery(Pagination pagination)
        {
            Filter = Option<TFilter>.None;
            Pagination = pagination;
        }

        /// <summary>
        /// Фильтр.
        /// </summary>
        public virtual Option<TFilter> Filter { get; }

        /// <summary>
        /// Пагинация.
        /// </summary>
        public virtual Pagination Pagination { get; }
    }
}