using System;
using System.Linq;
using Caliburn.Micro;
using Coin.Accounts;
using Coin.Data;
using Coin.Shared;

namespace Coin.Transactions
{
    public class AccountTransactionEditViewModel : Screen
    {
        private int _id;
        private DateTimeOffset? _transactionTime;
        private DateTime _recordedDate;
        private MoneyViewModel _amount;
        private string _payee;
        private string _description;
        private AccountTransactionTypeViewModel _accountTransactionType;

        public int Id   
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public DateTimeOffset? TransactionTime
        {
            get { return _transactionTime; }
            set
            {
                if (value.Equals(_transactionTime)) return;
                _transactionTime = value;
                NotifyOfPropertyChange(() => TransactionTime);
            }
        }

        public DateTime RecordedDate
        {
            get { return _recordedDate; }
            set
            {
                if (value.Equals(_recordedDate)) return;
                _recordedDate = value;
                NotifyOfPropertyChange(() => RecordedDate);
            }
        }

        public MoneyViewModel Amount
        {
            get { return _amount; }
            set
            {
                if (value == _amount) return;
                _amount = value;
                NotifyOfPropertyChange(() => Amount);
            }
        }

        public string Payee
        {
            get { return _payee; }
            set
            {
                if (value == _payee) return;
                _payee = value;
                NotifyOfPropertyChange(() => Payee);
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }

        public AccountTransactionTypeViewModel AccountTransactionType
        {
            get { return _accountTransactionType; }
            set
            {
                if (value == _accountTransactionType) return;
                _accountTransactionType= value;
                NotifyOfPropertyChange(() => AccountTransactionType);
            }
        }

        public CurrencyViewModel AccountCurrency { get; set; }
        public BindableCollection<AccountTransactionTypeViewModel> AccountTransactionTypes { get; }

        public AccountTransactionEditViewModel()
        {
            AccountTransactionTypes = new BindableCollection<AccountTransactionTypeViewModel>();
        }

        public AccountTransactionEditViewModel ForAccount(AccountViewModel account)
        {
            AccountCurrency = account.Currency;
            return this;
        }

        protected override void OnInitialize()
        {
            using (var db = new Database())
            {
                AccountTransactionTypes.AddRange(
                    db.AccountTransactionTypes
                      .OrderBy(x => x.Name)
                      .Select(AccountTransactionTypeViewModel.CreateFrom));

                if (AccountTransactionType != null)
                {
                    AccountTransactionType =
                        AccountTransactionTypes
                            .SingleOrDefault(x => x.Id == AccountTransactionType.Id);
                }
            }

            RecordedDate = DateTime.Now;
            Amount = Amount ?? new MoneyViewModel {Amount = null, Currency = AccountCurrency};
        }

        public static AccountTransactionEditViewModel CreateFrom(AccountTransaction arg)
        {
            var currencyViewModel = CurrencyViewModel.CreateFrom(arg.AccountStatement.Account.Currency);

            var vm = new AccountTransactionEditViewModel
            {
                Id                           = arg.Id,
                Amount                       = MoneyViewModel.CreateFrom(arg.Amount, currencyViewModel),
                AccountCurrency              = currencyViewModel,
                AccountTransactionType       = AccountTransactionTypeViewModel.CreateFrom(arg.AccountTransactionType),
                Description                  = arg.Description,
                Payee                        = arg.Payee,
                RecordedDate                 = arg.RecordedDate,
                TransactionTime              = arg.TransactionTime
            };

            return vm;
        }
    }
}