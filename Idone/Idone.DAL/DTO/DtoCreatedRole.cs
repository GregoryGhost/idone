namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO созданной роли.
    /// </summary>
    public class DtoCreatedRole : Record<DtoCreatedRole>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Название роли. </param>
        public DtoCreatedRole(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; private set; }
    }
}