using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class AccountTransactionAccountTransactionCategory
    {
        public int AccountTransactionId { get; set; }
        public int AccountTransactionCategoryId { get; set; }
        public decimal Amount { get; set; }

        public AccountTransaction AccountTransaction { get; set; }
        public AccountTransactionCategory AccountTransactionCategory { get; set; }
    }
}
