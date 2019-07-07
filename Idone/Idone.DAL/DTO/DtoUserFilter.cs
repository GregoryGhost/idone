namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO фильтра пользователей.
    /// </summary>
    public class DtoUserFilter : Record<DtoUserFilter>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="searchName"> Имя пользователя. </param>
        public DtoUserFilter(string searchName)
        {
            SearchName = searchName;
        }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string SearchName { get; private set; }
    }
}