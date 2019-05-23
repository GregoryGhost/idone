namespace Idone.DAL.Base.Extensions
{
    using System.Linq;

    using Idone.DAL.DTO;

    public static class GridExtensions
    {
        public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> dbQuery, Pagination pagination)
        {
            var skip = (pagination.Page - 1) * pagination.Count;
            var entities = dbQuery.Skip(skip).Take(pagination.Count);

            return entities;
        }
    }
}