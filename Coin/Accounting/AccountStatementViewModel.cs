using System;
using Caliburn.Micro;
using Coin.Data;

namespace Coin.Accounting
{
    public class AccountStatementViewModel : Screen
    {
        private DateTimeOffset _periodStart;

        public DateTimeOffset PeriodStart
        {
            get { return _periodStart; }
            set
            {
                if (value.Equals(_periodStart)) return;
                _periodStart = value;
                NotifyOfPropertyChange(() => PeriodStart);
            }
        }

        public int Id { get; set; }

        public static AccountStatementViewModel CreateFrom(AccountStatement arg)
        {
            return new AccountStatementViewModel
            {
                Id = arg.Id,
                PeriodStart = arg.PeriodStart
            };
        }
    }
}