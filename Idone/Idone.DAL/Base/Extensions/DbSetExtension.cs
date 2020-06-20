namespace Idone.DAL.Base.Extensions
{
    using System.Linq;

    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

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
        /// <param name="keyValue"> Параметр поиска. </param>
        /// <returns> Возвращает монаду Maybe. </returns>
        public static Option<T> Find<T>(this DbSet<T> dbSet, IIdentity keyValue)
            where T : class
        {
            var searchResult = dbSet.Find(keyValue.Id);
            return searchResult != null
                ? Some(searchResult)
                : None;
        }

        public static Either<Error, T> FindEither<T>(this DbSet<T> dbSet, IIdentity keyValue)
            where T : class
        {
            return dbSet.Find<T>(keyValue).ToEither(Error.NotFoundRecord);
        }
    }
}