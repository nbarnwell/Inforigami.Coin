using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Person
    {
        public Person()
        {
            Account = new HashSet<Account>();
            PersonHouseholdMembership = new HashSet<PersonHouseholdMembership>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Account> Account { get; set; }
        public ICollection<PersonHouseholdMembership> PersonHouseholdMembership { get; set; }
    }
}
