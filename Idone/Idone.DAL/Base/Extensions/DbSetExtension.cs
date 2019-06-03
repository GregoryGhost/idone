namespace Idone.DAL.Base.Extensions
{
    using LanguageExt;

    using Microsoft.EntityFrameworkCore;

    using static LanguageExt.Prelude;

    /// <summary>
    /// Расширения для репозитория EF Core.
    /// </summary>
    public static class DbSetExtension
    {
        /// <summary>
        /// Очистить таблицу.
        /// </summary>
        /// <typeparam name="T"> Тип сущностей таблицы. </typeparam>
        /// <param name="dbSet"> Репозиторий. </param>
        public static void Clear<T>(this DbSet<T> dbSet)
            where T : class
        {
            dbSet.RemoveRange(dbSet);
        }

        /// <summary>
        /// Найти объект.
        /// </summary>
        /// <typeparam name="T"> Тип сущности. </typeparam>
        /// <param name="dbSet"> Репозиторий. </param>
        /// <param name="keyValues"> Параметры поиска. </param>
        /// <returns> Возвращает монаду Maybe. </returns>
        public static Option<T> Find<T>(this DbSet<T> dbSet, params object[] keyValues)
            where T : class
        {
            var searchResult = dbSet.Find(keyValues);
            return searchResult != null
                ? Some(searchResult)
                : None;
        }
    }
}