using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.Budgets
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
        ViewData["HouseholdId"] = new SelectList(_context.Household, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Budget Budget { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Budget.Add(Budget);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}