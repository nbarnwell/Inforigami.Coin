﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Pages.Households
{
    public class EditModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public EditModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Household Household { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Household = await _context.Household.FirstOrDefaultAsync(m => m.Id == id);

            if (Household == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Household).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HouseholdExists(Household.Id))
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

        private bool HouseholdExists(int id)
        {
            return _context.Household.Any(e => e.Id == id);
        }
    }
}
