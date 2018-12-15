using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Household
    {
        public Household()
        {
            Budget = new HashSet<Budget>();
            PersonHouseholdMembership = new HashSet<PersonHouseholdMembership>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Budget> Budget { get; set; }
        public ICollection<PersonHouseholdMembership> PersonHouseholdMembership { get; set; }
    }
}
