using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class BankAccount
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public int AccountId { get; set; }
        public decimal CreditLimit { get; set; }
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }

        public Account Account { get; set; }
        public Bank Bank { get; set; }
    }
}
