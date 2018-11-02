using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.BankSpecificTransactionTypes
{
    public class EditModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public EditModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BankSpecificTransactionType BankSpecificTransactionType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BankSpecificTransactionType = await _context.BankSpecificTransactionType
                .Include(b => b.AccountTransactionType)
                .Include(b => b.Bank).FirstOrDefaultAsync(m => m.Id == id);

            if (BankSpecificTransactionType == null)
            {
                return NotFound();
            }
           ViewData["AccountTransactionTypeId"] = new SelectList(_context.AccountTransactionType, "Id", "Name");
           ViewData["BankId"] = new SelectList(_context.Bank, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BankSpecificTransactionType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankSpecificTransactionTypeExists(BankSpecificTransactionType.Id))
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

        private bool BankSpecificTransactionTypeExists(int id)
        {
            return _context.BankSpecificTransactionType.Any(e => e.Id == id);
        }
    }
}
