namespace Idone.DAL.DTO
{
    public class DtoRowUser
    {
        public DtoRowUser(string email, string displayName)
        {
            Email = email;
            DisplayName = displayName;
        }

        public string DisplayName { get; }

        public string Email { get; }
    }
}