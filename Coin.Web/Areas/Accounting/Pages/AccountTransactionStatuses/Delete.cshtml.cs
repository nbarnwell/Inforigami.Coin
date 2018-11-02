using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.AccountTransactionStatuses
{
    public class DeleteModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public DeleteModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccountTransactionStatus AccountTransactionStatus { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountTransactionStatus = await _context.AccountTransactionStatus.FirstOrDefaultAsync(m => m.Id == id);

            if (AccountTransactionStatus == null)
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

            AccountTransactionStatus = await _context.AccountTransactionStatus.FindAsync(id);

            if (AccountTransactionStatus != null)
            {
                _context.AccountTransactionStatus.Remove(AccountTransactionStatus);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
