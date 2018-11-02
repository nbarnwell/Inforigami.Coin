using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.AccountTransactionTypes
{
    public class DeleteModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public DeleteModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccountTransactionType AccountTransactionType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountTransactionType = await _context.AccountTransactionType.FirstOrDefaultAsync(m => m.Id == id);

            if (AccountTransactionType == null)
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

            AccountTransactionType = await _context.AccountTransactionType.FindAsync(id);

            if (AccountTransactionType != null)
            {
                _context.AccountTransactionType.Remove(AccountTransactionType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
