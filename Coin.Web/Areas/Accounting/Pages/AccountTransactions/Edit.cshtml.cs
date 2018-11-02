using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.AccountTransactions
{
    public class EditModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public EditModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccountTransaction AccountTransaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountTransaction = await _context.AccountTransaction
                .Include(a => a.AccountStatement)
                .Include(a => a.AccountTransactionStatus)
                .Include(a => a.AccountTransactionType).FirstOrDefaultAsync(m => m.Id == id);

            if (AccountTransaction == null)
            {
                return NotFound();
            }
           ViewData["AccountStatementId"] = new SelectList(_context.AccountStatement, "Id", "Id");
           ViewData["AccountTransactionStatusId"] = new SelectList(_context.AccountTransactionStatus, "Id", "Name");
           ViewData["AccountTransactionTypeId"] = new SelectList(_context.AccountTransactionType, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AccountTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountTransactionExists(AccountTransaction.Id))
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

        private bool AccountTransactionExists(int id)
        {
            return _context.AccountTransaction.Any(e => e.Id == id);
        }
    }
}
