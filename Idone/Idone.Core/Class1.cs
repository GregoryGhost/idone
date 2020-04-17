namespace Idone.Core
{
    using LanguageExt;
    using LanguageExt.UnsafeValueAccess;

    using static LanguageExt.Prelude;

    public static class Class1
    {
        public static Either<string, int> TestDatas(int id)
        {
            var isValid = Validate(id);
            return isValid.IsRight ? (Either<string, int>)isValid.Value() : "kek";
        }

        private static Either<string, int> Validate(int number) => number > 0
                ? Right<string, int>(number)
                : Left("печалька, невалидно");
    }
}