using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class AccountStatement
    {
        public AccountStatement()
        {
            AccountTransaction = new HashSet<AccountTransaction>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTimeOffset PeriodStart { get; set; }
        public DateTimeOffset PeriodEnd { get; set; }
        public decimal StartingBalance { get; set; }

        public Account Account { get; set; }
        public ICollection<AccountTransaction> AccountTransaction { get; set; }
    }
}
