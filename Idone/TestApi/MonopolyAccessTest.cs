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
            var result = await _monopolyAccess.AddRecordAsync(1234);
            if (result.IsLeft)
            {
                Assert.Fail("����, ���-�� ����� �� ���");
            }

            if (result.IsRight)
            {
                var state = result.ValueUnsafe();
                if (state == MonopolyAccessStates.IsBlocked)
                {
                    Assert.Fail("��� ���-�� ������������ ������");
                }
                else if (MonopolyAccessStates.WasBlocked == state)
                {
                    Assert.Pass("������� �������� ����������� ������ =)");
                }
            }
        }
    }
}