using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Fund
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }

        public Account Account { get; set; }
    }
}
