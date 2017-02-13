using System.Linq;
using Caliburn.Micro;
using Coin.CRUD.People;
using Coin.Data;
using Coin.Shared;

namespace Coin.CRUD.Accounts
{
    public class AccountViewModel : Screen
    {
        private int _accountId;
        private string _accountName;
        private BankAccountViewModel _bankAccountDetails;
        private bool _isBankAccount;
        private PersonViewModel _accountHolder;
        private CurrencyViewModel _currency;

        public BindableCollection<CurrencyViewModel> Currencies { get; }

        public int AccountId
        {
            get { return _accountId; }
            set
            {
                if (value == _accountId) return;
                _accountId = value;
                NotifyOfPropertyChange(() => AccountId);
            }
        }

        public string AccountName
        {
            get { return _accountName; }
            set
            {
                if (value == _accountName) return;
                _accountName = value;
                NotifyOfPropertyChange(() => AccountName);
            }
        }

        public CurrencyViewModel Currency
        {
            get { return _currency; }
            set
            {
                if (Equals(value, _currency)) return;
                _currency = value;
                NotifyOfPropertyChange(() => Currency);
            }
        }

        public bool IsBankAccount
        {
            get { return _isBankAccount; }
            set
            {
                if (value == _isBankAccount) return;
                _isBankAccount = value;
                NotifyOfPropertyChange(() => IsBankAccount);
            }
        }

        public BankAccountViewModel BankAccountDetails
        {
            get { return _bankAccountDetails; }
            set
            {
                if (Equals(value, _bankAccountDetails)) return;
                _bankAccountDetails = value;
                NotifyOfPropertyChange(() => BankAccountDetails);
            }
        }

        public PersonViewModel AccountHolder
        {
            get { return _accountHolder; }
            set
            {
                if (Equals(value, _accountHolder)) return;
                _accountHolder = value;
                NotifyOfPropertyChange(() => AccountHolder);
            }
        }

        public BindableCollection<PersonViewModel> People { get; }

        public AccountViewModel()
        {
            BankAccountDetails = new BankAccountViewModel();
            BankAccountDetails.ConductWith(this);

            Currencies = new BindableCollection<CurrencyViewModel>();
            People = new BindableCollection<PersonViewModel>();
        }

        protected override void OnInitialize()
        {
            using (var db = new Database())
            {
                Currencies.AddRange(
                    db.Currencies
                      .OrderBy(x => x.Name)
                      .Select(CurrencyViewModel.CreateFrom));

                People.AddRange(
                    db.People
                    .OrderBy(x => x.Name)
                    .Select(PersonViewModel.CreateFrom));

            }
        }

        public static AccountViewModel CreateFrom(Data.Account basicAccountDetails, BankAccount bankAccountDetails)
        {
            return new AccountViewModel
            {
                AccountId          = basicAccountDetails.Id,
                AccountName        = basicAccountDetails.Name,
                Currency           = CurrencyViewModel.CreateFrom(basicAccountDetails.Currency),
                AccountHolder      = PersonViewModel.CreateFrom(basicAccountDetails.Person),
                BankAccountDetails =
                    bankAccountDetails != null
                        ? new BankAccountViewModel
                        {
                            AccountNumber = new AccountNumberViewModel {Value = bankAccountDetails.AccountNumber},
                            SortCode      = new SortCodeViewModel {Value = bankAccountDetails.SortCode},
                            CreditLimit   = bankAccountDetails.CreditLimit
                        }
                        : null
            };
        }
    }
}