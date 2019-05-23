namespace Idone.DAL.DTO
{
    using LanguageExt;

    public class DtoRegistratedUser : Record<DtoRegistrateUser>
    {
        public DtoRegistratedUser(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}