namespace Idone.DAL.DTO
{
    using System.Collections.Generic;

    using LanguageExt;

    /// <summary>
    /// DTO для формирования таблицы записей.
    /// </summary>
    /// <typeparam name="T"> Тип записей таблицы. </typeparam>
    public class DtoGrid<T> : Record<DtoGrid<T>>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="rows"> Строки. </param>
        /// <param name="total"> Общее количество строк. </param>
        public DtoGrid(IEnumerable<T> rows, int total)
        {
            Rows = rows;
            Total = total;
        }

        /// <summary>
        /// Строки.
        /// </summary>
        public IEnumerable<T> Rows { get; }

        /// <summary>
        /// Общее количество строк.
        /// </summary>
        public int Total { get; }
    }
}