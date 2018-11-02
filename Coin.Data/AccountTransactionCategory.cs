using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class AccountTransactionCategory
    {
        public AccountTransactionCategory()
        {
            AccountTransactionAccountTransactionCategory = new HashSet<AccountTransactionAccountTransactionCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<AccountTransactionAccountTransactionCategory> AccountTransactionAccountTransactionCategory { get; set; }
    }
}
