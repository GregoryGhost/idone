namespace Idone.DAL.DTO
{
    using LanguageExt;

    public class DtoPermission: Record<DtoPermission>, IIdentity
    {
        public DtoPermission(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public int Id { get; }
    }
}