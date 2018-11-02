using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class Household
    {
        public Household()
        {
            Person = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Person> Person { get; set; }
    }
}
