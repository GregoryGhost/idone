namespace Idone.DAL.Entities
{
    using LanguageExt;

    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User : IdentityUser<int>
    {
        public readonly FullName FullName;

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
    }

    public class FullName
    {
        public readonly string Name;

        public readonly string Patronymic;

        public readonly string Surname;

        public FullName(string surname, string name, string patronymic)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
        }
    }
}