namespace Idone.DAL.Entities
{
    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

    using LanguageExt;

    using static LanguageExt.Prelude;

    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User : IdentityUser<int>
    {
        public FullName FullName { get; private set; }

        /// <summary>
        /// Конструктор для EF Core.
        /// </summary>
        private User()
        {
        }

        private User(FullName fullName)
        {
            FullName = fullName;
        }

        public static bool operator ==(User x, User y)
        {
            return RecordType<User>.EqualityTyped(x, y);
        }

        public static bool operator !=(User x, User y)
        {
            return !(x == y);
        }

        public static Either<Error, User> Create(DtoRegistrateUser registrateUser)
        {
            var user = new User(new FullName(registrateUser.Surname, registrateUser.Name, registrateUser.Patronomic));
            user.DisplayName = "kek";
            user.Email = registrateUser.Email;
            user.UserName = registrateUser.Email;
            return Right<Error, User>(user);
        }

        public string DisplayName { get; private set; }
    }

    public class FullName
    {
        public string Name { get; private set; }

        public string Patronymic { get; private set; }

        public string Surname { get; private set; }

        /// <summary>
        /// Конструктор для EF Core.
        /// </summary>
        private FullName()
        {
        }

        public FullName(string surname, string name, string patronymic)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
        }
    }
}