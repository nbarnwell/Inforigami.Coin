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
        private DateTimeViewModel _transactionTime;
        private string _payee;
        private AccountTransactionTypeViewModel _selectedAccountTransactionType;
        private string _description;
        private string _splitAmount;
        private ListItemViewModel _selectedCategory;

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

        public string SplitAmount
        {
            get { return _splitAmount; }
            set
            {
                if (value == _splitAmount) return;
                _splitAmount = value;
                NotifyOfPropertyChange(() => SplitAmount);
            }
        }

        public ListItemViewModel SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (Equals(value, _selectedCategory)) return;
                _selectedCategory = value;
                NotifyOfPropertyChange(() => SelectedCategory);
            }
        }

        public decimal AmountTotal
        {
            get { return CategorySplits.Sum(x => x.SplitAmount); }
        }

        public CurrencyViewModel AccountCurrency { get; set; }
        public BindableCollection<AccountTransactionTypeViewModel> AccountTransactionTypes { get; }
        public BindableCollection<ListItemViewModel> Categories { get; }
        public BindableCollection<AccountTransactionCategorySplitViewModel> CategorySplits { get; }

        public AccountTransactionEditViewModel()
        {
            AccountTransactionTypes = new BindableCollection<AccountTransactionTypeViewModel>();
            Categories = new BindableCollection<ListItemViewModel>();
            CategorySplits = new BindableCollection<AccountTransactionCategorySplitViewModel>();
        }

        public Money GetTotal()
        {
            return new Money(CategorySplits.Sum(x => x.SplitAmount), AccountCurrency.Code);
        }

        public void AddSplit()
        {
            decimal splitAmount;
            if (decimal.TryParse(SplitAmount, out splitAmount))
            {
                CategorySplits.Add(
                    new AccountTransactionCategorySplitViewModel(
                        splitAmount,
                        SelectedCategory));

                SplitAmount = "";
                SelectedCategory = null;
                NotifyOfPropertyChange(() => AmountTotal);
            }
            else
            {
                throw new NotImplementedException("TODO: MessageBox to user");
            }
        }

        public void RemoveSplit(AccountTransactionCategorySplitViewModel split)
        {
            CategorySplits.Remove(split);
            NotifyOfPropertyChange(() => AmountTotal);
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

                Categories.AddRange(
                    db.AccountTransactionCategories
                      .OrderBy(x => x.Name)
                      .Select(x => new ListItemViewModel
                      {
                          Id = x.Id,
                          Name = x.Name,
                      }));
            }

            TransactionTime = TransactionTime ?? new DateTimeViewModel();
        }

        public static AccountTransactionEditViewModel CreateFrom(AccountTransaction arg)
        {
            var currencyViewModel = CurrencyViewModel.CreateFrom(arg.AccountStatement.Account.Currency);

            var vm = new AccountTransactionEditViewModel
            {
                Id                             = arg.Id,
                AccountCurrency                = currencyViewModel,
                SelectedAccountTransactionType = AccountTransactionTypeViewModel.CreateFrom(arg.AccountTransactionType),
                Description                    = arg.Description,
                Payee                          = arg.Payee,
                TransactionTime                =
                    DateTimeViewModel.CreateFrom(arg.TransactionTime?.Date, arg.TransactionTime?.TimeOfDay)
            };


            return vm;
        }
    }
}