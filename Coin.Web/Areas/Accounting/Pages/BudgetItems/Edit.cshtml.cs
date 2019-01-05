using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.BudgetItems
{
    public class EditModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public EditModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Name");
           ViewData["BankSpecificTransactionTypeId"] = new SelectList(_context.BankSpecificTransactionType, "Id", "Description");
           ViewData["BudgetId"] = new SelectList(_context.Budget, "Id", "Name");
           ViewData["TimePeriodId"] = new SelectList(_context.TimePeriod, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BudgetItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetItemExists(BudgetItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BudgetItemExists(int id)
        {
            return _context.BudgetItem.Any(e => e.Id == id);
        }
    }
}
