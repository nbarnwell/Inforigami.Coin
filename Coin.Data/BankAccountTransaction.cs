﻿using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class BankAccountTransaction
    {
        public int Id { get; set; }
        public int AccountTransactionId { get; set; }
        public int BankSpecificTransactionTypeId { get; set; }
        public string Description { get; set; }

        public AccountTransaction AccountTransaction { get; set; }
    }
}
