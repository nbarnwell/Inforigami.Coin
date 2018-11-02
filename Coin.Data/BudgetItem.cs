using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int TimePeriodId { get; set; }

        public Budget Budget { get; set; }
        public TimePeriod TimePeriod { get; set; }
    }
}
