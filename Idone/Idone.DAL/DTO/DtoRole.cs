namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO созданной роли.
    /// </summary>
    public class DtoRole : Record<DtoRole>, IIdentity
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Название роли. </param>
        public DtoRole(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; private set; }

        public int Id { get; }
    }
}