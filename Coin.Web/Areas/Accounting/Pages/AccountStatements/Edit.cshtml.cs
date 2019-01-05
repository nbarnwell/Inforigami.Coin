using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.AccountStatements
{
    public class EditModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public EditModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccountStatement AccountStatement { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountStatement = await _context.AccountStatement
                .Include(a => a.Account).FirstOrDefaultAsync(m => m.Id == id);

            if (AccountStatement == null)
            {
                return NotFound();
            }
           ViewData["AccountId"] = new SelectList(_context.Account, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AccountStatement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountStatementExists(AccountStatement.Id))
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

        private bool AccountStatementExists(int id)
        {
            return _context.AccountStatement.Any(e => e.Id == id);
        }
    }
}
