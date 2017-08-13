using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Coin.CRUD.Accounts;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Accounting
{
    public class AccountListScreen : Screen, IHandle<RefreshRequested>, IHandle<EntityCreated<Data.Account>>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IEventAggregator _events;

        public BindableCollection<AccountViewModel> Accounts { get; }

        public override string DisplayName => "Accounting";

        public AccountListScreen(IViewModelFactory viewModelFactory, IEventAggregator events)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            if (events == null) throw new ArgumentNullException(nameof(events));

            _viewModelFactory = viewModelFactory;
            _events = events;

            Accounts = new BindableCollection<AccountViewModel>();
            _events.Subscribe(this);
        }

        protected override void OnActivate()
        {
            RefreshData();
        }

        public override void TryClose(bool? dialogResult = null)
        {
            _events.Unsubscribe(this);
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

        public IEnumerable<IResult> AddAccount()
        {
            var accountViewModel = _viewModelFactory.Create<AccountViewModel>();
            var showDialog = new ShowDialog(accountViewModel);
            yield return showDialog;

            if (showDialog.Result == true)
            {
                using (var db = new Database())
                {
                    var entity =
                        new Data.Account
                        {
                            Name = accountViewModel.AccountName,
                            PersonId = accountViewModel.AccountHolder.PersonId,
                            CurrencyId = accountViewModel.Currency.Id
                        };

                    // TODO: If it's a bank account, add the bank account details
                    if (accountViewModel.IsBankAccount)
                    {
                        var bankDetails = accountViewModel.BankAccountDetails;
                        entity.BankAccounts.Add(
                            new BankAccount
                            {
                                BankId = bankDetails.SelectedBank.BankId,
                                CreditLimit = bankDetails.CreditLimit,
                                AccountNumber = bankDetails.AccountNumber.Value,
                                SortCode = bankDetails.SortCode.Value
                            });
                    }

                    db.Accounts.Add(entity);

                    db.SaveChanges();

                    db.Entry(entity).Reload();

                    _events.PublishOnUIThread(new EntityCreated<Data.Account>(entity));
                }
            }
        }

        public void Handle(RefreshRequested message)
        {
            RefreshData();
        }

        public void Handle(EntityCreated<Account> message)
        {
            var vm = AccountViewModel.CreateFrom(message.Entity, message.Entity.BankAccounts.FirstOrDefault());
            Accounts.InsertWhere(
                x => string.Compare(x.AccountName, vm.AccountName, StringComparison.Ordinal) >= 0,
                vm);
        }
    }
}
