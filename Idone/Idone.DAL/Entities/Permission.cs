namespace Idone.DAL.Entities
{
    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

    using LanguageExt;

    /// <summary>
    ///     Право на выполнение каких-то действий в системе.
    /// </summary>
    public class Permission
    {
        /// <summary>
        ///     Конструктор для EF Core.
        /// </summary>
        private Permission()
        {
        }

        /// <summary>
        ///     Описание права.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        ///     Идентификатор.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     Название права.
        /// </summary>
        public string Name { get; private set; }

        public static Either<Error, Permission> Create(string description, string name)
        {
            var permission = new Permission
            {
                Description = description,
                Name = name
            };

            return permission;
        }

        public static Either<Error, Permission> Create(DtoNewPermission newPermission)
        {
            var user = new Permission
            {
                Description = newPermission.Description,
                Name = newPermission.Name
            };

            return user;
        }
    }
}