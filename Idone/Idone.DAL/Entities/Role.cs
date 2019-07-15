namespace Idone.DAL.Entities
{
    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

    using Microsoft.AspNetCore.Identity;

    using LanguageExt;

    using static LanguageExt.Prelude;

    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public class Role: IdentityRole<int>
    {
        /// <summary>
        /// Конструктор для EF Core.
        /// </summary>
        private Role()
        {
        }

        /// <summary>
        /// Создать роль.
        /// </summary>
        /// <param name="newRole"> DTO с данными для создания роли. </param>
        /// <returns> Возращает монаду создания роли. </returns>
        public static Either<Error, Role> Create(DtoNewRole newRole)
        {
            var role = new Role
            {
                Name = newRole.Name
            };

            return Right<Error, Role>(role);
        }
    }
}