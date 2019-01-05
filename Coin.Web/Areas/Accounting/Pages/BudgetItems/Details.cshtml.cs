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
    public class DetailsModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public DetailsModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public BudgetItem BudgetItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BudgetItem = await _context.BudgetItem
                .Include(b => b.Account)
                .Include(b => b.BankSpecificTransactionType)
                .Include(b => b.Budget)
                .Include(b => b.TimePeriod).FirstOrDefaultAsync(m => m.Id == id);

            if (BudgetItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
