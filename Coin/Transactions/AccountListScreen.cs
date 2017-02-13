using System;
using System.Linq;
using Caliburn.Micro;
using Coin.CRUD.Accounts;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Transactions
{
    public class AccountListScreen : Screen
    {
        private readonly IViewModelFactory _viewModelFactory;

        public BindableCollection<AccountViewModel> Accounts { get; }

        public override string DisplayName => "Accounting";

        public AccountListScreen(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;

            Accounts = new BindableCollection<AccountViewModel>();
        }

        protected override void OnActivate()
        {
            RefreshData();
        }

        public IResult ShowAccount(AccountViewModel account)
        {
            var vm = _viewModelFactory.Create<AccountSummaryScreen>().ForAccountId(account.AccountId);
            return new ShowScreen(vm);
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
                     .Select(x => AccountViewModel.CreateFrom(x.BasicAccountDetails, x.BankAccountDetails));

                Accounts.AddRange(items);
            }
        }

    }
}
