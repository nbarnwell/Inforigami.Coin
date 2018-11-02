using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class AccountTransaction
    {
        public AccountTransaction()
        {
            AccountTransactionAccountTransactionCategory = new HashSet<AccountTransactionAccountTransactionCategory>();
        }

        public int Id { get; set; }
        public int? AccountStatementId { get; set; }
        public DateTimeOffset? TransactionTime { get; set; }
        public DateTime RecordedDate { get; set; }
        public int AccountTransactionStatusId { get; set; }
        public decimal Amount { get; set; }
        public string Payee { get; set; }
        public string Description { get; set; }
        public int AccountTransactionTypeId { get; set; }

        public AccountStatement AccountStatement { get; set; }
        public AccountTransactionStatus AccountTransactionStatus { get; set; }
        public AccountTransactionType AccountTransactionType { get; set; }
        public ICollection<AccountTransactionAccountTransactionCategory> AccountTransactionAccountTransactionCategory { get; set; }
    }
}
