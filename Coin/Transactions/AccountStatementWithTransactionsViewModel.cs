using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Coin.Accounts;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Transactions
{
    public class AccountStatementWithTransactionsViewModel : Screen
    {
        private readonly IViewModelFactory _viewModelFactory;
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

        public BindableCollection<AccountTransactionEditViewModel> Transactions { get; }

        public AccountStatementWithTransactionsViewModel(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;

            Transactions = new BindableCollection<AccountTransactionEditViewModel>();
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

        public IEnumerable<IResult> Add()
        {
            var vm = _viewModelFactory.Create<AccountTransactionEditViewModel>().ForAccount(Account);
            yield return new ShowDialog(vm);

            yield return 
                new ProcessCommand(
                    new RecordTransaction(
                        vm.SelectedAccountTransactionType.Id,
                        vm.Amount.AsMoney(),
                        vm.Description,
                        vm.Payee,
                        vm.RecordedDate,
                        vm.TransactionTime.GetDateTimeOffset()));
        }

        public void Close()
        {
            TryClose();
        }

        protected override void OnInitialize()
        {
            using (var db = new Database())
            {
                Transactions.AddRange(
                    db.AccountTransactions
                      .Where(x => x.AccountStatementId == Statement.Id)
                      .Select(AccountTransactionEditViewModel.CreateFrom));
            }
        }
    }
}