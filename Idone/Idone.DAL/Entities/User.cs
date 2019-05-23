namespace Idone.DAL.Entities
{
    using Idone.DAL.Dictionaries;
    using Idone.DAL.DTO;

    using LanguageExt;

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

        public User(FullName fullName)
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
            throw new System.NotImplementedException();
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