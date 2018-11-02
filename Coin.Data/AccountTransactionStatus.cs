using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class AccountTransactionStatus
    {
        public AccountTransactionStatus()
        {
            AccountTransaction = new HashSet<AccountTransaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<AccountTransaction> AccountTransaction { get; set; }
    }
}
