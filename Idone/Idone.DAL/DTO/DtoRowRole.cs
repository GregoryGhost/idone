namespace Idone.DAL.DTO
{
    using LanguageExt;

    /// <summary>
    /// DTO строки роли в таблице ролей.
    /// </summary>
    public class DtoRowRole : Record<DtoRowRole>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="name"> Название роли. </param>
        public DtoRowRole(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; }
    }
}