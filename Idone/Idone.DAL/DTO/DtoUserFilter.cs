namespace Idone.DAL.DTO
{
    using LanguageExt;

    public class DtoUserFilter : Record<DtoUserFilter>
    {
        public DtoUserFilter(string email)
        {
            Email = email;
        }

        public string Email { get; private set; }
    }
}