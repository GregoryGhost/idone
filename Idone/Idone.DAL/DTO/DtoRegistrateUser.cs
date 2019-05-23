namespace Idone.DAL.DTO
{
    using Idone.DAL.Entities;

    using LanguageExt;

    public class DtoRegistrateUser : Record<DtoRegistrateUser>
    {
        public DtoRegistrateUser(
            string sid,
            string surname,
            string name,
            string patronomic,
            string email)
        {
            Sid = sid;
            Surname = surname;
            Name = name;
            Patronomic = patronomic;
            Email = email;
        }
        
        public string Email { get; }

        public string Name { get; }

        public string Patronomic { get; }

        public string Sid { get; }

        public string Surname { get; }
    }
}