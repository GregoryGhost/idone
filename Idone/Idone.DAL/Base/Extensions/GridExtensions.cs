namespace Idone.DAL.Base.Extensions
{
    using System.Linq;

    using Idone.DAL.DTO;

    /// <summary>
    /// Расширения для работы с таблицей.
    /// </summary>
    public static class GridExtensions
    {
        /// <summary>
        /// Выполнить постраничное формирование сущностей.
        /// </summary>
        /// <typeparam name="TEntity"> Тип сущностей. </typeparam>
        /// <param name="dbQuery"> Запрос. </param>
        /// <param name="pagination"> Настройки постраничного вывода. </param>
        /// <returns> Возвращает запрос. </returns>
        public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> dbQuery, Pagination pagination)
        {
            var skip = (pagination.Page - 1) * pagination.Count;
            var entities = dbQuery.Skip(skip).Take(pagination.Count);

            return entities;
        }
    }
}