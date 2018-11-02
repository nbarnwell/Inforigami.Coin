using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class BankSpecificTransactionType
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AccountTransactionTypeId { get; set; }

        public AccountTransactionType AccountTransactionType { get; set; }
        public Bank Bank { get; set; }
    }
}
