using NUnit.Framework;

namespace Tests
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using LanguageExt.UnsafeValueAccess;

    using static LanguageExt.Prelude;

    public class Tests
    {
        private readonly MonopolyAccess _monopolyAccess = new MonopolyAccess();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CheckMonopolyAccess()
        {
            _monopolyAccess.ResetBlock();

            var result = await _monopolyAccess.AddRecordAsync(1234);
            if (result.IsLeft)
            {
                Assert.Fail("Упсс, что-то пошло не так");
            }

            if (result.IsRight)
            {
                var state = result.ValueUnsafe();
                if (state == MonopolyAccessStates.IsBlocked)
                {
                    Assert.Fail("Уже кто-то заблокировал доступ");
                }
                else if (MonopolyAccessStates.WasBlocked == state)
                {
                    Assert.Pass("Удалось получить монопольный доступ =)");
                }
            }
        }

        [Test]
        public async Task CheckPermissionMonopolyAccess()
        {
            _monopolyAccess.ResetBlock();

            var result = await _monopolyAccess.AddRecordAsync(1234);

            if (result.IsLeft)
                Assert.Fail("Что-то пошло не так");

            if (result.IsRight)
                if (result.ValueUnsafe() == MonopolyAccessStates.IsBlocked)
                    Assert.Fail("Кто-то уже заблокировал доступ");

            var result2 = await _monopolyAccess.AddRecordAsync(3442);


            if (result2.IsLeft)
                Assert.Fail("Что-то пошло не так");

            if (result2.IsRight)
                if (result2.ValueUnsafe() == MonopolyAccessStates.IsBlocked)
                    Assert.Pass("Успешно проверена блокировка");
                else if (result.ValueUnsafe() == MonopolyAccessStates.WasBlocked)
                    Assert.Fail("Странно, кто-то уже снял блокировку, и она была установлена заново");
        }
    }
}