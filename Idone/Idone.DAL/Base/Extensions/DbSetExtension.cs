namespace Idone.DAL.Base.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Idone.DAL.Dictionaries;

    using LanguageExt;
    using LanguageExt.SomeHelp;

    using static LanguageExt.Prelude;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Расширения для репозитория EF Core.
    /// </summary>
    public static class DbSetExtension
    {
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