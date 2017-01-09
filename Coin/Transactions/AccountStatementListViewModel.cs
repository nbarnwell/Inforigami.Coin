using System;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Transactions
{
    public class AccountStatementListViewModel : Screen
    {
        private readonly IViewModelFactory _viewModelFactory;
        private Accounts.AccountViewModel _accountDetails;
        private int _accountId;

        public Accounts.AccountViewModel AccountDetails
        {
            get { return _accountDetails; }
            set
            {
                if (Equals(value, _accountDetails)) return;
                _accountDetails = value;
                NotifyOfPropertyChange(() => AccountDetails);
            }
        }

        public BindableCollection<AccountStatementViewModel> Statements { get; }

        public AccountStatementListViewModel(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;

            Statements = new BindableCollection<AccountStatementViewModel>();
        }

        public AccountStatementListViewModel ForAccountId(int accountId)
        {
            _accountId = accountId;
            return this;
        }

        public IResult ShowStatement(AccountStatementViewModel statement)
        {
            var vm = 
                _viewModelFactory.Create<AccountStatementWithTransactionsViewModel>()
                .ForAccount(AccountDetails)
                .ForStatement(statement);
            return new ShowViewModel(this, vm);
        }

        protected override void OnInitialize()
        {
            using (var db = new Database())
            {
                var accountDetails =
                    db.Accounts
                      .Include("AccountStatements")
                      .Single(x => x.Id == _accountId);

                AccountDetails =
                    Accounts.AccountViewModel.CreateFrom(
                        accountDetails, 
                        accountDetails.BankAccounts.FirstOrDefault());

                Statements.AddRange(
                    accountDetails.AccountStatements
                                  .Select(AccountStatementViewModel.CreateFrom));
            }
        }
    }
}