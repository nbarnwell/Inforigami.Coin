using System;
using System.Data.Entity.Infrastructure;
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
        private DateTimeViewModel _transactionTime;
        private DateTime _recordedDate;
        private MoneyViewModel _amount;
        private string _payee;
        private string _description;
        private AccountTransactionTypeViewModel _selectedAccountTransactionType;

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

        public DateTimeViewModel TransactionTime
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

        public AccountTransactionTypeViewModel SelectedAccountTransactionType
        {
            get { return _selectedAccountTransactionType; }
            set
            {
                if (value == _selectedAccountTransactionType) return;
                _selectedAccountTransactionType= value;
                NotifyOfPropertyChange(() => SelectedAccountTransactionType);
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

                if (SelectedAccountTransactionType != null)
                {
                    SelectedAccountTransactionType =
                        AccountTransactionTypes
                            .SingleOrDefault(x => x.Id == SelectedAccountTransactionType.Id);
                }
            }

            RecordedDate = DateTime.Now;
            Amount = Amount ?? new MoneyViewModel {Amount = null, Currency = AccountCurrency};
            TransactionTime = TransactionTime ?? new DateTimeViewModel();
        }

        public static AccountTransactionEditViewModel CreateFrom(AccountTransaction arg)
        {
            var currencyViewModel = CurrencyViewModel.CreateFrom(arg.AccountStatement.Account.Currency);

            var vm = new AccountTransactionEditViewModel
            {
                Id                           = arg.Id,
                Amount                       = MoneyViewModel.CreateFrom(arg.Amount, currencyViewModel),
                AccountCurrency              = currencyViewModel,
                SelectedAccountTransactionType       = AccountTransactionTypeViewModel.CreateFrom(arg.AccountTransactionType),
                Description                  = arg.Description,
                Payee                        = arg.Payee,
                RecordedDate                 = arg.RecordedDate,
                TransactionTime              = DateTimeViewModel.CreateFrom(arg.TransactionTime?.Date, arg.TransactionTime?.TimeOfDay)
            };

            return vm;
        }
    }
}