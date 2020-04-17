namespace Idone.DAL.Entities
{
    using Idone.DAL.Dictionaries;

    using LanguageExt;

    using static LanguageExt.Prelude;

    /// <summary>
    /// Роль, назначенная пользователю.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Конструктор для EF Core.
        /// </summary>
        private UserRole()
        {
        }

        public int Id { get; private set; }

        public User User { get; private set; }

        public Role Role { get; private set; }

        public static Either<Error, UserRole> Create(User user, Role role)
        {
            var userRole = new UserRole
            {
                User = user,
                Role = role
            };

            return Right<Error, UserRole>(userRole);
        }
    }
}