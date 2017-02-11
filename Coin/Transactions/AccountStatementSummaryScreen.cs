using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Caliburn.Micro;
using Coin.Accounts;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Transactions
{
    public class AccountStatementSummaryScreen : Screen, IHandle<EntityCreated<AccountTransaction>>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IEventAggregator _eventAggregator;
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

        public BindableCollection<TransactionListItem> Transactions { get; }

        public AccountStatementSummaryScreen(IViewModelFactory viewModelFactory, IEventAggregator eventAggregator)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            _viewModelFactory = viewModelFactory;
            _eventAggregator = eventAggregator;

            Transactions = new BindableCollection<TransactionListItem>();

            _eventAggregator.Subscribe(this);
        }

        public AccountStatementSummaryScreen ForAccount(AccountViewModel account)
        {
            Account = account;
            return this;
        }

        public AccountStatementSummaryScreen ForStatement(AccountStatementViewModel statement)
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
                        Statement.Id,
                        vm.SelectedAccountTransactionType.Id,
                        vm.Amount.AsMoney(),
                        vm.Description,
                        vm.Payee,
                        vm.RecordedDate,
                        vm.TransactionTime.GetDateTimeOffset()));
        }

        public void Handle(EntityCreated<AccountTransaction> message)
        {
            using (var db = new Database())
            {
                var newItem =
                    db.AccountTransactions
                      .Where(x => x.Id == message.Entity.Id)
                      .Select(x => new TransactionListItem
                      {
                          Amount = x.Amount,
                          AccountTransactionStatusName = x.AccountTransactionStatus.Name,
                          AccountTransactionTypeName = x.AccountTransactionType.Name,
                          Description = x.Description,
                          Payee = x.Payee,
                          RecordedDate = x.RecordedDate,
                          TransactionTime = x.TransactionTime
                      })
                      .SingleOrDefault();

                Transactions.InsertWhere(
                    x => x.TransactionTime <= newItem.TransactionTime,
                    newItem);
            }
        }

        protected override void OnInitialize()
        {
            using (var db = new Database())
            {
                Transactions.AddRange(
                    db.AccountTransactions
                      .Where(x => x.AccountStatementId == Statement.Id)
                      .OrderByDescending(x => x.TransactionTime)
                      .Select(x => new TransactionListItem
                      {
                          Amount = x.Amount,
                          AccountTransactionStatusName = x.AccountTransactionStatus.Name,
                          AccountTransactionTypeName = x.AccountTransactionType.Name,
                          Description = x.Description,
                          Payee = x.Payee,
                          RecordedDate = x.RecordedDate,
                          TransactionTime = x.TransactionTime
                      }));
            }
        }

        public override void TryClose(bool? dialogResult = null)
        {
            base.TryClose(dialogResult);
            _eventAggregator.Unsubscribe(this);
        }

        public class TransactionListItem
        {
            public decimal Amount { get; set; }
            public string AccountTransactionStatusName { get; set; }
            public string AccountTransactionTypeName { get; set; }
            public string Description { get; set; }
            public string Payee { get; set; }
            public DateTime RecordedDate { get; set; }
            public DateTimeOffset? TransactionTime { get; set; }
        }
    }
}