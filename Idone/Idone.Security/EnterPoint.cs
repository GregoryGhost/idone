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

        public Either<Error, Test> RegistrateUserEither(Test test)
        {
            return test != null
                ? Right<Error, Test>(test)
                : Left(Error.Exception);
        }
    }

    public interface ISecurityModule
    {
        Option<Test> RegistrateUser(Test test);
        //TODO:дополнить остальными методами для Ролей, Прав.
    }

    public enum Error
    {
        Exception = 0,
    }

    public class Test
    {
        public int Data { get; set; }
    }
}