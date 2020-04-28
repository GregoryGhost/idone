namespace Idone.Security.Services
{
    public partial class AdService
    {
        internal enum DictValueCase
        {
            WrongKey = 0,

            EmptyValue = 1
        }

        internal class DictValueCases
        {
            private DictValueCases(DictValueCase valueCase, string value)
            {
                DictValueCase = (valueCase, value);
            }

            public (DictValueCase, string) DictValueCase { get; }

            public static DictValueCases Create(DictValueCase valueCase, string value)
            {
                return new DictValueCases(valueCase, value);
            }
        }
    }
}