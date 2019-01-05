using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.AccountTransactions
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
        ViewData["AccountStatementId"] = new SelectList(_context.AccountStatement, "Id", "Id");
        ViewData["AccountTransactionStatusId"] = new SelectList(_context.AccountTransactionStatus, "Id", "Name");
        ViewData["AccountTransactionTypeId"] = new SelectList(_context.AccountTransactionType, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public AccountTransaction AccountTransaction { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AccountTransaction.Add(AccountTransaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
