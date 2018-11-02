using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class AccountTransactionType
    {
        public AccountTransactionType()
        {
            AccountTransaction = new HashSet<AccountTransaction>();
            BankSpecificTransactionType = new HashSet<BankSpecificTransactionType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIncome { get; set; }

        public ICollection<AccountTransaction> AccountTransaction { get; set; }
        public ICollection<BankSpecificTransactionType> BankSpecificTransactionType { get; set; }
    }
}
