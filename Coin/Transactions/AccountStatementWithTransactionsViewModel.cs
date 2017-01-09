using System.Linq;
using Caliburn.Micro;
using Coin.Accounts;
using Coin.Data;
using Coin.Infrastructure;

namespace Coin.Transactions
{
    public class AccountStatementWithTransactionsViewModel : Screen
    {
        private AccountStatementViewModel _statement;
        private AccountViewModel _account;

        public AccountStatementViewModel Statement
        {
            get { return _statement; }
            set
            {
                if (Equals(value, _statement)) return;
                _statement = value;
                NotifyOfPropertyChange(() => Statement);
            }
        }

        public AccountViewModel Account
        {
            get { return _account; }
            set
            {
                if (Equals(value, _account)) return;
                _account = value;
                NotifyOfPropertyChange(() => Account);
            }
        }

        public BindableCollection<AccountTransactionViewModel> Transactions { get; }

        public AccountStatementWithTransactionsViewModel()
        {
            Transactions = new BindableCollection<AccountTransactionViewModel>();
        }

        public AccountStatementWithTransactionsViewModel ForAccount(AccountViewModel account)
        {
            Account = account;
            return this;
        }

        public AccountStatementWithTransactionsViewModel ForStatement(AccountStatementViewModel statement)
        {
            Statement = statement;
            return this;
        }

        protected override void OnInitialize()
        {
            using (var db = new Database())
            {
                Transactions.AddRange(
                    db.AccountTransactions
                      .Where(x => x.AccountStatementId == Statement.Id)
                      .Select(AccountTransactionViewModel.CreateFrom));
            }
        }
    }
}