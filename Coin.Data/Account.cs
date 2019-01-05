using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Account
    {
        public Account()
        {
            AccountStatement = new HashSet<AccountStatement>();
            BankAccount = new HashSet<BankAccount>();
            BudgetItem = new HashSet<BudgetItem>();
            Fund = new HashSet<Fund>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int PersonId { get; set; }
        public int CurrencyId { get; set; }
        public int TimePeriodId { get; set; }

        public Currency Currency { get; set; }
        public Person Person { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public ICollection<AccountStatement> AccountStatement { get; set; }
        public ICollection<BankAccount> BankAccount { get; set; }
        public ICollection<BudgetItem> BudgetItem { get; set; }
        public ICollection<Fund> Fund { get; set; }
    }
}
