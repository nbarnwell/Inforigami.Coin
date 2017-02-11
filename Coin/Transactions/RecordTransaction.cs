using System;
using Coin.Shared;
using Inforigami.Regalo.Interfaces;

namespace Coin.Transactions
{
    public class RecordTransaction : Command
    {
        public int StatementId { get; set; }
        public int TransactionTypeId { get; set; }
        public Money Amount { get; set; }
        public string Description { get; set; }
        public string Payee { get; set; }
        public DateTime RecordedDate { get; set; }
        public DateTimeOffset? TransactionTime { get; set; }

        public RecordTransaction(int statementId, int transactionTypeId, Money amount, string description, string payee, DateTime recordedDate, DateTimeOffset? transactionTime)
        {
            StatementId = statementId;
            TransactionTypeId = transactionTypeId;
            Amount = amount;
            Description = description;
            Payee = payee;
            RecordedDate = recordedDate;
            TransactionTime = transactionTime;
        }
    }
}