using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coin.Data;

namespace Coin.Web.Services
{
    public interface IPersonUserConnector
    {
        Task<Person> CreatePerson(string userId, string name);
    }

    public class PersonUserConnector : IPersonUserConnector
    {
        private readonly CoinContext _context;

        public PersonUserConnector(CoinContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Person> CreatePerson(string userId, string name)
        {
            var person = 
                new Person
                    {
                        Name = name,
                        UserAccount =
                                new UserAccount
                                {
                                    Username = userId
                                }
                    };

            await _context.Person.AddAsync(person);

            await _context.SaveChangesAsync();

            return person;
        }
    }
}