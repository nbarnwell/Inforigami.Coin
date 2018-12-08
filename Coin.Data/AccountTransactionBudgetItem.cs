using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class AccountTransactionBudgetItem
    {
        public int AccountTransactionId { get; set; }
        public int BudgetItemId { get; set; }
        public decimal Amount { get; set; }

        public AccountTransaction AccountTransaction { get; set; }
        public BudgetItem BudgetItem { get; set; }
    }
}
