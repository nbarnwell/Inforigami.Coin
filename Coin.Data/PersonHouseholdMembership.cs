using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class PersonHouseholdMembership
    {
        public int PersonId { get; set; }
        public int HouseholdId { get; set; }

        public Household Household { get; set; }
        public Person Person { get; set; }
    }
}
