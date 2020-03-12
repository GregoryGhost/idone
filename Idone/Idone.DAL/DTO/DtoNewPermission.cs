namespace Idone.DAL.DTO
{
    using LanguageExt;

    public class DtoNewPermission: Record<DtoNewPermission>
    {
        public DtoNewPermission(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}