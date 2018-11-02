using System.Linq;
using System.Security.Claims;
using Coin.Data;

namespace Coin.Web.Areas.Accounting
{
    public static class BankAccountExtensions
    {
        public static IQueryable<BankAccount> ForUser(this IQueryable<BankAccount> bankAccounts, ClaimsPrincipal user)
        {
            var userId = user.GetUserId();
            return bankAccounts.Where(x => x.Account.Person.Name == userId);
        }
    }
}