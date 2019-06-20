namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO фильтра прав.
    /// </summary>
    public class DtoPermissionFilter : Record<DtoPermissionFilter>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Наименование права. </param>
        public DtoPermissionFilter(string name)
        {
            Name = name;
        }
        
        /// <summary>
        /// Наименование права.
        /// </summary>
        public string Name { get; private set; }
    }
}