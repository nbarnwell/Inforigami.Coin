using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.BankSpecificTransactionTypes
{
    public class DeleteModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public DeleteModel(Coin.Data.CoinContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BankSpecificTransactionType = await _context.BankSpecificTransactionType.FindAsync(id);

            if (BankSpecificTransactionType != null)
            {
                _context.BankSpecificTransactionType.Remove(BankSpecificTransactionType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
