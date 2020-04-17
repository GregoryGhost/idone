namespace Idone.DAL.DTO
{
    using LanguageExt;

    public class DtoPermission: Record<DtoPermission>, IIdentity
    {
        public DtoPermission(int id, string name)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; }

        public int Id { get; }
    }
}