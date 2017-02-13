using System;
using System.Linq;
using Caliburn.Micro;
using Coin.Data;
using Coin.Shared;
using Inforigami.Regalo.Core;

namespace Coin.Transactions
{
    public class RecordTransactionHandler : ICommandHandler<RecordTransaction>
    {
        private readonly IEventBus _eventBus;

        public RecordTransactionHandler(IEventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            _eventBus = eventBus;
        }

        public void Handle(RecordTransaction command)
        {
            using (var db = new Database())
            {
                var statement = db.AccountStatements.Find(command.StatementId);

                var accountTransaction = new AccountTransaction
                {
                    AccountTransactionStatusId = (int)AccountTransactionStatus.Recorded,
                    AccountTransactionTypeId = command.TransactionTypeId,
                    Amount = command.CategorySplits.Sum(x => x.Amount),
                    Description = command.Description,
                    Payee = command.Payee,
                    RecordedDate = command.RecordedDate,
                    TransactionTime = command.TransactionTime
                };

                statement.AccountTransactions.Add(accountTransaction);

                foreach (var split in command.CategorySplits)
                {
                    accountTransaction.AccountTransactionAccountTransactionCategories
                                      .Add(
                                          new AccountTransactionAccountTransactionCategory
                                          {
                                              AccountTransactionCategoryId = split.CategoryId,
                                              Amount = split.Amount
                                          });
                }

                // TODO: If this isn't, for some reason, the latest statement, recalculate all statements' starting balances

                db.SaveChanges();

                db.Entry(accountTransaction).Reload();

                _eventBus.Publish(new EntityCreated<AccountTransaction>(accountTransaction));
            }
        }
    }
}