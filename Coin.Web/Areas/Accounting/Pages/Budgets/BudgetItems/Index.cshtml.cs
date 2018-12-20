using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.Budgets.BudgetItems
{
    public class IndexModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public IndexModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public int BudgetId { get; set; }
        public IList<BudgetItem> BudgetItem { get;set; }

        public async Task OnGetAsync(int budgetId)
        {
            BudgetId = budgetId;

            BudgetItem = await _context.BudgetItem
                .Where(x => x.BudgetId == budgetId)
                .Include(b => b.BankSpecificTransactionType)
                .Include(b => b.Budget)
                .Include(b => b.TimePeriod).ToListAsync();
        }
    }
}
