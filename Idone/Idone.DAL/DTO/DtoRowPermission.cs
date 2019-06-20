namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO табличная запись права.
    /// </summary>
    public class DtoRowPermission : Record<DtoRowPermission>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Наименование права. </param>
        public DtoRowPermission(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Наименование права.
        /// </summary>
        public string Name { get; set; }
    }
}