using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class BankSpecificTransactionType
    {
        public BankSpecificTransactionType()
        {
            BankAccountTransaction = new HashSet<BankAccountTransaction>();
            BudgetItem = new HashSet<BudgetItem>();
        }

        public int Id { get; set; }
        public int BankId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AccountTransactionTypeId { get; set; }

        public AccountTransactionType AccountTransactionType { get; set; }
        public Bank Bank { get; set; }
        public ICollection<BankAccountTransaction> BankAccountTransaction { get; set; }
        public ICollection<BudgetItem> BudgetItem { get; set; }
    }
}
