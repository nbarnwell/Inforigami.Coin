using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.AccountTransactions
{
    public class IndexModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public IndexModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public IList<AccountTransaction> AccountTransaction { get;set; }

        public async Task OnGetAsync()
        {
            AccountTransaction = await _context.AccountTransaction
                .Include(a => a.AccountStatement)
                .Include(a => a.AccountTransactionStatus)
                .Include(a => a.AccountTransactionType).ToListAsync();
        }
    }
}
