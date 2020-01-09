namespace Idone.DAL.DTO
{
    using LanguageExt;

    public class DtoNewPermission: Record<DtoNewPermission>
    {
        public DtoNewPermission(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}