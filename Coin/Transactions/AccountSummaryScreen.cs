using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Infrastructure;
using Coin.Shared;

namespace Coin.Transactions
{
    public class AccountSummaryScreen : Screen
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

        public AccountSummaryScreen(IViewModelFactory viewModelFactory)
        {
            if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
            _viewModelFactory = viewModelFactory;

            Statements = new BindableCollection<AccountStatementViewModel>();
        }

        public AccountSummaryScreen ForAccountId(int accountId)
        {
            _accountId = accountId;
            return this;
        }

        public IResult ShowStatement(AccountStatementViewModel statement)
        {
            var vm = 
                _viewModelFactory.Create<AccountStatementSummaryScreen>()
                .ForAccount(AccountDetails)
                .ForStatement(statement);
            return new ShowScreen(vm);
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

                EnsureStatementsExist(accountDetails);

                Statements.AddRange(
                    accountDetails.AccountStatements
                                  .Select(AccountStatementViewModel.CreateFrom));

                db.SaveChanges();
            }
        }

        private void EnsureStatementsExist(Account account)
        {
            var statements = account.AccountStatements;

            var now = DateTimeOffset.Now;

            if (!statements.Any())
            {
                // Create one for the current month
                var thisMonth = now.AddDays(0 - now.Day).Add(now.TimeOfDay);
                var nextMonth = thisMonth.AddMonths(1);

                var newStatement =
                    new Data.AccountStatement
                    {
                        Account = account,
                        PeriodStart = thisMonth,
                        PeriodEnd = nextMonth,
                        StartingBalance = 0
                    };
                statements.Add(newStatement);
            }
            else
            {
                var mostRecentStatement = statements.Last();

                if (!now.IsBetween(mostRecentStatement.PeriodStart, mostRecentStatement.PeriodEnd))
                {
                    foreach (var monthStart in MonthsSince(mostRecentStatement.PeriodEnd))
                    {
                        var newStatement =
                            new Data.AccountStatement
                            {
                                Account = account,
                                PeriodStart = monthStart,
                                PeriodEnd = monthStart.AddMonths(1),
                                StartingBalance =
                                    mostRecentStatement.StartingBalance +
                                    mostRecentStatement.AccountTransactions.Sum(x => x.Amount)
                            };
                        account.AccountStatements.Add(newStatement);
                    }
                }
            }
        }

        private IEnumerable<DateTimeOffset> MonthsSince(DateTimeOffset endOfMostRecentStatement)
        {
            DateTimeOffset current = endOfMostRecentStatement;
            DateTimeOffset nextMonth = DateTimeOffset.Now.AddDays(0 - DateTimeOffset.Now.Day).AddMonths(1);

            while (current < nextMonth)
            {
                yield return current;
                current = current.AddMonths(1);
            }
        }
    }
}