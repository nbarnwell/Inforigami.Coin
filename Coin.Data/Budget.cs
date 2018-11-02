using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Budget
    {
        public Budget()
        {
            BudgetItem = new HashSet<BudgetItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BudgetItem> BudgetItem { get; set; }
    }
}
