namespace Idone.Security
{
    using LanguageExt;

    /// <summary>
    /// DTO новой роли.
    /// </summary>
    public class DtoNewRole : Record<DtoNewRole>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Название роли. </param>
        public DtoNewRole(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; private set; }
    }
}