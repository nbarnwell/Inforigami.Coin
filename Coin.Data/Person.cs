using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Person
    {
        public Person()
        {
            Account = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int UserAccountId { get; set; }
        public int HouseholdId { get; set; }

        public Household Household { get; set; }
        public UserAccount UserAccount { get; set; }
        public ICollection<Account> Account { get; set; }
    }
}
