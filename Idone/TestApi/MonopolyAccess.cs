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

        private readonly AsyncManualResetEvent _trigger = new AsyncManualResetEvent(false);

        public async Task<Either<MonopolyAccessErrors, MonopolyAccessStates>> AddRecordAsync(int newNumber)
        {
            if (_trigger.IsSet)
                return Right<MonopolyAccessErrors, MonopolyAccessStates>(MonopolyAccessStates.IsBlocked);

            _trigger.Set();

            _list.Add(newNumber);

            return Right<MonopolyAccessErrors, MonopolyAccessStates> (MonopolyAccessStates.WasBlocked);
        }

        public void ResetBlock() => _trigger.Reset();
    }

    public enum MonopolyAccessStates
    {
        IsBlocked,
        WasBlocked,
    }

    public enum MonopolyAccessErrors
    {
        Poisoned
    }
}