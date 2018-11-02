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
    public class DetailsModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public DetailsModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

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
    }
}
