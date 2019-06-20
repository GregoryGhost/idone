namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO фильтра ролей.
    /// </summary>
    public class DtoRoleFilter : Record<DtoRoleFilter>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Название роли. </param>
        public DtoRoleFilter(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; private set; }
    }
}