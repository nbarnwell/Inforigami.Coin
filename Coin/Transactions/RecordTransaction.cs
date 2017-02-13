using System;
using System.Collections.Generic;
using Coin.Shared;
using Inforigami.Regalo.Interfaces;

namespace Coin.Transactions
{
    public class RecordTransaction : Command
    {
        public int                        StatementId       { get; private set; }
        public int                        TransactionTypeId { get; private set; }
        public string                     Description       { get; private set; }
        public string                     Payee             { get; private set; }
        public DateTime                   RecordedDate      { get; private set; }
        public DateTimeOffset?            TransactionTime   { get; private set; }
        public IEnumerable<CategorySplit> CategorySplits    { get; private set; }

        public RecordTransaction(
            int statementId,
            int transactionTypeId,
            string description,
            string payee,
            DateTime recordedDate,
            DateTimeOffset? transactionTime,
            IEnumerable<CategorySplit> categorySplits)
        {
            StatementId = statementId;
            TransactionTypeId = transactionTypeId;
            Description = description;
            Payee = payee;
            RecordedDate = recordedDate;
            TransactionTime = transactionTime;
            CategorySplits = categorySplits;
        }

        public class CategorySplit
        {
            public int CategoryId { get; private set; }
            public decimal Amount { get; private set; }

            public CategorySplit(int categoryId, decimal amount)
            {
                CategoryId = categoryId;
                Amount = amount;
            }
        }
    }
}