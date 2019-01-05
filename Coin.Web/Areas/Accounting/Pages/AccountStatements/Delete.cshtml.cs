using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.AccountStatements
{
    public class DeleteModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public DeleteModel(Coin.Data.CoinContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountStatement = await _context.AccountStatement.FindAsync(id);

            if (AccountStatement != null)
            {
                _context.AccountStatement.Remove(AccountStatement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
