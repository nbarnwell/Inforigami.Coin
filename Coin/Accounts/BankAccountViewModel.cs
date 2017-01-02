using System.Linq;
using Caliburn.Micro;
using Coin.Banking;
using Coin.Data;

namespace Coin.Accounts
{
    public class BankAccountViewModel : Screen
    {
        private BankViewModel _selectedBank;
        private decimal _creditLimit;
        private AccountNumberViewModel _accountNumber;
        private SortCodeViewModel _sortCode;

        public BankViewModel SelectedBank
        {
            get { return _selectedBank; }
            set
            {
                if (Equals(value, _selectedBank)) return;
                _selectedBank = value;
                NotifyOfPropertyChange(() => SelectedBank);
            }
        }

        public decimal CreditLimit
        {
            get { return _creditLimit; }
            set
            {
                if (Equals(value, _creditLimit)) return;
                _creditLimit = value;
                NotifyOfPropertyChange(() => CreditLimit);
            }
        }

        public AccountNumberViewModel AccountNumber
        {
            get { return _accountNumber; }
            set
            {
                if (Equals(value, _accountNumber)) return;
                _accountNumber = value;
                NotifyOfPropertyChange(() => AccountNumber);
            }
        }

        public SortCodeViewModel SortCode
        {
            get { return _sortCode; }
            set
            {
                if (Equals(value, _sortCode)) return;
                _sortCode = value;
                NotifyOfPropertyChange(() => SortCode);
            }
        }

        public BindableCollection<BankViewModel> Banks { get; }

        public BankAccountViewModel()
        {
            Banks = new BindableCollection<BankViewModel>();

            AccountNumber = new AccountNumberViewModel();
            SortCode = new SortCodeViewModel();
        }

        protected override void OnInitialize()
        {
            using (var db = new Database())
            {
                Banks.AddRange(
                    db.Banks
                      .OrderBy(x => x.Name)
                      .Select(BankViewModel.CreateFrom));
            }
        }
    }
}