using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.BankSpecificTransactionTypes
{
    public class CreateModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public CreateModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AccountTransactionTypeId"] = new SelectList(_context.AccountTransactionType, "Id", "Name");
        ViewData["BankId"] = new SelectList(_context.Bank, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public BankSpecificTransactionType BankSpecificTransactionType { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BankSpecificTransactionType.Add(BankSpecificTransactionType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
