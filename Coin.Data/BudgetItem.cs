using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class BudgetItem
    {
        public BudgetItem()
        {
            AccountTransactionBudgetItem = new HashSet<AccountTransactionBudgetItem>();
        }

        public int Id { get; set; }
        public int BudgetId { get; set; }
        public string Name { get; set; }
        public int TimePeriodId { get; set; }
        public int? AccountId { get; set; }
        public int? BankSpecificTransactionTypeId { get; set; }
        public string TransactionDescriptionMatchPattern { get; set; }
        public decimal AmountLower { get; set; }
        public decimal AmountUpper { get; set; }

        public Account Account { get; set; }
        public BankSpecificTransactionType BankSpecificTransactionType { get; set; }
        public Budget Budget { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public ICollection<AccountTransactionBudgetItem> AccountTransactionBudgetItem { get; set; }
    }
}
