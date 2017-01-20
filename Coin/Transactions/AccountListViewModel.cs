using System;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Transactions
{
    public class AccountListViewModel : Screen
    {
        private readonly IViewModelFactory _viewModelFactory;

        public BindableCollection<Accounts.AccountViewModel> Accounts { get; }

        public override string DisplayName => "Account Statements";

        public AccountListViewModel(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;

            Accounts = new BindableCollection<Accounts.AccountViewModel>();
        }

        protected override void OnInitialize()
        {
            RefreshData();
        }

        public IResult ShowAccount(Accounts.AccountViewModel account)
        {
            var vm = _viewModelFactory.Create<AccountStatementListViewModel>().ForAccountId(account.AccountId);
            return new ShowViewModel(vm);
        }

        public void RefreshData()
        {
            Accounts.Clear();

            using (var db = new Database())
            {
                var q =
                    from a in db.Accounts
                    join ba1 in db.BankAccounts on a.Id equals ba1.AccountId into ba2
                    from ba in ba2.DefaultIfEmpty()
                    orderby a.Name
                    select new { BasicAccountDetails = a, BankAccountDetails = ba };

                var items =
                    q.ToList()
                     .Select(x => Coin.Accounts.AccountViewModel.CreateFrom(x.BasicAccountDetails, x.BankAccountDetails));

                Accounts.AddRange(items);
            }
        }

    }
}
