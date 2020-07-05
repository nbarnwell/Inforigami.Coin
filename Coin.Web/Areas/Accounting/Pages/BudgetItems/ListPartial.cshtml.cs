using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.BudgetItems
{
    public class ListPartialModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public ListPartialModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public IList<BudgetItem> BudgetItem { get;set; }

        public async Task OnGetAsync()
        {
            //BudgetItem = await _context.BudgetItem
            //    .Include(b => b.Account)
            //    .Include(b => b.BankSpecificTransactionType)
            //    .Include(b => b.Budget)
            //    .Include(b => b.TimePeriod).ToListAsync();
        }
    }
}
