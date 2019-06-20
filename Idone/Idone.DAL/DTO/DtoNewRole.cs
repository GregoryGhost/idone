namespace Idone.Security
{
    using LanguageExt;

    public class DtoNewRole : Record<DtoNewRole>
    {
        public DtoNewRole(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}