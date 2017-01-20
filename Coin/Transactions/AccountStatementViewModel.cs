using System;
using Caliburn.Micro;
using Coin.Data;

namespace Coin.Transactions
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
        public BindableCollection<AccountTransactionEditViewModel> Transactions { get; }

        public AccountStatementViewModel()
        {
            Transactions = new BindableCollection<AccountTransactionEditViewModel>();
        }

        public static AccountStatementViewModel CreateFrom(Data.AccountStatement arg)
        {
            return new AccountStatementViewModel
            {
                Id = arg.Id,
                PeriodStart = arg.PeriodStart
            };
        }
    }
}