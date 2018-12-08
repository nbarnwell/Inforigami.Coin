using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coin.Data;
using Microsoft.EntityFrameworkCore;

namespace Coin.Web.Areas.Accounting.TransactionImport
{
    class TransactionImporter : ITransactionImporter
    {
        private readonly CoinContext _context;

        public TransactionImporter(CoinContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Task<int>> Import(IEnumerable<TransactionImportRow> transactions, int personId)
        {
            var groupedTransactions =
                transactions.GroupBy(x => new { x.AccountNumber, x.SortCode });

            foreach (var transactionGroup in groupedTransactions)
            {
                var bankAccount =
                    await _context.BankAccount
                                  .Where(
                                      x =>
                                          x.AccountNumber == transactionGroup.Key.AccountNumber
                                          && x.SortCode == transactionGroup.Key.SortCode)
                                  .SingleOrDefaultAsync();

                var bankSpecificTransactionTypes =
                    await _context.BankSpecificTransactionType
                                  .ToDictionaryAsync(x => x.Name);

                foreach (var transaction in transactionGroup)
                {
                    // Try to find the transaction
                    var bankAccountTransaction =
                        await _context.BankAccountTransaction.SingleOrDefaultAsync(
                            x =>
                                x.BankId == bankAccount.BankId
                                && x.Description == transaction.TransactionDescription
                                && x.AccountTransaction.TransactionTime.Value.Date == transaction.TransactionDate
                                && x.BankSpecificTransactionType.Name == transaction.TransactionType
                                && x.AccountTransaction.Amount == Math.Max(Math.Abs(transaction.CreditAmount ?? 0), Math.Abs(transaction.DebitAmount ?? 0)));
                    if (bankAccountTransaction == null)
                    {
                        // Load or create the appropriate statement
                        var statement =
                            bankAccount.Account
                                       .AccountStatement
                                       .SingleOrDefault(
                                           x =>
                                               x.PeriodStart <= transaction.TransactionDate
                                               && x.PeriodEnd >= transaction.TransactionDate);

                        if (statement == null)
                        {
                            statement = new AccountStatement();
                            await _context.AccountStatement.AddAsync(statement);
                        }

                        // Look up the budget item it corresponds to
                        var budget =
                            await _context.Budget
                                          .Include(x => x.Household)
                                          .ThenInclude(x => x.Person)
                                          .SingleOrDefaultAsync(x => x.Household.Person.Any(p => p.Id == personId));

                        var budgetItem = 
                            FindBudgetItem(budget, transaction);

                        var newBankAccountTransaction =
                            new BankAccountTransaction
                            {
                                BankId = bankAccount.BankId,
                                BankSpecificTransactionType = bankSpecificTransactionTypes[transaction.TransactionType],
                                Description = transaction.TransactionDescription,
                            };

                        var newAccountTransaction =
                            new AccountTransaction
                            {
                                Amount = GetAmount(transaction)
                            };

                        newAccountTransaction.AccountTransactionBudgetItem.Add(
                            new AccountTransactionBudgetItem
                            {
                                BudgetItem = budgetItem,
                                Amount = newAccountTransaction.Amount
                            });

                        newAccountTransaction.BankAccountTransaction.Add(newBankAccountTransaction);

                        // Create the transaction
                        statement.AccountTransaction.Add(newBankAccountTransaction.AccountTransaction);
                    }
                }
            }

            return _context.SaveChangesAsync();
        }

        private BudgetItem FindBudgetItem(Budget budget, TransactionImportRow transaction)
        {
            throw new NotImplementedException();
        }

        private decimal GetAmount(TransactionImportRow transaction)
        {
            var creditAmount = transaction.CreditAmount ?? 0;
            var debitAmount = transaction.DebitAmount ?? 0;

            if (creditAmount > 0 && debitAmount > 0)
            {
                var e = new InvalidOperationException("Transaction being imported has credit AND debit values.");
                e.Data.Add("Transaction values", transaction.ToString());
                throw e;
            }

            if (creditAmount < 0 || debitAmount < 0)
            {
                var e = new InvalidOperationException("Transaction being imported has negative credit or debit values.");
                e.Data.Add("Transaction values", transaction.ToString());
                throw e;
            }

            // One or the other will be zero, and the other will be non-zero
            return creditAmount + debitAmount;
        }
    }
}