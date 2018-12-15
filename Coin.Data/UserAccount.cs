using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            Person = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string Username { get; set; }

        public ICollection<Person> Person { get; set; }
    }
}
