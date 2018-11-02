using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Currency
    {
        public Currency()
        {
            Account = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<Account> Account { get; set; }
    }
}
