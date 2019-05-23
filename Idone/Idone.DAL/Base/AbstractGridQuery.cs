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
        public AbstractGridQuery(TFilter filter, Pagination pagination)
        {
            Filter = filter;
            Pagination = pagination;
        }

        /// <summary>
        /// Фильтр.
        /// </summary>
        public virtual TFilter Filter { get; private set; }

        /// <summary>
        /// Пагинация.
        /// </summary>
        public virtual Pagination Pagination { get; private set; }
    }
}