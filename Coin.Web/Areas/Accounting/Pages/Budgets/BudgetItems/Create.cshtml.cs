﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.Budgets.BudgetItems
{
    public class CreateModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public CreateModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int budgetId)
        {
            ViewData["BankSpecificTransactionTypeId"] = new SelectList(_context.BankSpecificTransactionType, "Id", "Description");
            ViewData["BudgetId"] = new SelectList(_context.Budget, "Id", "Name", budgetId);
            ViewData["TimePeriodId"] = new SelectList(_context.TimePeriod, "Id", "Name");
            BudgetId = budgetId;
            return Page();
        }

        [BindProperty]
        public BudgetItem BudgetItem { get; set; }

        public int BudgetId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BudgetItem.Add(BudgetItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { budgetId = BudgetItem.BudgetId });
        }
    }
}