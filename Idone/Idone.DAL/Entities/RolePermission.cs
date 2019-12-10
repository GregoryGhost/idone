namespace Idone.DAL.Entities
{
    using Idone.DAL.Dictionaries;

    using LanguageExt;

    /// <summary>
    ///     Связка роль-право.
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        ///     Конструктор для EF Core.
        /// </summary>
        private RolePermission()
        {
        }

        /// <summary>
        ///     Идентификатор.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     Право.
        /// </summary>
        public Permission Permission { get; private set; }

        /// <summary>
        ///     Роль.
        /// </summary>
        public Role Role { get; private set; }

        public Either<Error, RolePermission> Create(Permission permission, Role role)
        {
            return new RolePermission
            {
                Permission = permission,
                Role = Role
            };
        }
    }
}