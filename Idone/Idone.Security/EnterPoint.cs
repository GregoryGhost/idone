namespace Idone.Security
{
    using LanguageExt;

    using static LanguageExt.Prelude;

    public class EnterPoint : ISecurityModule
    {
        public Option<Test> RegistrateUser(Test test)
        {
            return test != null
                ? Some(test)
                : None;
        }
    }

    public interface ISecurityModule
    {
        Option<Test> RegistrateUser(Test test);
        //TODO:дополнить остальными методами для Ролей, Прав.
    }

    public class Test
    {
        public int Data { get; set; }
    }
}