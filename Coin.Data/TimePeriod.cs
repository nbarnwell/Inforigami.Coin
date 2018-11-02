using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class TimePeriod
    {
        public TimePeriod()
        {
            Account = new HashSet<Account>();
            BudgetItem = new HashSet<BudgetItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Account> Account { get; set; }
        public ICollection<BudgetItem> BudgetItem { get; set; }
    }
}
