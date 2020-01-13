namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using LanguageExt;
    using static LanguageExt.Prelude;

    using Nito.AsyncEx;

    public class MonopolyAccess
    {
        private readonly List<int> _list = new List<int>();

        private readonly AsyncAutoResetEvent _trigger = new AsyncAutoResetEvent(false);

        public async Task<Either<MonopolyAccessFail, MonopolyAccessStates>> AddRecordAsync(int newNumber)
        {
            if (_trigger.IsSet)
                return Right<MonopolyAccessFail, MonopolyAccessStates>(MonopolyAccessStates.IsBlocked);

            _trigger.Set();

            return Right<MonopolyAccessFail, MonopolyAccessStates> (MonopolyAccessStates.WasBlocked);
        }
    }

    public enum MonopolyAccessStates
    {
        IsBlocked,
        WasBlocked,
    }

    public enum MonopolyAccessFail
    {
        Poisoned
    }
}