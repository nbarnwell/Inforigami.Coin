using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Bank
    {
        public Bank()
        {
            BankAccount = new HashSet<BankAccount>();
            BankSpecificTransactionType = new HashSet<BankSpecificTransactionType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BankAccount> BankAccount { get; set; }
        public ICollection<BankSpecificTransactionType> BankSpecificTransactionType { get; set; }
    }
}
