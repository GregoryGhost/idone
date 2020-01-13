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

        [Test]
        public async Task CheckPermissionMonopolyAccess()
        {
            _monopolyAccess.ResetBlock();

            var result = await _monopolyAccess.AddRecordAsync(1234);

            if (result.IsLeft)
                Assert.Fail("���-�� ����� �� ���");

            if (result.IsRight)
                if (result.ValueUnsafe() == MonopolyAccessStates.IsBlocked)
                    Assert.Fail("���-�� ��� ������������ ������");

            var result2 = await _monopolyAccess.AddRecordAsync(3442);


            if (result2.IsLeft)
                Assert.Fail("���-�� ����� �� ���");

            if (result2.IsRight)
                if (result2.ValueUnsafe() == MonopolyAccessStates.IsBlocked)
                    Assert.Pass("������� ��������� ����������");
                else if (result.ValueUnsafe() == MonopolyAccessStates.WasBlocked)
                    Assert.Fail("�������, ���-�� ��� ���� ����������, � ��� ���� ����������� ������");
        }
    }
}