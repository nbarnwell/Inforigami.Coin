using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coin.Data;

namespace Coin.Web.Services
{
    public interface IPersonUserConnector
    {
        Task CreatePerson(string userId, string name);
    }

    public class PersonUserConnector : IPersonUserConnector
    {
        private readonly CoinContext _context;

        public PersonUserConnector(CoinContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreatePerson(string userId, string name)
        {
            await _context.Person.AddAsync(
                new Person
                {
                    Name = name,
                });
        }
    }
}